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
		public event EventHandler AudioPlayingEnded;
		public event EventHandler AudioChanged;
		public Task HandlePause();
		public Task PlayAudio(Stream audioStream);
	}
}
