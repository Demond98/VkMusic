using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VkMusic.Domain.Core;

namespace VkMusic.Application.Interfaces
{
	public interface IAudioPlayer
	{
		public event EventHandler AudioPlayingStopped;
		public event EventHandler SongChanged;
		public bool OnPause { get; set; }
		public void PlayAudio(Stream audioStream);
	}
}
