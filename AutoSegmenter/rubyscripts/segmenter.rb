require 'pp'

# Should we stop 0.3 seconds before each start?
#
class Segmenter

  attr_reader :thresholds,
			  :cluster_timespan,
			  :count_threshold,
			  :silence_threshold,
			  :cluster_decay,
			  :aubioonsetPath

  def initialize options = {}, &cluster_decay
	@thresholds, @cluster_timespan, @count_threshold, @silence_threshold = extract_options options
	@cluster_decay = cluster_decay || lambda { |last| last * 0.75 }
  end

  def extract_options options = {}
	[
	  options[:thresholds],
	  options[:cluster],
	  options[:threshold],
	  options[:silence]
	]
  end

  def process filename
	puts "Usage: ruby test.rb <filename>" or exit unless filename

	onsets = []

	thresholds.each do |threshold|
	  onsets += extract_from filename, silence: silence_threshold, threshold: threshold
	end

	onsets = prepare onsets
	onsets = cluster onsets, cluster_timespan, &cluster_decay
	onsets = threshold onsets, count_threshold
	onsets = normalize onsets

	onsets
  end

  def extract_from filename, options = {}
	aubioPath = (aubioonsetPath || "") + "aubioonset"
	silence   = options[:silence]   || -40
	threshold = options[:threshold] || 1.5
	`"#{aubioPath}" -a -H 256 -s #{silence} -t #{threshold} -i "#{filename}"`.split("\n")
  end

  def prepare onsets
	# This is the original code. The round method was causing an argument error. I
	# don't know why because there's a call to round(2) further down that doesn't
	# cause a problem (unless I haven't yet found a case when that code is executed).
	# onsets.collect! { |onset| onset.to_f.round(2) }

	# Therefore, remove the rounding since the C# side of the process that
	# interprets the results of the segmenting process can easily do the rounding.
	onsets.collect! { |onset| onset.to_f }
	onsets.sort!
  end

  def cluster onsets, starting = 1.0, &decay
	last_onset = -starting
	last_starting = starting.to_f

	decay ||= lambda { |last| last }

	onsets.reduce({}) do |clustered, onset|
	  if onset < last_onset + last_starting
		clustered[last_onset] ||= 0
		clustered[last_onset] += 1
	  else
		clustered[onset] ||= 0
		clustered[onset] += 1
		last_onset = onset
		last_starting = starting
	  end

	  last_starting = decay.call last_starting

	  clustered
	end
  end

  def threshold onsets, count_threshold = 1
	onsets.select { |time, count| count > count_threshold }
  end

  def normalize onsets
	max = onsets.inject(0) do |max, (time, value)|
	  max < value ? value : max
	end
	onsets.inject({}) do |result, (time, value)|
	  result[time] = value.to_f / max
	  result
	end
  end

  def shift onsets, offset = 0.3
	onsets.map do |time, occurrences|
	  [(time-offset).round(2), occurrences]
	end
  end

  def cache_key
	[thresholds, silence_threshold, count_threshold, cluster_timespan]
  end

  def to_s
	"Segmenter(#{thresholds.join(', ')}; #{silence_threshold}; #{count_threshold}/#{cluster_timespan})"
  end

  # ==========================================================
  # These two methods added by DDO for calling from C#
  # ==========================================================

  def initFromCSharp silence, cluster, threshold, thresholds
	@silence_threshold = silence
	@cluster_timespan = cluster
	@count_threshold = threshold
	@thresholds = thresholds
	@cluster_decay = lambda { |last| last * 0.75 }
  end

  def processFromCSharp aubioonsetPath, audioFile
	@aubioonsetPath = aubioonsetPath
	process audioFile
  end

end