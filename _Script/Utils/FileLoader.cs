using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace x600d1dea.utils
{
	public class FileLoader 
	{
		public static T LoadJson<T>(string path)
		{
			string filePath = null;
#if UNITY_EDITOR
			filePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), path); 
			if (!File.Exists(filePath))
				filePath = null;

#endif
			if (filePath == null)
			{
				filePath = Path.Combine(Application.persistentDataPath, path);
				if (!File.Exists(filePath))
					filePath = null;
			}

			string content = null;
			if (filePath == null)
			{
				var asset = ResMgr.Load<TextAsset>(path);
				if (asset != null)
				{
					content = asset.text;
				}
			}
			else
			{
				content = File.ReadAllText(filePath);
			}
			return JsonConvert.DeserializeObject<T>(content);
		}
	}
}
