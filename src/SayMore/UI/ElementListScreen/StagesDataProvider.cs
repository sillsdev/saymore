using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SayMore.Model.Files;
using SayMore.Properties;

namespace SayMore.UI.ElementListScreen
{
	/// ------------------------------------------------------------------------------------
	public class StagesDataProvider
	{
		private const int kComponentStageColorBlockWidth = 8;
		private const int kComponentStageColorBlockHeight = 12;
		private readonly Dictionary<Color, Image> s_clrBlockCache = new Dictionary<Color, Image>();
		private readonly Dictionary<long, Image> s_stagesImageCache = new Dictionary<long, Image>();

		private readonly IEnumerable<ComponentRole> _componentRoles;
		private readonly IDictionary<string, int> _roleKeys;

		/// ------------------------------------------------------------------------------------
		public StagesDataProvider(IEnumerable<ComponentRole> componentRoles)
		{
			_componentRoles = componentRoles;

			int i = 1;
			_roleKeys = new Dictionary<string, int>();
			foreach (var role in _componentRoles)
			{
				_roleKeys[role.Id] = i;
				i *= 2;
			}
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Makes images representing the workflow stages that have been accomplished. Colors
		/// representing the stages not found in the specified stage are absent (i.e. filled
		/// with some white-ish color).
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image CreateImageForComponentStage(IEnumerable<ComponentRole> completedRoles)
		{
			var completedRolesList = completedRoles.ToArray();
			var completedRolesKey = GetCompletedRolesKey(completedRolesList);

			Image img;
			if (s_stagesImageCache.TryGetValue(completedRolesKey, out img))
				return img;

			var bmp = new Bitmap((kComponentStageColorBlockWidth - 1) * _componentRoles.Count() + 1, kComponentStageColorBlockHeight);

			// Now create a single image by combining the blocks for each stage.
			using (var g = Graphics.FromImage(bmp))
			{
				int dx = 0;

				foreach (var role in _componentRoles)
				{
					g.DrawImageUnscaled(GetComponentStageColorBlock(role, completedRolesList), dx, 0);
					dx += (kComponentStageColorBlockWidth - 1);
				}
			}

			s_stagesImageCache[completedRolesKey] = bmp;
			return bmp;
		}

		/// ------------------------------------------------------------------------------------
		public int GetCompletedRolesKey(IEnumerable<ComponentRole> completedRoles)
		{
			return completedRoles.Aggregate(0, (current, role) => current | _roleKeys[role.Id]);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a uniquely colored block for the specified role if the specified role is
		/// in the list of completed roles. Otherwise a white-ish colored block is returned
		/// indicating the role is imcomplete.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image GetComponentStageColorBlock(ComponentRole role, IEnumerable<ComponentRole> completedRoles)
		{
			var clr = (role.IsContainedIn(completedRoles) ?
				role.Color : Settings.Default.IncompleteStageColor);

			return GetComponentStageColorBlock(clr);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a uniquely colored block for the specified role.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image GetComponentStageColorBlock(ComponentRole role)
		{
			return GetComponentStageColorBlock(role.Color);
		}

		/// ------------------------------------------------------------------------------------
		private Image GetComponentStageColorBlock(Color clr)
		{
			Image img;
			if (s_clrBlockCache.TryGetValue(clr, out img))
				return img;

			Bitmap bmp = new Bitmap(kComponentStageColorBlockWidth, kComponentStageColorBlockHeight);
			Graphics gBmp = Graphics.FromImage(bmp);

			using (var pen = new Pen(Color.FromArgb(133, 133, 108)))
				gBmp.DrawRectangle(pen, new Rectangle(0, 0, kComponentStageColorBlockWidth - 1, kComponentStageColorBlockHeight - 1));

			using (var brush = new SolidBrush(clr))
				gBmp.FillRectangle(brush, new Rectangle(1, 1, kComponentStageColorBlockWidth - 2, kComponentStageColorBlockHeight - 2));

			s_clrBlockCache[clr] = bmp;
			return bmp;
		}
	}
}
