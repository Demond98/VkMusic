using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;

namespace VkMusic.Application.Infrastructure
{
	public sealed class AudioPlayerNAudio : IAudioPlayer
	{
		private readonly WaveOutEvent _waveOutEvent;
		private readonly AutoResetEvent _autoResetEvent;

		public AudioPlayerNAudio()
		{
			_waveOutEvent = new WaveOutEvent();
			_autoResetEvent = new AutoResetEvent(false);
			_waveOutEvent.PlaybackStopped += PlaybackHandler;
		}

		private void PlaybackHandler(object sender, StoppedEventArgs args)
			=> _autoResetEvent.Set();

		public PlayerState CurrentState => _waveOutEvent.PlaybackState switch
		{
			PlaybackState.Stopped => PlayerState.Stopped,
			PlaybackState.Playing => PlayerState.Playing,
			PlaybackState.Paused => PlayerState.Paused,
			_ => throw new NotImplementedException()
		};

		public Task PlayAudio(Stream audioStream)
		{
			lock (_waveOutEvent)
			{
				_waveOutEvent.PlaybackStopped -= PlaybackHandler;

				using var reader = new StreamMediaFoundationReader(audioStream);
				_waveOutEvent.Stop();
				_waveOutEvent.Init(reader);
				_waveOutEvent.Play();

				_waveOutEvent.PlaybackStopped += PlaybackHandler;
			}

			return Task.Run(_autoResetEvent.WaitOne);
		}

		public Task Pause()
		{
			lock (_waveOutEvent)
			{
				if (_waveOutEvent.PlaybackState != PlaybackState.Paused)
					_waveOutEvent.Pause();
			}

			return Task.CompletedTask;
		}

		public Task Unpause()
		{
			lock (_waveOutEvent)
			{
				if (_waveOutEvent.PlaybackState == PlaybackState.Paused)
					_waveOutEvent.Play();
			}

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_waveOutEvent.Dispose();
			_autoResetEvent.Dispose();
		}
	}
}
