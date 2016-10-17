using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using L10NSharp;
using SayMore.Model.Fields;

namespace SayMore.Model.Files
{
	public class ExportSessionsCommand: ExportCommand
	{
		public ExportSessionsCommand(ElementRepository<Session> sessions)
			: base("exportSessions", sessions.AllItems.ToArray())
		{
		}
	}
	public class ExportPeoplCommand : ExportCommand
	{
		public ExportPeoplCommand(ElementRepository<Person> people)
			: base("exportPeople", people.AllItems.ToArray())
		{
		}
	}

	/// <summary>
	/// Exports metadata to file (currently, only csv)
	/// </summary>
	public class ExportCommand :Command
	{
		private readonly IEnumerable<ProjectElement> _elements;
		private char _delimeter = ',';

		public ExportCommand(string id, IEnumerable<ProjectElement> elements)
			: base(id)
		{
			_elements = elements;
		}

		/// <summary>
		/// for tests only
		/// </summary>
		public ExportCommand()
			: base("export")
		{
		}

		public override void Execute()
		{
			DoExport(_elements.ToArray());
		}

		public void DoExport(IEnumerable<ProjectElement> elements)
		{
			using(var dlg = new SaveFileDialog())
			{
				dlg.RestoreDirectory = true;
				dlg.Title = LocalizationManager.GetString("MainWindow.Export.ExportFileSaveFileDlg.Caption", "Export Data");
				dlg.AddExtension = true;
				dlg.AutoUpgradeEnabled = true;
				dlg.Filter = LocalizationManager.GetString("MainWindow.Export.ExportFileSaveFileDlg.CSVFileTypeText", "CSV (Comma delimited) (*.csv)|*.csv");
				if (DialogResult.OK == dlg.ShowDialog())
				{
					DoExport(elements, dlg.FileName);
					Process.Start("explorer.exe", "/select, \"" + dlg.FileName + "\"");
				}
			}
		}

		private void DoExport(IEnumerable<ProjectElement> elements, string path)
		{
			try
			{
				File.WriteAllText(path, GetFileString(elements));
			}
			catch(Exception e)
			{
				var msg = LocalizationManager.GetString("MainWindow.Export.ExportFailureMsg",
					"Something went wrong with the export.");
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(e, msg);
			}

#if GenericCsvExperiment	//	See: https://trello.com/c/xb6dN2I9/193-more-complete-csv-export
			//now output a file containing metadata for all the files each of those elements (e.g. all the files in the sessions, or all the files for each person)
			try
			{
				var fileMetaDataPath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + "-componentFiles.csv");
				File.WriteAllText(fileMetaDataPath, GetCsvStringOfComponentFileMetadata(elements));
			}
			catch (Exception e)
			{
				var msg = LocalizationManager.GetString("MainWindow.Export.ExportFailureMsg",
					"Something went wrong with the export.");
				SIL.Reporting.ErrorReport.NotifyUserOfProblem(e, msg);
			}
#endif
		}

#if GenericCsvExperiment	//	See: https://trello.com/c/xb6dN2I9/193-more-complete-csv-export
		//Get the meta data of the component files (e.g. sound, image, transcription) of a ProjectElement (i.e. session or person) as a CSV
		private string GetCsvStringOfComponentFileMetadata(IEnumerable<ProjectElement> elements)
		{
			var builder = new StringBuilder();
			foreach (var projectElement in elements)
			{
				foreach(var componentFile in projectElement.GetComponentFiles())
				{
					//review: this assumes that all the metadata we are interested in is available (and only available) via a sidecar xml file, rather than the original.

					var sidecarFilePath = componentFile.FileType.GetMetaFilePath(componentFile.PathToAnnotatedFile);// componentFile.GetAnnotationFile());
					if (!File.Exists(sidecarFilePath))
						continue;

					//Review. Doing this becuase if it's not a meta, this serializer thing isn't going to be able to read it
					if (!sidecarFilePath.EndsWith(Settings.Default.MetadataFileExtension))
						continue;

					var metaDataFields = new List<FieldInstance>();
					componentFile.XmlFileSerializer.Load(metaDataFields, sidecarFilePath, componentFile.RootElementName, componentFile.FileType);
					builder.Append(componentFile.FileName + ", ");//first column is the name of the file
					foreach (var field in metaDataFields)
					{
						builder.Append(field.ValueAsString+", ");
					}
					builder.AppendLine();
				}
				builder.AppendLine();//blank between project elements
			}
			return builder.ToString();
		}
#endif

		//Get the data of the ProjectElement (i.e. session or person) as a CSV
		public string GetFileString(IEnumerable<ProjectElement> elements)
		{
			return GetFileString(
				elements.Select(element => element.ExportFields)
					.Cast<IEnumerable<FieldInstance>>().ToList());
		}

		public string GetFileString(IEnumerable<IEnumerable<FieldInstance>> setsOfFields)
		{
			var builder = new StringBuilder();
			IEnumerable<string> keys = GetKeys(setsOfFields);

			builder.AppendLine(GetHeader(keys));
			foreach (var fields in setsOfFields)
			{
				builder.AppendLine(GetValueLine(keys, fields));
			}
			return builder.ToString();
		}

		/* This is really embarrasing. Someone with linq skills should rewrite it */
		private IEnumerable<string> GetKeys(IEnumerable<IEnumerable<FieldInstance>> setsOfFields)
		{
			var lists = from e in setsOfFields
						select e.Select(z=> z.FieldId);

			var allKeys = new List<string>();
			foreach (var list in lists)
			{
				allKeys.AddRange(list);
			}
			return allKeys.Distinct();
		}

		private string GetHeader(IEnumerable<string> keys)
		{
			var builder = new StringBuilder();

			foreach (string key in keys)
			{
				builder.Append(MassageIfNeeded(key) + _delimeter);
			}
			return builder.ToString().TrimEnd(_delimeter);
		}

		private string GetValueLine(IEnumerable<string> keys, IEnumerable<FieldInstance> fields)
		{
			var builder = new StringBuilder();

			foreach(string key in keys)
			{
				var f= fields.FirstOrDefault(x=> x.FieldId == key);
				if (f == null)
				{
					builder.Append(string.Empty + _delimeter);
				}
				else
				{
					builder.Append(MassageIfNeeded(f.ValueAsString) + _delimeter);
				}
			}
			return builder.ToString().TrimEnd(_delimeter);
		}

		private string MassageIfNeeded(string value)
		{
			var x = value;
			if(x.Contains(_delimeter))
			{
				x= '"' + x + '"';
			}
			x = x.Replace("\r\n", " ");
			x = x.Replace('\n', ' ');  //could have been entered on linux, but now we're outputing on windows
			return x;
		}

	}
}
