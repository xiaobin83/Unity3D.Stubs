using System.Collections;
using UnityEngine;
using x600d1dea.pike;


namespace x600d1dea.stubs.utils
{
	public class ResMgr
	{
		public class ResourcesWrapper
		{
			public virtual byte[] LoadBytes(string path)
			{
				var asset = Resources.Load<TextAsset>(path);
				if (asset != null)
				{
					return asset.bytes;
				}
				return null;
			}

			public virtual string LoadText(string path)
			{
				var asset = Resources.Load<TextAsset>(path);
				if (asset != null)
				{
					return asset.text;
				}
				return null;
			}

			public virtual byte[] LoadEncryptedBytes(string path)
			{
				var bytes = LoadBytes(path);
				if (bytes != null)
				{
					if (pike != null)
					{
						pike.Codec(ref bytes);
						return bytes;
					}
					Debug.LogError("load encrypted data without crypto key");
					return null;
				}
				return null;
			}

			public virtual string LoadCustomText(string tag, string path)
			{
				throw new System.NotImplementedException();
			}

			public virtual byte[] LoadCustomBytes(string tag, string path)
			{
				throw new System.NotImplementedException();
			}

			public virtual string LoadEncryptedText(string path)
			{
				var bytes = LoadEncryptedBytes(path);
				if (bytes != null)
				{
					return System.Text.Encoding.UTF8.GetString(bytes);
				}
				return null;
			}

			public virtual T Load<T>(string path) where T: Object
			{
				return Resources.Load<T>(path);
			}

			public virtual T[] LoadAll<T>(string path) where T: Object
			{
				return Resources.LoadAll<T>(path);
			}

			public virtual ResourceRequest LoadAsync<T>(string path) where T: Object
			{
				return Resources.LoadAsync(path, typeof(T));
			}

			public virtual Object Load(string path, System.Type type)
			{
				return Resources.Load(path, type);
			}

			public virtual Object Load(string path)
			{
				return Resources.Load(path);
			}

			public virtual Object[] LoadAll(string path)
			{
				return Resources.LoadAll(path);
			}		

			public virtual Object[] LoadAll(string path, System.Type type)
			{
				return Resources.LoadAll(path, type);
			}

			public virtual ResourceRequest LoadAsync(string path)
			{
				return Resources.LoadAsync(path);
			}

			public virtual void LoadCustom(string tag, string path)
			{
				throw new System.NotImplementedException();
			}

			public virtual Object LoadCustomObject(string tag, string path)
			{
				throw new System.NotImplementedException();
			}

			public virtual Object[] LoadAllCustomObjects(string tag, string path)
			{
				throw new System.NotImplementedException();
			}

			public virtual ResourceRequest LoadAsync(string path, System.Type type)
			{
				return Resources.LoadAsync(path, type);
			}

			public virtual void UnloadUnusedAssets()
			{
				Resources.UnloadUnusedAssets();
			}
		}

		static ResourcesWrapper rw = new ResourcesWrapper();

		public static void SetResourcesWrapper(ResourcesWrapper rw)
		{
			ResMgr.rw = rw;
		}


		public static byte[] LoadBytes(string path, bool encrypted=false)
		{
			if (encrypted)
			{
				return rw.LoadEncryptedBytes(path);
			}
			return rw.LoadBytes(path);
		}

		public static string LoadText(string path, bool encrypted=false)
		{
			if (encrypted)
			{
				return rw.LoadEncryptedText(path);
			}
			return rw.LoadText(path);
		}

		public static string LoadCustomText(string tag, string path)
		{
			return rw.LoadCustomText(tag, path);
		}

		public static byte[] LoadCustomBytes(string tag, string path)
		{
			return rw.LoadCustomBytes(tag, path);
		}

		public static Object Load(string uri)
		{
			return rw.Load(uri);
		}

		public static Object Load(string uri, System.Type type)
		{
			return rw.Load(uri, type);
		}

		public static T Load<T>(string uri) where T: Object
		{
			return rw.Load<T>(uri);
		}


		public static Object[] LoadAll(string uri)
		{
			return rw.LoadAll(uri);
		}

		public static T[] LoadAll<T>(string uri) where T: Object
		{
			return rw.LoadAll<T>(uri);
		}

		public static Object[] LoadAll(string uri, System.Type type)
		{
			return rw.LoadAll(uri, type);
		}

		public static void LoadCustom(string tag, string path)
		{
			rw.LoadCustom(tag, path);
		}

		public static Object LoadCustomObject(string tag, string path)
		{
			return rw.LoadCustomObject(tag, path);
		}

		public static Object[] LoadAllCustomObjects(string tag, string path)
		{
			return rw.LoadAllCustomObjects(tag, path);
		}


		static IEnumerator AsyncLoader(ResourceRequest r, System.Action<int, Object> progress)
		{
			while (!r.isDone)
			{
				int prog = (int)((r.progress / 100) * 99);
				progress(prog, null);
				yield return null;
			}
			progress(100, r.asset);
		}

		public static void LoadAsync(string uri, System.Action<int, Object> progress, MonoBehaviour workerBehaviour = null)
		{
			var r =  rw.LoadAsync(uri);
			TaskManager.StartCoroutine(AsyncLoader(r, progress), workerBehaviour);
		}
		

		public static void UnloadUnused()
		{
			rw.UnloadUnusedAssets();
		}

		static Pike pike;
		public static void SetCryptoKey(uint key)
		{
			pike = new Pike(key);
		}

	}
}

