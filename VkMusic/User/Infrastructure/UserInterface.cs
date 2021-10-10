using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class UserInterface : IUserInterface
	{
		public IAudioPlaylist _audioPlaylist;

		public UserInterface(IAudioPlaylist audioPlaylist)
		{
			_audioPlaylist = audioPlaylist;
		}

		private Dictionary<ConsoleKey, Action<IAudioPlaylist>> Commands { get; }
			= new Dictionary<ConsoleKey, Action<IAudioPlaylist>>
			{
				[ConsoleKey.LeftArrow] = playlist => playlist.PlayPrevious(),
				[ConsoleKey.RightArrow] = playlist => playlist.PlayNext(),
				[ConsoleKey.Spacebar] = playlist => playlist.AudioPlayer.OnPause = !playlist.AudioPlayer.OnPause,
			};

		public void Invoke()
		{
			var key = default(ConsoleKey);

			while (key != ConsoleKey.Escape)
			{
				key = Console.ReadKey(true).Key;
				
				if(Commands.ContainsKey(key))
					Commands[key].Invoke(_audioPlaylist);
			}
		}
	}
}
