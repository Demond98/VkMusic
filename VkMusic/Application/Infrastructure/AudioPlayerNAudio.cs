using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;

namespace VkMusic.Application.Infrastructure
{
	public sealed class AudioPlayerNAudio : IAudioPlayer
	{
		private WaveOutEvent _waveOutEvent;

		public event EventHandler AudioPlayingEnded;
		public event EventHandler AudioChanged;

		public AudioPlayerNAudio()
		{
			_waveOutEvent = new WaveOutEvent();
			_waveOutEvent.PlaybackStopped += InvokeAudioPlaingStoped;
		}

		public Task HandlePause()
		{
			lock (_waveOutEvent)
			{
				if (_waveOutEvent.PlaybackState == PlaybackState.Paused)
					_waveOutEvent.Play();
				else
					_waveOutEvent.Pause();
			}

			return Task.Delay(1);
		}

		public Task PlayAudio(Stream audioStream)
		{
			lock (_waveOutEvent)
			{
				_waveOutEvent.PlaybackStopped -= InvokeAudioPlaingStoped;

				using var reader = new StreamMediaFoundationReader(audioStream);
				_waveOutEvent.Stop();
				_waveOutEvent.Init(reader);
				_waveOutEvent.Play();
				AudioChanged?.Invoke(this, null);

				_waveOutEvent.PlaybackStopped += InvokeAudioPlaingStoped;
			}

			return Task.Delay(1);
		}

		private void InvokeAudioPlaingStoped(object sender, StoppedEventArgs e)
		{
			AudioPlayingEnded?.Invoke(this, e);
		}

		public void Dispose()
		{

		}
	}
}
