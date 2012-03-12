using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using IronRuby;
using Palaso.IO;

namespace AutoSegmenter
{
	public class AutoSegmenter
	{
		/// ------------------------------------------------------------------------------------
		public static Dictionary<double, double> SegmentAudioFile(string audioFile,
			int silenceThreshold, float clusterDuration, int clusterThreshold,
			float[] onsetDetectionValues)
		{
			var rubyScript =
				FileLocator.GetFileDistributedWithApplication("AutoSegmenter", "rubyscripts", "segmenter.rb");

			var libAubioFolder = Path.GetDirectoryName(
				FileLocator.GetFileDistributedWithApplication("AutoSegmenter", "libaudio", "aubioonset.exe"));

			if (!libAubioFolder.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))
				libAubioFolder += Path.DirectorySeparatorChar;

			var runtime = Ruby.CreateRuntime();
			var engine = runtime.GetEngine("rb");
			var paths = engine.GetSearchPaths().ToList();
			paths.Add(Path.GetDirectoryName(rubyScript));
			engine.SetSearchPaths(paths);

			engine.Runtime.ExecuteFile(rubyScript);
			var segmenterObj = engine.Runtime.Globals.GetVariable("Segmenter");
			var segmenterInst = engine.Operations.CreateInstance(segmenterObj);

			engine.Operations.InvokeMember(segmenterInst, "initFromCSharp",
				silenceThreshold, clusterDuration, clusterThreshold, onsetDetectionValues);

			IronRuby.Builtins.Hash returnValues = engine.Operations.InvokeMember(segmenterInst,
				"processFromCSharp", libAubioFolder, audioFile);

			return returnValues.ToDictionary(kvp => (double)kvp.Key, kvp => (double)kvp.Value);
		}
	}
}
