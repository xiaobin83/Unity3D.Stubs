using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace x600d1dea.stubs.utils
{
	public class NativeCallback : MonoBehaviour
	{
		struct CallbackMessage
		{
			public string message;
			public string content;
		}

		public static NativeCallback Create(string name)
		{
			var guid = System.Guid.NewGuid();
			var callbackObjectName = "_NativeCallback_" + name + "_" + guid.ToString();
			var go = new GameObject(callbackObjectName);
			var ncb = go.AddComponent<NativeCallback>();
			ncb.callbackObjectName = callbackObjectName;
			return ncb;
		}

		string callbackObjectName;
		Dictionary<string, System.Action<string>> callbacks = new Dictionary<string, System.Action<string>>();

		public string New(string name, System.Action<string> callbackDelegate)
		{
			var guid = System.Guid.NewGuid();
			var callbackName = name + "_" + guid.ToString();
			callbacks.Add(callbackName, callbackDelegate);
			var proto = new Dictionary<string, string>();
			proto.Add("name", callbackObjectName);
			proto.Add("message", callbackName);
			return JsonConvert.SerializeObject(proto);
		}


		/*
			message,
			{
				message: string,
				content: string
			}
		 */
		public void DispatchCallback(string jsonString)
		{
			var msg = JsonConvert.DeserializeObject<CallbackMessage>(jsonString);
			System.Action<string> callback;
			if (callbacks.TryGetValue(msg.message, out callback))
			{
				callback(msg.content);
			}
			else
			{
				Debug.LogErrorFormat("unable to process message: {0}", jsonString);
			}
		}

	}
}

