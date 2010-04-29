using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Palaso.Code;

namespace Sponge2.Model
{
	/// <summary>
	/// This is reposible for finding, creating, and removing items of the given type T
	/// (i.e. Sessions or People)
	/// </summary>
	public class ElementRepository<T> where T: ProjectElement
	{
		public delegate ElementRepository<T> Factory(string projectDirectory, string elementGroupName);

		private readonly Func<string, T> _elementFactory;
		private List<T> _items = new List<T>();
		private string _rootFolder;

		public ElementRepository(string projectDirectory, string elementGroupName, Func<string,T> elementFactory)
		{
			_elementFactory = elementFactory;
			RequireThat.Directory(projectDirectory).Exists();

			_rootFolder = Path.Combine(projectDirectory, elementGroupName);
			if(!Directory.Exists(_rootFolder))
			{
				Directory.CreateDirectory(_rootFolder);
			}
			RefreshItemList();
		}


		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the list of items by looking in the file system for all the subfolders
		/// in the project's folder corresponding to this repository.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public void RefreshItemList()
		{
			var folders = new HashSet<string>(Directory.GetDirectories(_rootFolder));

			// Go through the existing sessions we have and remove
			// any that no longer have a sessions folder.
			for (int i = _items.Count() - 1; i >= 0; i--)
			{
				if (!folders.Contains(_items[i].FolderPath))
					_items.RemoveAt(i);
			}

			// Add any items we don't already have
			foreach (string path in folders)
			{
				var item = _items.FirstOrDefault(x => x.FolderPath == path);
				if (item == null)
				{
					var element =_elementFactory(Path.GetFileName(path));
					_items.Add(element);
				}
			}

		}

		public IEnumerable<T> AllItems
		{
			get {return _items;}
		}
	}
}
