using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace x600d1dea.stubs.utils
{
	public class MockVoiceEvalProvider : IVoiceEvalProvider
	{
		public bool IsRecording()
		{
			return false;
		}

		public bool StartFileEval(string toEval, string filePath)
		{
			Debug.LogFormat("MockVoiceEvalProvider.StartFileEval \"{0}\" \"{1}\"", toEval, filePath);
			return true;
		}

		public bool StartRecordEval(string toEval)
		{
			Debug.LogFormat("MockVoiceEvalProvider.StartRecordEval \"{0}\"", toEval);
			VoiceEvalCallbacks.OnResult("on result");
			VoiceEvalCallbacks.OnEnd("on end");
			return true;
		}

		public void StopRecord()
		{
			Debug.LogFormat("MockVoiceEvalProvider.StopRecord");
		}
	}

}

