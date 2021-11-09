using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;

namespace VkMusic.Application.Infrastructure
{
	public sealed class AudioPlayerNAudio : IAudioPlayer
	{
		private readonly WaveOutEvent _waveOutEvent;

		public AudioPlayerNAudio()
		{
			_waveOutEvent = new WaveOutEvent();
		}

		public PlayerState CurrentState => _waveOutEvent.PlaybackState switch
		{
			PlaybackState.Stopped => PlayerState.Stopped,
			PlaybackState.Playing => PlayerState.Playing,
			PlaybackState.Paused => PlayerState.Paused,
			_ => throw new NotImplementedException()
		};

		public Task PlayAudioAsync(Stream audioStream)
		{
			var tsc = new TaskCompletionSource<StoppedEventArgs>();
			void lambda(object s, StoppedEventArgs e)
			{
				_waveOutEvent.PlaybackStopped -= lambda;
				if (e.Exception != null)
					tsc.SetException(e.Exception);
				
				tsc.SetResult(e);
			}

			lock (_waveOutEvent)
			{
				using var reader = new StreamMediaFoundationReader(audioStream);
				_waveOutEvent.Stop();
				_waveOutEvent.Init(reader);
				_waveOutEvent.Play();
				_waveOutEvent.PlaybackStopped += lambda;
			}

			return tsc.Task;
		}

		public Task PauseAsync()
		{
			lock (_waveOutEvent)
			{
				if (_waveOutEvent.PlaybackState != PlaybackState.Paused)
					_waveOutEvent.Pause();
			}

			return Task.CompletedTask;
		}

		public Task UnpauseAsync()
		{
			lock (_waveOutEvent)
			{
				if (_waveOutEvent.PlaybackState == PlaybackState.Paused)
					_waveOutEvent.Play();
			}

			return Task.CompletedTask;
		}
		public Task StopAsync()
		{
			lock (_waveOutEvent)
			{
				_waveOutEvent.Stop();
			}

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_waveOutEvent.Dispose();
		}
	}
}