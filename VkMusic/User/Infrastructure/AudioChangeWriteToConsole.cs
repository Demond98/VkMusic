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
		private readonly IAudioPlaylist _audioPlayList;

		public AudioChangeWriteToConsole(IAudioPlaylist audioPlaylist)
		{
			_audioPlayList = audioPlaylist;
		}

		public void Invoke()
		{
			var audio = _audioPlayList.CurrentAudio;
			if (audio == null)
				return;

			var title = audio.Title.Replace('\n', ' ');
			var artist = audio.Artist.Replace('\n', ' ');
			var duration = audio.DurationInSeconds;

			var info = $"{title} - {artist} - {duration}sec";
			var infoToShow = info.Truncate(Console.WindowWidth - 1);

			Console.SetCursorPosition(0, 6);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, 6);
			Console.WriteLine(infoToShow);
		}
	}
}

