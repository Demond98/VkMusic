using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Infrastructure;

namespace VkMusic.Application.Infrastructure
{
	public class AudioPlayerNCA : IAudioPlayer
	{
		private readonly NetCoreAudio.Player _player = new NetCoreAudio.Player();

		public event EventHandler AudioPlayingEnded;
		public event EventHandler AudioChanged;

		public AudioPlayerNCA()
		{
			_player.PlaybackFinished += (s, e) => AudioPlayingEnded?.Invoke(this, e);
		}

		public async Task HandlePause()
		{
			if (_player.Paused)
				await _player.Resume();
			else
				await _player.Pause();
		}

		public async Task PlayAudio(Stream audioStream)
		{
			if (audioStream is FileStream fileStream)
				await PlayAudio(fileStream);
		}
		
		private async Task PlayAudio(FileStream audioStream)
		{
			await _player.Play(audioStream.Name);
			AudioChanged?.Invoke(this, null);
		}
		
		public void Dispose()
		{

		}
	}
}
