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
			Console.CursorVisible = false;

			DrawInterface();

			_audioPlaylist.PlayNext();

			var key = default(ConsoleKey);
			while (key != ConsoleKey.Escape)
			{
				key = Console.ReadKey(true).Key;

				switch (key)
				{
					case ConsoleKey.LeftArrow:
					case ConsoleKey.A:
						_audioPlaylist.PlayPrevious().Wait();
						break;

					case ConsoleKey.RightArrow:
					case ConsoleKey.D:
						_audioPlaylist.PlayNext().Wait();
						break;

					case ConsoleKey.Spacebar:
						_audioPlaylist.HandlePause().Wait();
						break;
				}
			}
		}

		private static void DrawInterface()
		{
			Console.Clear();
			Console.WriteLine("VkMusic");
			Console.WriteLine("[A] - Previos song");
			Console.WriteLine("[D] - Next song");
			Console.WriteLine("[Spacebar] - Pause/unpause");
			Console.WriteLine("[Esc] - Exit");
		}
	}
}