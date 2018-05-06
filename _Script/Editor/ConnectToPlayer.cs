using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking.PlayerConnection;
using UnityEditor;
using UnityEditor.Networking.PlayerConnection;
using System.Text;
using System.Reflection;
using System.Linq;
using UnityEngine;
using x600d1dea.stubs.utils;

namespace x600d1dea.stubs.networking
{

	public class PlayerConnectorAttribute : Attribute
	{
		public string name = string.Empty;
		public PlayerConnectorAttribute(string name)
		{
			this.name = name;
		}

		static List<Action<bool, ConnectToPlayer>> GetAllConnectors()
		{
			var c = new List<Action<bool, ConnectToPlayer>>();
			IEnumerable<Type> allTypes = null;
			try
			{
				allTypes = Assembly.Load("Assembly-CSharp-Editor").GetTypes().AsEnumerable();
			}
			catch
			{ }
			try
			{
				var typesInPlugins = Assembly.Load("Assembly-CSharp-Editor-firstpass").GetTypes();
				if (allTypes != null)
					allTypes = allTypes.Union(typesInPlugins);
				else
					allTypes = typesInPlugins;
			}
			catch
			{ }
			if (allTypes == null)
			{
				return c;
			}

			var methods = allTypes.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				.Where(m => m.GetCustomAttributes(typeof(PlayerConnectorAttribute), false).Length > 0)
				.OrderBy(m =>
				{
					var attr = (PlayerConnectorAttribute)m.GetCustomAttributes(typeof(PlayerConnectorAttribute), false)[0];
					return attr.name;
				})
				.ToArray();
			for (int i = 0; i < methods.Length; ++i)
			{
				var m = methods[i];
				var cc = (Action<bool, ConnectToPlayer>)Delegate.CreateDelegate(typeof(Action<bool, ConnectToPlayer>), m);
				c.Add(cc);
			}
			return c;
		}

		public static void Connect(ConnectToPlayer ctp)
		{
			var ccs = GetAllConnectors();
			foreach (var c in ccs)
			{
				c(true, ctp);
			}
		}

		public static void Disconnect(ConnectToPlayer ctp)
		{
			var ccs = GetAllConnectors();
			foreach (var c in ccs)
			{
				c(false, ctp);
			}
		}
	}

	public class ConnectToPlayer : EditorWindow
	{
		EditorConnection editorConnection;
		int currentPlayerID = -1;


		void OnEnable()
		{
			editorConnection = EditorConnection.instance;
			editorConnection.Initialize();
			editorConnection.RegisterConnection(OnPlayerConnected);
			editorConnection.RegisterDisconnection(OnPlayerDisconnected);
			editorConnection.Register(EditorConnectionMessageID.Player, OnPlayerMessageReceived);
			PlayerConnectorAttribute.Connect(this);
		}

		void OnDisable()
		{
			PlayerConnectorAttribute.Disconnect(this);
			editorConnection.Unregister(EditorConnectionMessageID.Player, OnPlayerMessageReceived);
			editorConnection.DisconnectAll();
			editorConnection = null;
		}

		void OnPlayerConnected(int playerID)
		{
			Debug.LogFormat("OnPlayerConnected {0}", playerID);
			currentPlayerID = playerID;
		}

		void OnPlayerDisconnected(int playerID)
		{
			if (currentPlayerID == playerID)
				currentPlayerID = -1;
		}


		public event Action onGUI;
		public event Action<string, List<string>> onPlayerMessageReceived;

		void OnGUI()
		{
			if (onGUI != null)
				onGUI();
		}

		void OnPlayerMessageReceived(MessageEventArgs args)
		{
			var jsonString = args.data.Deserialize<string>();
			Debug.LogFormat("OnPlayerMessageReceived {0} {1}", args.playerId, jsonString);
			if (args.playerId == currentPlayerID)
			{
				var retStrings = new List<string>();
				if (onPlayerMessageReceived != null)
					onPlayerMessageReceived(jsonString, retStrings);
				foreach (var r in retStrings)
				{
					Send(r);
				}
			}
		}

		public void Send(string message)
		{
			editorConnection.Send(EditorConnectionMessageID.Editor, message.SerializeToByteArray());
		}

	}

}
