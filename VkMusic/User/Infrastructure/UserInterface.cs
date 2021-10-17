using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class UserInterface : IUserInterface
	{
		private readonly IAudioPlaylist _audioPlaylist;

		public UserInterface(IAudioPlaylist audioPlaylist)
		{
			_audioPlaylist = audioPlaylist;
		}

		public void Invoke()
		{
			var key = default(ConsoleKey);

			while (key != ConsoleKey.Escape)
			{
				key = Console.ReadKey(true).Key;

				switch (key)
				{
					case ConsoleKey.A:
						_audioPlaylist.PlayPrevious();
						break;
					case ConsoleKey.D:
						_audioPlaylist.PlayNext();
						break;
					case ConsoleKey.Spacebar:
						_audioPlaylist.HandlePause();
						break;
				}
			}
		}
	}
}
