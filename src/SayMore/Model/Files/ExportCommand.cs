using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SayMore.Model.Fields;

namespace SayMore.Model.Files
{
	/// <summary>
	/// Exports metadata to file (currently, only csv)
	/// </summary>
	public class ExportCommand :Command
	{
		private readonly IEnumerable<ProjectElement> _elements;
		private char _delimeter=',';

		public ExportCommand(ElementRepository<Session> sessions)
			: base("export")
		{
			_elements = sessions.AllItems.ToArray();
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
				dlg.Title = "Export Session Data";
				dlg.AddExtension = true;
				dlg.AutoUpgradeEnabled = true;
				dlg.Filter = "CSV (Comma delimited) (*.csv)|*.csv";
				if(DialogResult.OK!= dlg.ShowDialog())
				{
					return;
				}
				DoExport(elements, dlg.FileName);
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
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, "Something went wrong with the export.");
			}
		}

		public string GetFileString(IEnumerable<ProjectElement> elements)
		{
			return GetFileString(
				elements.Select(element => element.ExportFields)
					.Cast<IEnumerable<FieldValue>>().ToList());
		}

		public string GetFileString(IEnumerable<IEnumerable<FieldValue>> setsOfFields)
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
		private IEnumerable<string> GetKeys(IEnumerable<IEnumerable<FieldValue>> setsOfFields)
		{
			var lists = from e in setsOfFields
						select e.Select(z=> z.FieldDefinitionKey);

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
				builder.Append(EscapeIfNeeded(key) + _delimeter);
			}
			return builder.ToString().TrimEnd(_delimeter);
		}

		private string GetValueLine(IEnumerable<string> keys, IEnumerable<FieldValue> fields)
		{
			var builder = new StringBuilder();

			foreach(string key in keys)
			{
				var f= fields.FirstOrDefault(x=> x.FieldDefinitionKey == key);
				if (f == null)
				{
					builder.Append(string.Empty + _delimeter);
				}
				else
				{
					builder.Append(EscapeIfNeeded(f.Value) + _delimeter);
				}
			}
			return builder.ToString().TrimEnd(_delimeter);
		}

		private string EscapeIfNeeded(string value)
		{
			if(value.Contains(_delimeter))
			{
				return '"' + value + '"';
			}
			return value;
		}

	}
}
