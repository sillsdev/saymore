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
			return twoLetterCode.GetIso639ThreeCharCode();
		}

		[TestCase("eng")]
		[TestCase("spa")]
		[TestCase("deu")]
		[TestCase("fra")]
		[TestCase("und")]
		[TestCase("___")]
		public void GetIso639ThreeCharCode_ThreeLetterCode_ReturnsInput(string threeLetterCode)
		{
			Assert.That(threeLetterCode.GetIso639ThreeCharCode(), 
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
			Assert.That(language.GetIso639ThreeCharCode(), 
				Is.EqualTo(language));
		}

		[TestCase("es-ES", ExpectedResult = "spa")]
		[TestCase("en-GB", ExpectedResult = "eng")]
		[TestCase("am-Latn", ExpectedResult = "amh" )]
		[TestCase("am-Ethi", ExpectedResult = "amh")]
		[TestCase("egy-Latn", ExpectedResult = "egy")]
		public string GetIso639ThreeCharCode_BCP47CodeWithAdditionalSubtags_ReturnsIso639ThreeLetterCode(string language)
		{
			return language.GetIso639ThreeCharCode();
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
		[TestCase("esp:Spanish")] // Wrong-ish, but we'll treat as unambiguous
		[TestCase("es:")] // Weird, but possible
		[TestCase("e:s")] // Weirder, but possible
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
		[TestCase("eng")] // ZVietnam flag is irrelevant for this case
		public void IsAmbiguous_SpecialCaseOfENInVietnam_ReturnsTrue(string language)
		{
			Assert.That(LanguageHelper.IsAmbiguous(language, true), Is.True);
		}

		[TestCase("iii")]
		[TestCase("mas")]
		public void IsAmbiguous_UnambiguousCodeEvenInVietnam_ReturnsFalse(string language)
		{
			Assert.That(LanguageHelper.IsAmbiguous(language, true), Is.False);
		}

		/// <summary>
		/// A well-formed two-part language specification always includes an (unambiguous) BCP-47
		/// tag and a language name, delimited with a colon.
		/// </summary>
		/// <param name="language"></param>
		[TestCase("es")]
		[TestCase("en")] 
		[TestCase("eng")] 
		[TestCase("eng:")] 
		[TestCase("English")] 
		[TestCase(":English")] 
		[TestCase("en:English:American")] 
		[TestCase("Guacamolian")] 
		[TestCase("Acholi (ach)")] 
		[TestCase("Nomu (ISO-noh)")] 
		public void IsWellFormedTwoPartLanguageSpecification_NotTwoParts_ReturnsFalse(string language)
		{
			Assert.That(LanguageHelper.IsWellFormedTwoPartLanguageSpecification(language),
				Is.False);
		}

		/// <summary>
		/// Tests cases where that the portion before the colon is not a valid BCP-47 tag.
		/// </summary>
		/// <param name="language">tag and a language name, delimited with a colon</param>
		[TestCase("esp:Español")]
		[TestCase("eng:English")] 
		[TestCase("ENG:Greek Sign Language")] 
		[TestCase("en-american-latn:English")] 
		[TestCase("guac:Guacamolian")]
		[TestCase(":Nothingish")]
		[TestCase("a:Shortish")]
		public void IsWellFormedTwoPartLanguageSpecification_TwoPartsWithInvalidCode_ReturnsFalse(string language)
		{
			Assert.That(LanguageHelper.IsWellFormedTwoPartLanguageSpecification(language),
				Is.False);
		}

		/// <summary>
		/// Tests cases where that the portion before the colon is a valid BCP-47 tag.
		/// </summary>
		/// <param name="language">tag and a language name, delimited with a colon</param>
		[TestCase("es:Español")]
		[TestCase("en:English")] 
		[TestCase("gss:Greek Sign Language")]
		[TestCase("en-GB:English")]
		[TestCase("zh-CN:Chinese (simplified)")]
		[TestCase("noh:Nomu")] 
		[TestCase("ach:Acholi")] 
		[TestCase("qaa:Some language no one knows about")] 
		[TestCase("qab:Some language no one knows about")] 
		[TestCase("qtz:Some language no one knows about")] 
		public void IsWellFormedTwoPartLanguageSpecification_TwoPartsWithValidCode_ReturnsTrue(string language)
		{
			Assert.That(LanguageHelper.IsWellFormedTwoPartLanguageSpecification(language),
				Is.True);
		}
	}
}
