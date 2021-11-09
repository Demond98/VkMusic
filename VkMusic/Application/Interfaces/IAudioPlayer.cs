using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Domain.Core;

namespace VkMusic.Application.Interfaces
{
	public interface IAudioPlayer : IDisposable
	{
		public PlayerState CurrentState { get; }
		public Task PlayAudioAsync(Stream audioStream);
		public Task PauseAsync();
		public Task UnpauseAsync();
		public Task StopAsync();
	}
}
