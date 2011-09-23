using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SayMore.Model
{
	/// ------------------------------------------------------------------------------------
	public class ProjectElementComparer : IComparer<ProjectElement>
	{
		private readonly string _fieldId;
		private readonly SortOrder _direction;
		private readonly Func<ProjectElement, string, object> _getValueProvider;

		/// ------------------------------------------------------------------------------------
		public ProjectElementComparer(string fieldId, SortOrder direction,
			Func<ProjectElement, string, object> getValueProvider)
		{
			_fieldId = fieldId;
			_direction = direction;
			_getValueProvider = getValueProvider;
		}

		/// ------------------------------------------------------------------------------------
		public int Compare(ProjectElement x, ProjectElement y)
		{
			if (x == null && y == null)
				return 0;

			if (x == null)
				return -1;

			if (y == null)
				return 1;

			if (_direction == SortOrder.Descending)
			{
				var tmp = y;
				y = x;
				x = tmp;
			}

			var xval = _getValueProvider(x, _fieldId);
			var yval = _getValueProvider(y, _fieldId);

			if (xval is string)
				return string.Compare((string)xval, (string)yval, StringComparison.CurrentCulture);

			if (xval is int)
				return (int)xval - (int)yval;

			if (xval is DateTime)
				return DateTime.Compare((DateTime)xval, (DateTime)yval);

			return 0;
		}
	}
}
