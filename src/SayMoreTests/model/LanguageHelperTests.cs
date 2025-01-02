using NUnit.Framework;
using SayMore.Model;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class LanguageHelperTests
	{
		[TestCase("en", ExpectedResult = "eng")]
		[TestCase("es", ExpectedResult = "spa")]
		[TestCase("de", ExpectedResult = "deu")]
		[TestCase("fr", ExpectedResult = "fra")]
		public string GetIso639ThreeCharCode_TwoLetterCode_GetsThreeLetterCode(string twoLetterCode)
		{
			return LanguageHelper.GetIso639ThreeCharCode(twoLetterCode);
		}

		[TestCase("eng")]
		[TestCase("spa")]
		[TestCase("deu")]
		[TestCase("fra")]
		[TestCase("und")]
		[TestCase("___")]
		public void GetIso639ThreeCharCode_ThreeLetterCode_ReturnsInput(string threeLetterCode)
		{
			Assert.That(LanguageHelper.GetIso639ThreeCharCode(threeLetterCode), 
				Is.EqualTo(threeLetterCode));
		}

		[TestCase(null)]
		[TestCase("Spanish")]
		[TestCase("de:Deutsch")]
		[TestCase("deu:Deutsch")]
		[TestCase("Some unknown language")]
		[TestCase("____")]
		public void GetIso639ThreeCharCode_NullOrLongerName_ReturnsInput(string language)
		{
			Assert.That(LanguageHelper.GetIso639ThreeCharCode(language), 
				Is.EqualTo(language));
		}

		[TestCase("es-ES", ExpectedResult = "spa")]
		[TestCase("en-GB", ExpectedResult = "eng")]
		[TestCase("am-Latn", ExpectedResult = "amh" )]
		[TestCase("am-Ethi", ExpectedResult = "amh")]
		[TestCase("egy-Latn", ExpectedResult = "egy")]
		public string GetIso639ThreeCharCode_BCP47CodeWithAdditionalSubtags_ReturnsInput(string language)
		{
			return LanguageHelper.GetIso639ThreeCharCode(language);
		}

		[TestCase("eng")]
		[TestCase("ENG")]
		[TestCase("aka")]
		[TestCase("AKA")]
		public void IsAmbiguous_Ambiguous_ReturnsTrue(string language)
		{
			Assert.That(LanguageHelper.IsAmbiguous(language), Is.True);
		}

		[TestCase("English")]
		[TestCase("en")] // See test below which deals with special case of Vietnam
		[TestCase("En")] // Endangered language of Vietnam
		[TestCase("es:Spanish")]
		[TestCase("iii")]
		[TestCase("Walloon")]
		[TestCase("mas")]
		[TestCase("MAS")]
		[TestCase("Mas")]
		[TestCase("Aka")]
		public void IsAmbiguous_NotAmbiguous_ReturnsFalse(string language)
		{
			Assert.That(LanguageHelper.IsAmbiguous(language), Is.False);
		}

		/// <summary>
		/// En is an endangered indigenous language of Vietnam. If we know we're dealing with a
		/// project or session in Vietnam, we consider EN or en as ambiguous.
		/// </summary>
		/// <param name="language"></param>
		[TestCase("EN")]
		[TestCase("en")] 
		public void IsAmbiguous_SpecialCaseOfENInVietnam_ReturnsFalse(string language)
		{
			Assert.That(LanguageHelper.IsAmbiguous(language, true), Is.True);
		}
	}
}
