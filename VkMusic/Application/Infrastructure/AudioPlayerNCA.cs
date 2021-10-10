using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VkMusic.Application.Interfaces;
using VkMusic.Infrastructure;

namespace VkMusic.Application.Infrastructure
{
	public class AudioPlayerNCA : IAudioPlayer
	{
		NetCoreAudio.Player _player = new NetCoreAudio.Player();

		public bool OnPause { get => _player.Paused; set => throw new NotImplementedException(); }

		public event EventHandler AudioPlayingEnded;
		public event EventHandler AudioChanged;

		public AudioPlayerNCA()
		{
			_player.PlaybackFinished += AudioPlayingEnded;
		}

		public void PlayAudio(Stream audioStream)
		{
			var fs = audioStream as FileStream;
			_player.Play(fs.Name);
			AudioChanged?.Invoke(this, null);
		}
		
		public void Dispose()
		{

		}
	}
}
