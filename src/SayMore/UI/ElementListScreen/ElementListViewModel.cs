using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using L10NSharp;
using SayMore.Model;
using SayMore.Model.Files;
using SayMore.UI.ComponentEditors;

namespace SayMore.UI.ElementListScreen
{
	/// <summary>
	/// This is the logic behind the screen which shows the list of sessions, and also the
	/// screen which shows the list of persons
	/// </summary>
	public class ElementListViewModel<T> where T: ProjectElement
	{
		private readonly ElementRepository<T> _repository;
		private IEnumerable<IEditorProvider> _currentEditorProviders;

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
			get { return _repository.AllItems; }
		}

		/// ------------------------------------------------------------------------------------
		public IEnumerable<ComponentFile> GetComponentsOfSelectedElement()
		{
			return (SelectedElement == null ? new ComponentFile[] { } : SelectedElement.GetComponentFiles());
		}

		/// ------------------------------------------------------------------------------------
		public bool VerifyAllElementsStillExist()
		{
			var bldr = new StringBuilder();

			foreach (var element in Elements.Where(e => !Directory.Exists(e.FolderPath)))
			{
				int i = element.FolderPath.LastIndexOf(Path.DirectorySeparatorChar);
				bldr.AppendLine(element.FolderPath.Substring(i + 1));
			}

			if (bldr.Length == 0)
				return true;

			var msg = LocalizationManager.GetString("MainWindow.RemovedElementsErrorMsg",
				"The folders for the following elements have been removed from your computer.\r\n" +
				"Therefore, SayMore will remove them from the list.\r\n\r\n{0}");

			Palaso.Reporting.ErrorReport.NotifyUserOfProblem(msg, bldr.ToString());
			RefreshElementList();
			return false;
		}

		/// ------------------------------------------------------------------------------------
		public void RefreshElementList()
		{
			_repository.RefreshItemList();
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

			if (index < 0)
				return false;

			var componentFiles = SelectedElement.GetComponentFiles();
			if (index >= componentFiles.Length)
				return false;

			SelectedComponentFile = componentFiles[index];

			_currentEditorProviders = SelectedComponentFile.FileType.GetEditorProviders(
				GetHashCode(), SelectedComponentFile);

			return true;
		}

		/// ------------------------------------------------------------------------------------
		public bool AddComponentFiles(string[] files)
		{
			return SelectedElement.AddComponentFiles(files);
		}

		/// ------------------------------------------------------------------------------------
		public bool DeleteComponentFile(ComponentFile file)
		{
			if (_currentEditorProviders.Any(editor => !editor.ComponentFileDeletionInitiated(file)))
				return false;

			return SelectedElement.DeleteComponentFile(file, true);
		}

		/// ------------------------------------------------------------------------------------
		public void ActivateComponentEditors()
		{
			if (_currentEditorProviders == null)
				return;

			foreach (var editor in _currentEditorProviders)
				editor.Activated();
		}

		/// ------------------------------------------------------------------------------------
		public void DeactivateComponentEditors()
		{
			if (_currentEditorProviders == null)
				return;

			foreach (var editor in _currentEditorProviders)
				editor.Deactivated();
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
		public virtual string PathToSessionsFolder
		{
			get { return _repository.PathToFolder; }
		}

		/// ------------------------------------------------------------------------------------
		public bool Remove(string id)
		{
			var retVal = _repository.Remove(id);
			if (retVal && SelectedElement != null && SelectedElement.Id == id)
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