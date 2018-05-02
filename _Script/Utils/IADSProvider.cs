using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace x600d1dea.utils
{
	public class ADSCallbacks
	{
		public delegate void Callback(string ret);
		

		public static event Callback onRewardSuccess;
		public static event Callback onRewardFailed;


		public static void OnRewardSuccess(string ret)
		{
			onRewardSuccess(ret);
		}
		public static void OnRewardFailed(string ret)
		{
			onRewardFailed(ret);

		}
	}

	public interface IADSProvider
	{
		bool CouldShow(string ID);
		void Show(string ID);
	}
}

