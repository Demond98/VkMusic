using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Domain.Core;

namespace VkMusic.Domain.Interfaces
{
	public interface IAudioRepository : IDisposable
	{
		public Task<LinkedList<AudioDTO>> GetAllAudios();
		public Task<Stream> GetAudioStream(AudioDTO audioInfo, IProgress<(long BytesReceived, long TotalBytesToReceive)> progress);
	}
}