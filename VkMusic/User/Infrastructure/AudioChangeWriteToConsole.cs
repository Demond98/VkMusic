using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.User.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class AudioChangeWriteToConsole : IAudioChangeExecuter
	{
		private IAudioPlaylist AudioPlaylist { get; set; }

		public AudioChangeWriteToConsole(IAudioPlaylist audioPlaylist)
		{
			AudioPlaylist = audioPlaylist;
		}

		public void Invoke(object sender, EventArgs args)
		{
			lock (sender)
			{
				var audio = AudioPlaylist.CurrentAudio;
				var title = audio.Title.Replace('\n', ' ');
				var artist = audio.Artist.Replace('\n', ' ');

				Console.SetCursorPosition(0, 6);
				Console.Write(new string(' ', Console.WindowWidth));
				Console.SetCursorPosition(0, 6);
				Console.WriteLine($"{title} - {artist}".Truncate(Console.WindowWidth - 1));
			}
		}
	}
}

