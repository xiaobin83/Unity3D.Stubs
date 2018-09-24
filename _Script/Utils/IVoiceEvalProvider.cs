using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace x600d1dea.stubs.utils
{
	public class VoiceEvalCallbacks
	{
		public delegate void Callback(string ret);


		public static event Callback onResult;
		public static event Callback onEnd;


		public static void OnResult(string message)
		{
			onResult(message);
		}

		public static void OnEnd(string message)
		{
			onEnd(message);
		}

	}

	public interface IVoiceEvalProvider
	{
		bool StartRecordEval(string toEval);
		bool StartFileEval(string toEval, string filePath);
		void StopRecord();
		bool IsRecording();
	}
}

