using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using SIL.Code;
using SIL.Windows.Forms.FileSystem;
using SayMore.Model.Files;
using SayMore.UI.ProjectWindow;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	public class ElementIdChangedArgs : EventArgs
	{
		public ProjectElement Element { get; private set; }
		public string OldId { get; private set; }
		public string NewId { get; private set; }

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
	/// This is reposible for finding, creating, and removing items of the given type T
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

		public List<XmlException> FileLoadErrors { get; private set; }

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the list of items by looking in the file system for all the subfolders
		/// in the project's folder corresponding to this repository.
		/// </summary>
		/// <remarks>Any file load errors ocurring during this call will be noted in
		/// FileLoadErrors. Caller is responsible for checking this and handling them as
		/// appropriate.</remarks>
		/// ------------------------------------------------------------------------------------
		public void RefreshItemList()
		{
			FileLoadErrors.Clear();

			var folders = new HashSet<string>(Directory.GetDirectories(_rootFolder));

			// Go through the existing sessions we have and remove
			// any that no longer have a sessions folder.
			for (int i = _items.Count() - 1; i >= 0; i--)
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
		public virtual IEnumerable<T> AllItems
		{
			get { return _items; }
		}

		/// ------------------------------------------------------------------------------------
		public virtual string PathToFolder
		{
			get { return _rootFolder; }
		}

		/// ------------------------------------------------------------------------------------
		public FileType ElementFileType { get; private set; }

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
