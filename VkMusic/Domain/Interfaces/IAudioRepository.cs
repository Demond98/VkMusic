using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VkMusic.Domain.Core;

namespace VkMusic.Domain.Interfaces
{
	public interface IAudioRepository : IDisposable
	{
		public event EventHandler<(long bitesRecived, long totaBitesToRecive)> LoadingAudioProgressChanged;
		public LinkedList<AudioDTO> GetAllAudios();
		public FileStream GetAudioStream(AudioDTO audioInfo);
	}
}
