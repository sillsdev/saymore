using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SayMore.Model.Files;
using SayMore.Properties;
using SayMore.UI.Utilities;

namespace SayMore.UI.ElementListScreen
{
	/// ------------------------------------------------------------------------------------
	/// <summary>
	/// Makes images representing the workflow stages that have been accomplished. Colors
	/// representing the stages not found in the specified stage are absent (i.e. filled
	/// with some white-ish color).
	/// </summary>
	/// ------------------------------------------------------------------------------------
	public class StagesImageMaker
	{
		private readonly Dictionary<Color, Image> s_clrBlockCache = new Dictionary<Color, Image>();
		private readonly Dictionary<long, Image> s_stagesImageCache = new Dictionary<long, Image>();

		private readonly IEnumerable<ComponentRole> _componentRoles;

		/// ------------------------------------------------------------------------------------
		public StagesImageMaker(IEnumerable<ComponentRole> componentRoles)
		{
			_componentRoles = componentRoles;
		}

		/// ------------------------------------------------------------------------------------
		public Image CreateImageForComponentStage(IEnumerable<ComponentRole> completedRoles)
		{
			var completedRolesKey = GetCompletedRolesKey(completedRoles);
			Image img;
			if (s_stagesImageCache.TryGetValue(completedRolesKey, out img))
				return img;

			var sz = Resources.ComponentStageColorBlockTemplate.Size;

			var bmp = new Bitmap((sz.Width - 1) * _componentRoles.Count() + 1, sz.Height);

			// Now create a single image by combining the blocks for each stage.
			using (var g = Graphics.FromImage(bmp))
			{
				int dx = 0;

				foreach (var role in _componentRoles)
				{
					g.DrawImageUnscaled(GetComponentStageColorBlock(role, completedRoles), dx, 0);
					dx += (sz.Width - 1);
				}
			}

			s_stagesImageCache[completedRolesKey] = bmp;
			return bmp;
		}

		/// ------------------------------------------------------------------------------------
		private static long GetCompletedRolesKey(IEnumerable<ComponentRole> completedRoles)
		{
			long key = 0;
			foreach (var role in completedRoles)
				key += role.Id.GetHashCode();

			return key;
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

			img = AppColors.ReplaceColor(Resources.ComponentStageColorBlockTemplate,
				Color.FromArgb(0xFF, Color.White), clr);

			s_clrBlockCache[clr] = img;
			return img;
		}
	}
}
