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
		private readonly IEnumerable<ComponentRole> _componentRoles;

		/// ------------------------------------------------------------------------------------
		public StagesImageMaker(IEnumerable<ComponentRole> componentRoles)
		{
			_componentRoles = componentRoles;
		}

		/// ------------------------------------------------------------------------------------
		public Image CreateImageForComponentStage(IEnumerable<ComponentRole> completedRoles)
		{
			var sz = Resources.ComponentStageColorBlockTemplate.Size;

			// Subtract 1 from the number of stages so the value 'None' is not included.
			var bmp = new Bitmap((sz.Width - 1) * (_componentRoles.Count() - 1) + 1, sz.Height);

			// Now create a single image by combining the blocks for each stage
			// that is not the 'None' stage.
			using (var g = Graphics.FromImage(bmp))
			{
				int dx = 0;

				foreach (var role in _componentRoles)
				{
					g.DrawImageUnscaled(GetComponentStageColorBlock(role, completedRoles), dx, 0);
					dx += (sz.Width - 1);
				}
			}

			return bmp;
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
			var color = (role.IsContainedIn(completedRoles) ?
				role.Color : Settings.Default.IncompleteStageColor);

			return AppColors.ReplaceColor(Resources.ComponentStageColorBlockTemplate,
				Color.FromArgb(0xFF, Color.White), color);
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Gets a uniquely colored block for the specified role.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Image GetComponentStageColorBlock(ComponentRole role)
		{
			return AppColors.ReplaceColor(Resources.ComponentStageColorBlockTemplate,
				Color.FromArgb(0xFF, Color.White), role.Color);
		}
	}
}
