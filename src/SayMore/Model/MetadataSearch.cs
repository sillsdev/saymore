using SayMore.Model.Files;
using SIL.Core.ClearShare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// The class that contains the data searching methods.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class MetadataSearch
	{
		
		private readonly ProjectContext _projectContext;

		public MetadataSearch(ProjectContext projectContext)
		{
			_projectContext = projectContext;
		}

		// Searchable tags based on file type.
		private static readonly HashSet<string> sessionFileSearchableTags = new HashSet<string>
		{
			"genre",
			"title",
			"setting",
			"participants",
			"situation",
			"synopsis",
			"location",
			"access",
			"notes",
			"continent",
			"country",
			"region",
			"address",
			"sub-genre",
			"name",
			"contributors",
			"additional_location_country",
			"additional_location_continent",
			"additional_location_region",
			"additional_location_address",
			"additional_sub-genre",
			"additional_interactivity",
			"additional_planning_type",
			"additional_involvement",
			"additional_social_context",
			"additional_task"
		};

		// We know this is not how you search for annotation files.
		private static readonly HashSet<string> annotationFileSearchableTags = new HashSet<string>
		{
			"annotation_value"
		};

		private static readonly HashSet<string> otherFileSearchableTags = new HashSet<string>
		{
			"notes",
			"name",
			"microphone",
			"device",
			"participants",
			"annotation_value",
			"recordist",
			"speaker"
		};

		public IEnumerable<string> SearchSessions(string query)
		{
			var allSessions = _projectContext.Project.GetAllSessions(CancellationToken.None);

			foreach (var session in allSessions)
			{
				bool found = false;
				foreach (var componentFile in session.GetComponentFiles())
				{
					// Determins what file type is being searched for to determine searchable tags.
					HashSet<string> searchableTags;
					if (componentFile.FileType is SessionFileType)
					{
						searchableTags = new HashSet<string>(sessionFileSearchableTags.Union(GetCustomFieldIds(componentFile)));
					}
					else if (componentFile is AnnotationComponentFile)
					{
						searchableTags = annotationFileSearchableTags;
					}
					else if (componentFile is OralAnnotationComponentFile)
					{
						searchableTags = annotationFileSearchableTags;
					}
					else
					{
						searchableTags = new HashSet<string>(otherFileSearchableTags.Union(GetCustomFieldIds(componentFile)));
					}

					// Actually peforms the search and yields the session ID if found.
					var fields = componentFile.MetaDataFieldValues;

					foreach (var field in fields)
					{
						if (searchableTags.Contains(field.FieldId?.ToLowerInvariant()) && 
							(field.Value?.ToString() ?? string.Empty).IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
						{
							found = true;
							yield return session.Id;
							break;
						}
					}

					// If the query wasn't found in the offical fields or the custom fields, check the contributer notes.
					if (!found && componentFile.FileType is SessionFileType && ContainsContributorNotes(componentFile, query))
					{
						found = true;
						yield return session.Id;
					}
				}
				if (found) break;
			}
		}


		// Returns the custom fields for a given component file.
		// Note: this does not work for audio files.
		private HashSet<string> GetCustomFieldIds(ComponentFile file)
		{
			HashSet<string> customFields = new HashSet<string>();

			if (file is ProjectElementComponentFile projectElementFile)
			{
				customFields = new HashSet<string>(
					projectElementFile.GetCustomFields()
						.Select(f => f.FieldId?.ToLowerInvariant())
						.Where(id => !string.IsNullOrEmpty(id))
				);
			}
			
			return customFields;
		}

		// Checks through contributer notes of a given component file for a query.
		private static bool ContainsContributorNotes(ComponentFile file, string query)
		{
			var contributionsField = file.MetaDataFieldValues
				.FirstOrDefault(f => f.FieldId == SessionFileType.kContributionsFieldName);

			if (contributionsField?.Value is ContributionCollection contributions)
			{
				foreach (var contribution in contributions)
				{
					if (!string.IsNullOrEmpty(contribution.Comments) &&
						contribution.Comments.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
