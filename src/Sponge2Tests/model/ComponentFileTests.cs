using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Palaso.TestUtilities;
using Sponge2.Model;
using Autofac;
using Autofac.Core;


namespace SpongeTests.model
{
	[TestFixture]
	public class ComponentFileTests
	{
		private TemporaryFolder _parentFolder;

		[SetUp]
		public void Setup()
		{
			_parentFolder = new TemporaryFolder("fileTypeTest");
		}

		[TearDown]
		public void TearDown()
		{
			_parentFolder.Dispose();
			_parentFolder = null;
		}

		[Test]
		public void CollectionResolution()
		{
			var builder = new ContainerBuilder();

			//including this line makes the Resolve fail
			builder.RegisterType<FileType>();

			builder.RegisterInstance(FileType.Create("video", new[] { ".avi", ".mov", ".mp4" }));
			builder.RegisterInstance(FileType.Create("image", new[] { ".jpg", ".tiff", ".bmp" }));
			builder.RegisterInstance(FileType.Create("audio", new[] { ".mp3", ".wav", ".ogg" }));

			var container = builder.Build();
			var enumerable = container.Resolve<IEnumerable<FileType>>();
			Assert.AreEqual(3,enumerable.Count());
		}

//		[Test]
//		public void temporaryAutofacTest2()
//		{
//
//			var builder = new ContainerBuilder();
//
//				builder.Register(c =>
//				                 	{
//				                 		var p = c.Resolve<Project>(new Parameter[] {new PositionalParameter(0, _projectPath)});
//				                 		return p;
//				                 	});
//
//
//				builder.RegisterInstance(FileType.Create("video", new[] {".avi", ".mov", ".mp4"}));
//				builder.RegisterInstance(FileType.Create("image", new[] {".jpg", ".tiff", ".bmp"}));
//				builder.RegisterInstance(FileType.Create("audio", new[] {".mp3", ".wav", ".ogg"}));
//
//				var container = builder.Build();
//
//				var enumerable = container.Resolve<IEnumerable<FileType>>();
//
//
//		}
		[Test]
		public void GetFileType_IsText_GivesTextFileType()
		{
			ComponentFile f = CreateComponentFile("abc.txt");
			Assert.AreEqual("Text",f.GetFileType().Name);
		}

		private ComponentFile CreateComponentFile(string path)
		{
			return new ComponentFile(path, new FileType[]{FileType.Create("Text", ".txt"), });
		}

		[Test]
		public void GetFileType_UnknownType_UnknownFileType()
		{
			ComponentFile f = CreateComponentFile("abc.txt");
			Assert.AreEqual("Unknown", f.GetFileType().Name);
		}

	}
}
