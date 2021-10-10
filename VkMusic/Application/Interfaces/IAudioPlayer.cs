using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VkMusic.Domain.Core;

namespace VkMusic.Application.Interfaces
{
	public interface IAudioPlayer : IDisposable
	{
		public event EventHandler AudioPlayingEnded;
		public event EventHandler AudioChanged;
		public bool OnPause { get; set; }
		public void PlayAudio(Stream audioStream);
	}
}
