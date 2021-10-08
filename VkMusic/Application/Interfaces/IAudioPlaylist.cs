using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;

namespace VkMusic.Application.Interfaces
{
	public interface IAudioPlaylist
	{
		public IAudioPlayer AudioPlayer { get; }
		public IAudioRepository AudioRepository { get; }
		public AudioDTO CurrentAudio { get; }
		public void PlayNext();
		public void PlayPrevious();
	}
}
