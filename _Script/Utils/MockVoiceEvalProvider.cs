using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


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
			var result = new Dictionary<string, object>();
			result.Add("wavPath", "D:\\test.wav");
			var resultObject = new Dictionary<string, object>();
			result.Add("result", new Dictionary<string, object>(){
				{ "refText", toEval },
				{ "result", resultObject }
			});
			resultObject.Add("overall", 70);
			resultObject.Add("accuracy", 70);
			resultObject.Add("fluency", new Dictionary<string, int>() {
				{"overall", 70 },
				{"pause", 0 },
				{"speed", 70 },
			});
			resultObject.Add("rhythm", new Dictionary<string, int>() {
				{"overall", 70 },
				{"stress", 0 },
				{"sense", 70 },
				{"tone", 0 },
			});
			var jsonString = JsonConvert.SerializeObject(result);
			VoiceEvalCallbacks.OnResult(jsonString);
			VoiceEvalCallbacks.OnEnd("on end");
			return true;
		}

		public void StopRecord()
		{
			Debug.LogFormat("MockVoiceEvalProvider.StopRecord");
		}
	}

}

