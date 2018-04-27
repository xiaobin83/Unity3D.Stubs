using System;

namespace x600d1dea.networking
{
	public interface IChanModule : IDisposable 
	{
		// main thread
		void Init(Chan chan);

		// worker thread
		void ProcessSendingBytes(ref byte[] data);
		// worker thread
		void ProcessReceivedBytes(ref byte[] data);
	}
}
