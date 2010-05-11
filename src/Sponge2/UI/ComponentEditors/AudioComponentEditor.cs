using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sponge2.UI.ComponentEditors
{
	public partial class AudioComponentEditor : EditorBase
	{
		public AudioComponentEditor()
		{
			InitializeComponent();

			_tableLayout.BackColor = SystemColors.Window;
		}
	}
}
