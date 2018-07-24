using UnityEngine;
using UnityEditor;

namespace x600d1dea.stubs
{

	public class GameToolsConfig : ScriptableObject
	{
		public DefaultAsset gameDataAsset;
		public bool encryptGameData;
		public string pathExportGameData;
		public string pathPiker;
		public int cryptoKey = 0x600d1dea;
	}

}
