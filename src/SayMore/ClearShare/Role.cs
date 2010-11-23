namespace SayMore.ClearShare
{
	public class Role
	{
		public string Code { get; private set; }
		public string Name { get; private set; }
		public string Definition { get; private set; }

		public Role(string code, string name, string definition)
		{
			Code = code;
			Name = name;
			Definition = definition;
		}
	}
}