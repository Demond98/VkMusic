using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Infrastructure;

namespace VkMusic.Application.Infrastructure
{
	/*
	public class AudioPlayerNCA : IAudioPlayer
	{
		private readonly NetCoreAudio.Player _player;

		public event EventHandler AudioPlayingEnded;
		public event EventHandler AudioChanged;

		public AudioPlayerNCA()
		{
			_player = new NetCoreAudio.Player();
			_player.PlaybackFinished += (s, e) => AudioPlayingEnded?.Invoke(this, e);
		}

		public async Task Pause()
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

			else
				throw new NotSupportedException("NCA support only FileStream");
		}
		
		private async Task PlayAudio(FileStream audioStream)
		{
			var playTask = _player.Play(audioStream.Name);
			AudioChanged?.Invoke(this, new EventArgs());
			await playTask;
		}
		
		public void Dispose()
		{

		}
	}
	*/
}