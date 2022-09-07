using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SIL.Code;
using SIL.Windows.Forms.FileSystem;
using SIL.Reporting;
using L10NSharp;
using FileType = SayMore.Model.Files.FileType;
using System.Threading;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	public class ElementIdChangedArgs : EventArgs
	{
		public ProjectElement Element { get; }
		public string OldId { get; }
		public string NewId { get; }

		/// ------------------------------------------------------------------------------------
		public ElementIdChangedArgs(ProjectElement element, string oldId, string newId)
		{
			Element = element;
			OldId = oldId;
			NewId = newId;
		}
	}

	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// This is responsible for finding, creating, and removing items of the given type T
	/// (i.e. Sessions or People)
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class ElementRepository<T> where T : ProjectElement
	{
		public event EventHandler<ElementIdChangedArgs> ElementIdChanged;

		public delegate ElementRepository<T> Factory(string projectDirectory, string elementGroupName, FileType type);

		public delegate T ElementFactory(string parentElementFolder, string id,
			Action<ProjectElement, string, string> idChangedNotificationReceiver);

		//private readonly  Func<string, string, T> _elementFactory;
		private readonly ElementFactory _elementFactory; //TODO: fix this. I'm struggling with autofac on this issue
		private readonly List<T> _items = new List<T>();
		private readonly string _rootFolder;

		/// ------------------------------------------------------------------------------------
		public ElementRepository(string projectDirectory, string elementGroupName,
			FileType type, ElementFactory elementFactory)
		{
			ElementFileType = type;
			_elementFactory = elementFactory;
			RequireThat.Directory(projectDirectory).Exists();

			_rootFolder = Path.Combine(projectDirectory, elementGroupName);

			if (!Directory.Exists(_rootFolder))
				Directory.CreateDirectory(_rootFolder);

			FileLoadErrors = new List<XmlException>();

			RefreshItemList();
		}

		[Obsolete("For Mocking Only")]
		public ElementRepository(){}

		public List<XmlException> FileLoadErrors { get; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the list of items by looking in the file system for all the subfolders
		/// in the project's folder corresponding to this repository.
		/// </summary>
		/// <remarks>Any file load errors occurring during this call will be noted in
		/// FileLoadErrors. Caller is responsible for checking this and handling them as
		/// appropriate.</remarks>
		/// ------------------------------------------------------------------------------------
		public void RefreshItemList()
		{
			FileLoadErrors.Clear();

			// SP-2273...
			string[] sessionDirectories = null;
			var retry = false;
			do
			{
				try
				{
					sessionDirectories = Directory.GetDirectories(_rootFolder);
				}
				catch (Exception e)
				{
					if (retry)
						throw;

					if (e is DirectoryNotFoundException)
					{
						Thread.Sleep(200); // Give it a fighting chance in case it was something transient.
						if (Directory.Exists(_rootFolder))
						{
							retry = true;
							continue;
						}
					}
					if (e is IOException || e is UnauthorizedAccessException)
					{
						ErrorReport.ReportNonFatalExceptionWithMessage(e,
							string.Format(LocalizationManager.GetString(
								"MainWindow.ElementFolderMissingOrUnavailable",
								"It looks like the {0} folder is no longer accessible. If you " +
								"are able to fix this, {1} will retry. Otherwise, please report " +
								"this ({2}).", 
								"Param 0: element type (\"Session\" or \"Person\"); "+
								"Param 1: \"SayMore\" (program name); "+
								"Param 2: Jira issue number (for developers)"),
								ElementFileType.Name, Program.ProductName, "SP-2273"));
						if (!Directory.Exists(_rootFolder))
							throw;
						retry = true;
					}
					else
						throw;
				}
			} while (retry);
			// ...SP-2273

			var folders = new HashSet<string>(sessionDirectories);

			// Go through the existing sessions we have and remove
			// any that no longer have a sessions folder.
			for (int i = _items.Count - 1; i >= 0; i--)
			{
				if (!folders.Contains(_items[i].FolderPath))
					_items.RemoveAt(i);
			}

			// Add any items we don't already have
			foreach (var path in folders)
			{
				if (!_items.Any(x => x.FolderPath == path))
				{
					var elementPath = Path.GetDirectoryName(path);
					var elementId = path.Substring(elementPath.Length + 1);
					try
					{
						var element = _elementFactory(elementPath, elementId, OnElementIdChanged);
						_items.Add(element);
					}
					catch(Exception exception)
					{
						var xmlException = exception.InnerException as XmlException;
						if (xmlException == null)
							throw;
						FileLoadErrors.Add(xmlException);
					}
				}
			}
		}

		/// ------------------------------------------------------------------------------------
		public virtual IEnumerable<T> AllItems => _items;

		/// ------------------------------------------------------------------------------------
		public virtual string PathToFolder => _rootFolder;

		/// ------------------------------------------------------------------------------------
		public FileType ElementFileType { get; }

		/// ------------------------------------------------------------------------------------
		public T CreateNew(string id)
		{
			T element = _elementFactory(_rootFolder, id, OnElementIdChanged);
			_items.Add(element);
			return element;
		}

		/// ------------------------------------------------------------------------------------
		public bool Remove(string id)
		{
			var item = _items.FirstOrDefault(x => x.Id == id);
			return (item != null && Remove(item));
		}

		/// ------------------------------------------------------------------------------------
		public bool Remove(T item)
		{
			if (!_items.Contains(item) || !RemoveItemFromFileSystem(item))
				return false;

			_items.Remove(item);
			item.Dispose();

			return true;
		}

		/// ------------------------------------------------------------------------------------
		protected bool RemoveItemFromFileSystem(T item)
		{
			if (item.FolderPath == "*mocked*")
				return true;

			// Recycle only, since the user has already confirmed the delete by this time.
			return ConfirmRecycleDialog.Recycle(item.FolderPath);
		}

		/// ------------------------------------------------------------------------------------
		public virtual T GetById(string id)
		{
			return _items.FirstOrDefault(x => x.Id == id);
		}

		/// ------------------------------------------------------------------------------------
		public virtual T GetByField(string fieldName, string fieldValue)
		{
			return _items.FirstOrDefault(x => x.MetaDataFile.GetStringValue(fieldName, null) == fieldValue);
		}

		/// ------------------------------------------------------------------------------------
		protected virtual void OnElementIdChanged(ProjectElement element, string oldId, string newId)
		{
			if (ElementIdChanged != null)
				ElementIdChanged(this, new ElementIdChangedArgs(element, oldId, newId));
		}
	}
}
