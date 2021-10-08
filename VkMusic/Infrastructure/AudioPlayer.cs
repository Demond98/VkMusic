using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VkMusic.Application.Interfaces;

namespace VkMusic.Infrastructure
{
	public class AudioPlayer : IAudioPlayer
	{
		private WaveOutEvent _waveOutEvent;

		public event EventHandler AudioPlayingStopped;
		public event EventHandler SongChanged;

		public AudioPlayer()
		{
			_waveOutEvent = new WaveOutEvent();
			_waveOutEvent.PlaybackStopped += InvokeAudioPlaingStoped;
		}

		public bool _onPause;
		public bool OnPause
		{
			get => _onPause;
			set
			{
				if (_onPause == value)
					return;

				_onPause = value;

				if (value)
					_waveOutEvent.Pause();
				else
					_waveOutEvent.Play();
			}
		}
		
		public void PlayAudio(Stream audioStream)
		{
			lock(_waveOutEvent)
			{
				_waveOutEvent.PlaybackStopped -= InvokeAudioPlaingStoped;

				using var reader = new StreamMediaFoundationReader(audioStream);
				_waveOutEvent.Stop();
				_waveOutEvent.Init(reader);
				_waveOutEvent.Play();
				_onPause = false;
				SongChanged?.Invoke(this, null);

				_waveOutEvent.PlaybackStopped += InvokeAudioPlaingStoped;
			}
		}

		private void InvokeAudioPlaingStoped(object sender, StoppedEventArgs e)
		{
			Console.WriteLine("invoke InvokeAudioPlaingStoped");
			AudioPlayingStopped?.Invoke(this, null);
		}
	}
}
