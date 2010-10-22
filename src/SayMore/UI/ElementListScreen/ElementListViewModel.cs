using System.Collections.Generic;
using System.Linq;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.ComponentEditors;

namespace SayMore.UI.ElementListScreen
{
	/// <summary>
	/// This is the logic behind the screen which shows the list of events, and also the
	/// screen swhich shows the list of persons
	/// </summary>
	public class ElementListViewModel<T> where T: ProjectElement
	{
		private readonly ElementRepository<T> _repository;
		private IEnumerable<IEditorProvider> _currentEditorProviders;
		private ComponentFile[] _componentFiles;

		public T SelectedElement { get; private set; }
		public ComponentFile SelectedComponentFile { get; private set; }

		/// ------------------------------------------------------------------------------------
		public ElementListViewModel(ElementRepository<T> repository)
		{
			_repository = repository;
		}

		/// ------------------------------------------------------------------------------------
		public FileType ElementFileType
		{
			get { return _repository.ElementFileType; }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ProjectElement> Elements
		{
			get { return _repository.AllItems.Cast<ProjectElement>(); }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentFile> GetComponentsOfSelectedElement()
		{
			return (SelectedElement == null ? new ComponentFile[] { } : _componentFiles);
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshElementList()
		{
			_repository.RefreshItemList();
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshSelectedElementComponentFileList()
		{
			_componentFiles = (SelectedElement != null ? SelectedElement.GetComponentFiles().ToArray() : null);
		}

		/// ------------------------------------------------------------------------------------
		public int GetIndexOfSelectedElement()
		{
			int index = 0;
			foreach (var element in Elements)
			{
				if (SelectedElement == element)
					return index;

				index++;
			}

			return -1;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetSelectedElement(T element)
		{
			if (SelectedElement == element)
				return false;

			SelectedElement = element;
			RefreshSelectedElementComponentFileList();
			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool SetSelectedComponentFile(int index)
		{
			if (SelectedElement == null)
			{
				SelectedComponentFile = null;
				return false;
			}

			if (index < 0 || index >= _componentFiles.Length)
				return false;

			SelectedComponentFile = _componentFiles[index];

			_currentEditorProviders =
				SelectedComponentFile.FileType.GetEditorProviders(GetHashCode(), SelectedComponentFile);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool AddComponentFiles(string[] files)
		{
			if (SelectedElement.AddComponentFiles(files))
			{
				_componentFiles = SelectedElement.GetComponentFiles().ToArray();
				return true;
			}

			return false;
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshAfterIdChanged()
		{
			_componentFiles = SelectedElement.GetComponentFiles().ToArray();
		}

		/// ------------------------------------------------------------------------------------
		public ComponentFile GetComponentFile(int index)
		{
			return _componentFiles[index];
		}

		/// ------------------------------------------------------------------------------------
		public void MakeComponentEditorsGoDormant()
		{
			if (_currentEditorProviders != null)
			{
				foreach (var editor in _currentEditorProviders)
					editor.GoDormant();
			}
		}

		/// ------------------------------------------------------------------------------------
		public T CreateNewElement()
		{
			return _repository.CreateNew(null);
		}

		/// ------------------------------------------------------------------------------------
		public T CreateNewElementWithId(string id)
		{
			return _repository.CreateNew(id);
		}

		/// ------------------------------------------------------------------------------------
		public string PathToEventsFolder
		{
			get { return _repository.PathToFolder; }
		}

		/// ------------------------------------------------------------------------------------
		public bool Remove(string id)
		{
			var retVal = _repository.Remove(id);
			if (retVal && SelectedElement.Id == id)
				SelectedElement = null;

			return retVal;
		}

		/// ------------------------------------------------------------------------------------
		public bool Remove(T item)
		{
			if (SelectedElement == item)
				SelectedElement = null;

			return _repository.Remove(item);
		}

		/// ------------------------------------------------------------------------------------
		public string GetSelectedProviderKey()
		{
			return (SelectedComponentFile != null ?
				SelectedComponentFile.FileType.GetType().Name : null);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// For each component, we provide 1 or more viewers/editors base on its file type.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public IEnumerable<IEditorProvider> GetComponentEditorProviders()
		{
			return (SelectedComponentFile == null || _currentEditorProviders == null ?
				new IEditorProvider[] { } : _currentEditorProviders);
		}
	}
}