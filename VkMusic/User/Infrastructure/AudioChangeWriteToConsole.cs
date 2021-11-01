using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.User.Interfaces;
using VkMusic.Utils;

namespace VkMusic.User.Infrastructure
{
	public class AudioChangeWriteToConsole : IAudioChangeExecuter
	{
		private const int ConsoleLineIndex = 6;

		public AudioChangeWriteToConsole()
		{

		}

		public void Invoke(AudioDTO audioInfo)
		{
			if (audioInfo == null)
				return;

			var title = audioInfo.Title.Replace('\n', ' ');
			var artist = audioInfo.Artist.Replace('\n', ' ');
			var duration = audioInfo.DurationInSeconds;

			var info = $"{title} - {artist} - {duration}sec";
			var infoToShow = info.Truncate(Console.WindowWidth - 1);

			Console.SetCursorPosition(0, ConsoleLineIndex);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, ConsoleLineIndex);
			Console.WriteLine(infoToShow);
		}
	}
}