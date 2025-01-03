// --------------------------------------------------------------------------------------------
#region // Copyright (c) 2025, SIL Global. All Rights Reserved.
// <copyright from='2025' to='2025' company='SIL Global'>
//		Copyright (c) 2025, SIL Global. All Rights Reserved.
//
//		Distributable under the terms of the MIT License (https://sil.mit-license.org/)
// </copyright>
#endregion
// --------------------------------------------------------------------------------------------
using SIL.WritingSystems;

namespace SayMore.Model
{
	public class TwoPartLanguageSpecifier
	{
		string Code { get; }
		string Name { get; }
		public bool IsValid => IetfLanguageTag.IsValid(Code);

		public TwoPartLanguageSpecifier(string code, string name)
		{
			Code = code;
			Name = name;
		}

		public override string ToString() => $"{Code}:{Name}";
	}
}
