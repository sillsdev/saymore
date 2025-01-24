// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2024' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using System.Windows.Forms;
using SIL.Windows.Forms.WritingSystems;

namespace SayMore.UI.LowLevelControls
{
	public partial class LanguageChooser : Form
	{
		public LanguageChooser(WritingSystemSetupModel model)
		{
			InitializeComponent();

			wsPicker.BindToModel(model);
		}
	}
}
