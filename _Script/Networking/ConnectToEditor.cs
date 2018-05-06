using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using System;
using System.Text;
using System.Collections.Generic;
using x600d1dea.stubs.utils;

namespace x600d1dea.stubs.networking
{
	public class ConnectToEditor : MonoBehaviour
	{

		static ConnectToEditor instance_;
		public static ConnectToEditor instance
		{
			get
			{
				if (instance_ == null)
				{
					var go = new GameObject("_ConnectToEditor");
					instance_ = go.AddComponent<ConnectToEditor>();
					DontDestroyOnLoad(go);
				}
				return instance_;
			}
		}

		PlayerConnection playerConnection;
		int currentEditorID = -1;

		void Awake()
		{
			playerConnection = PlayerConnection.instance; 
			playerConnection.RegisterConnection(OnEditorConnected);
			playerConnection.RegisterDisconnection(OnEditorDisconnected);
			playerConnection.Register(EditorConnectionMessageID.Editor, OnEditorMessageReceived);
		}
		void OnDestroy()
		{
			playerConnection.Unregister(EditorConnectionMessageID.Editor, OnEditorMessageReceived);
			playerConnection.DisconnectAll();
		}

		void OnEditorConnected(int editorID)
		{
			Debug.LogFormat("OnEditorConnected {0}", editorID);
			currentEditorID = editorID;
		}

		void OnEditorDisconnected(int editorID)
		{
			Debug.LogFormat("OnEditorDisconnectedConnected {0}", editorID);
			if (currentEditorID == editorID)
				currentEditorID = -1;
			playerConnection.DisconnectAll();
		}

		public event Action<string, List<string>> onEditorMessageReceived;
		
		public void OnEditorMessageReceived(MessageEventArgs args)
		{
			var jsonString = args.data.Deserialize<string>();
			Debug.LogFormat("OnEditorMessageReceived {0} {1}", args.playerId, jsonString);
			if (args.playerId == currentEditorID)
			{
				var retStrings = new List<string>();
				onEditorMessageReceived(jsonString, retStrings);
				if (playerConnection.isConnected)
				{
					foreach (var r in retStrings)
					{
						playerConnection.Send(EditorConnectionMessageID.Player, r.SerializeToByteArray());
					}
				}
			}
		}

	}



}
