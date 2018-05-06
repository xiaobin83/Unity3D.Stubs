using System;

namespace x600d1dea.stubs.networking
{
	public class EditorConnectionMessageID
	{
		public static readonly Guid Editor = new Guid("7DA393DA467C42D4B4F5DBEC3D7D36D9");
		public static readonly Guid Player = new Guid("A8D85BA6C26F458E9B97EED5C87F35F8");
	}

	public class ECMHeader {
		public string id;
	}

	public class ECMessage
	{
		public string id;
		public string content;
	}
	

	public class ECMExecLuaScript
	{
		public string id = "exec_lua";
		public string content;
	}

	public class ECMPutFile
	{
		public string id = "put_file";
		public string path;
		public string content;
	}
}
