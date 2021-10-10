using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VkMusic.Application.Interfaces;

namespace VkMusic.Application.Infrastructure
{
	public class AudioPlayerIrrKlang : IAudioPlayer
	{
		//private ManagedBass.MediaPlayer _player;

		public bool OnPause { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public event EventHandler AudioPlayingEnded;
		public event EventHandler SongChanged;

		public AudioPlayerIrrKlang()
		{
			
		}

		public void PlayAudio(Stream audioStream)
		{
			var path = Path.GetTempFileName();
			File.WriteAllText(path, ReadText(audioStream));
			
			var engine = new IrrKlang.ISoundEngine();
			engine.Play2D(path);
		}

		public static string ReadText(Stream input)
		{
			using var ms = new MemoryStream();
			input.CopyTo(ms);
			var array = ms.ToArray();
			return Encoding.UTF8.GetString(array);
		}

		public void Dispose()
		{

		}
	}
}
