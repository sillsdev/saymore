using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Palaso.CommandLineProcessing;


namespace SayMoreTests
{
	/// <summary>
	/// I really need this feature now, so I'm just going to hack it into a test which does what I want
	/// and someday we can make it a real feature
	/// </summary>
	[TestFixture]
	public class ShrinkProject
	{
		private ConsoleProgress _progress;

		[SetUp]
		public void Setup()
		{
		   _progress = new ConsoleProgress();
			_progress.ShowVerbose = true;
		}

		[Test]
		public void ShrinkFolder()
		{
			foreach (var directory in Directory.GetDirectories(@"C:\Users\John\Documents\Language Data\edolo\SmallEdolo"))
			{
				foreach (var sub in Directory.GetDirectories(directory))
				{
					foreach (var file in Directory.GetFiles(sub,("*.MOV")))
					{
					   //if(!file.Contains("-small"))
						   // ShrinkVideo(file);
					}
					foreach (var file in Directory.GetFiles(sub, ("*.jpg")))
					{
//                        if(!file.Contains("-small"))
//                            ShrinkPicture(file);
					}
					foreach (var file in Directory.GetFiles(sub, ("*.wav")))
					{
						if(!file.Contains("-small"))
							ShrinkAudio(file);
					}
				}
			}
		}

		private void ShrinkAudio(string file)
		{
			Debug.WriteLine("ShrinkAudio " + file);

			var extension = Path.GetExtension(file);
			var newPath = file.Replace(extension, "-small.mp3");
			if(File.Exists(newPath))
				File.Delete(newPath);

			Palaso.Media.FFmpegRunner.MakeLowQualityCompressedAudio(file, newPath,
																   _progress);
			if(File.Exists(file+".meta"))
				File.Move(file+".meta", newPath+".meta");
		}

		private void ShrinkPicture(string file)
		{
			Debug.WriteLine("ShrinkPicture " + file);
			var extension = Path.GetExtension(file);
			var newPath = file.Replace(extension, "-small.jpg");
			if (File.Exists(newPath))
				File.Delete(newPath);
			Palaso.Media.FFmpegRunner.MakeLowQualitySmallPicture(file, newPath,
																   _progress);
			if (File.Exists(file + ".meta"))
				File.Move(file + ".meta", newPath + ".meta");
		}

		private void ShrinkVideo(string file)
		{
			Debug.WriteLine("ShrinkVIdeo "+file);

			var extension = Path.GetExtension(file);

			var newPath = file.Replace(extension, "-small.mp4");
			if (File.Exists(newPath))
				File.Delete(newPath);
			var result = Palaso.Media.FFmpegRunner.MakeLowQualitySmallVideo(file, newPath,
																	_progress);
			if (File.Exists(file + ".meta"))
				File.Move(file + ".meta", newPath + ".meta");
		}
	}
}
