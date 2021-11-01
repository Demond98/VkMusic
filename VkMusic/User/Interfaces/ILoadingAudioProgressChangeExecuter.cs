using System;

namespace VkMusic.User.Interfaces
{
	public interface ILoadingAudioProgressChangeExecuter
	{
		public void ProgressHandler((long BytesReceived, long TotalBytesToReceive) data);
	}
}
