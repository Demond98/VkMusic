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
		public Task<LinkedList<AudioDTO>> GetAllAudiosAsync();
		public Task<Stream> GetAudioStreamAsync(AudioDTO audioInfo, IProgress<(long BytesReceived, long TotalBytesToReceive)> progress);
	}
}