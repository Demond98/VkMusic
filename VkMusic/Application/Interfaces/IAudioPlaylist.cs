using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;

namespace VkMusic.Application.Interfaces
{
	public interface IAudioPlaylist
	{
		public PlayerState CurrentState { get; }
		public AudioDTO CurrentAudio { get; }
		public Task Pause();
		public Task Unpase();
		public Task PlayNext(Action<(long BytesReceived, long TotalBytesToReceive)> progress);
		public Task PlayPrevious(Action<(long BytesReceived, long TotalBytesToReceive)> progress);
	}
}
