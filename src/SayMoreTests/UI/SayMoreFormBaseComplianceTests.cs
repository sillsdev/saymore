using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using NUnit.Framework;
using SayMore.UI;

namespace SayMoreTests.UI
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Guards against newly-added dialogs silently missing out on SayMore's UI text-scaling
	/// support (needed, for localizations where the default font size can be too small to read).
	/// Every Form in the SayMore assembly is expected to derive from 
	/// <see cref="SayMoreFormBase"/>, which applies the user's text scale automatically,
	/// unless it has a documented reason to be excluded below.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	[TestFixture]
	public class SayMoreFormBaseComplianceTests
	{
		/// <summary>
		/// Forms intentionally excluded from the SayMoreFormBase requirement, each with a reason.
		/// </summary>
		private static readonly Type[] s_excludedForms =
		{
			// Shown before the project/settings context is ready, and its layout is driven by
			// hard-coded pixel positions in a custom OnPaint override rather than normal
			// WinForms layout, so scaling its labels' fonts without also reworking that custom
			// painting would misalign or clip text against the painted logo/graphics.
			typeof(SayMore.UI.SplashScreenForm),

			// Owns the UITextScale setting itself and already implements its own scale-on-load
			// and live-preview-while-adjusting logic. See the comment on the class.
			typeof(SayMore.UI.ProjectWindow.UILanguageDlg),
		};

		/// ------------------------------------------------------------------------------------
		[Test]
		public void AllFormsInSayMoreAssembly_DeriveFromSayMoreFormBaseOrAreExplicitlyExcluded()
		{
			var assembly = Assembly.GetAssembly(typeof(SayMoreFormBase));

			var nonCompliantForms = assembly.GetTypes()
				.Where(t => t.IsClass && !t.IsAbstract && typeof(Form).IsAssignableFrom(t))
				.Where(t => t != typeof(SayMoreFormBase))
				.Where(t => !typeof(SayMoreFormBase).IsAssignableFrom(t))
				.Where(t => !s_excludedForms.Contains(t))
				.Select(t => t.FullName)
				.OrderBy(name => name)
				.ToList();

			Assert.That(nonCompliantForms, Is.Empty,
				"The following Forms do not derive from SayMoreFormBase. Either migrate them " +
				"or add them to s_excludedForms with a documented reason:\n" +
				string.Join("\n", nonCompliantForms));
		}
	}
}
