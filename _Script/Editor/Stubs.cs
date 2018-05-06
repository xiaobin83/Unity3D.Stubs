using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace x600d1dea.stubs
{
	public class Stubs 
	{
		[MenuItem("Unity3D.Stubs/Connect To Player")]
		static void ShowConnectToPlayer()
		{
			var wnd = EditorWindow.GetWindow<networking.ConnectToPlayer>("Connect To Player");
			wnd.ShowUtility();
		}
	
	}
}

