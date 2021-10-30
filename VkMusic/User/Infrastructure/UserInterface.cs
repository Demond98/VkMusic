using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;

namespace VkMusic.User.Infrastructure
{
	public class UserInterface : IUserInterface
	{
		const int ProgressBarLength = 20;

		private readonly IAudioPlaylist _audioPlaylist;

		public UserInterface(IAudioPlaylist audioPlaylist)
		{
			_audioPlaylist = audioPlaylist;
		}

		public async Task Invoke()
		{
			Console.CursorVisible = false;
			DrawInterface();

			var canselTokenSource = new CancellationTokenSource();
			
			var task = PlayNext().ContinueWith(async a => { while (true) await PlayNext(); });
			
			while (true)
			{
				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.LeftArrow:
					case ConsoleKey.A:
						task = PlayPrevious().ContinueWith(async a => { while (true) await PlayNext(); });
						break;

					case ConsoleKey.RightArrow:
					case ConsoleKey.D:
						task = PlayNext().ContinueWith(async a => { while (true) await PlayNext(); });
						break;

					case ConsoleKey.Spacebar:
						await HandlePause();
						break;

					case ConsoleKey.Escape:
						return;
				}
			}
		}

		private Task HandlePause()
		{
			if (_audioPlaylist.CurrentState == PlayerState.Paused)
				return _audioPlaylist.Unpase();

			if (_audioPlaylist.CurrentState == PlayerState.Playing)
				return _audioPlaylist.Pause();

			return Task.CompletedTask;
		}

		private Task PlayNext()
			=> PlayAudio(_audioPlaylist.PlayNext, _audioPlaylist.CurrentAudio);

		private Task PlayPrevious()
			=> PlayAudio(_audioPlaylist.PlayPrevious, _audioPlaylist.CurrentAudio);

		private static Task PlayAudio(Func<Action<(long, long)>, Task> playAudioFunc, AudioDTO currentAudio)
		{
			var task = playAudioFunc(ProgressHandler);
			DrawAudioInfo(currentAudio);
			return task;
		}

		private static void DrawAudioInfo(AudioDTO currentAudio)
		{
			var title = currentAudio.Title.Replace('\n', ' ');
			var artist = currentAudio.Artist.Replace('\n', ' ');

			Console.SetCursorPosition(0, 6);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, 6);
			Console.WriteLine($"{title} - {artist}".Truncate(Console.WindowWidth - 1));
		}

		private static object _syncObject = new();
		private static void ProgressHandler((long BytesReceived, long TotalBytesToReceive) e)
		{
			lock (_syncObject)
			{
				var loadedPart = (int)(ProgressBarLength * e.BytesReceived / e.TotalBytesToReceive);

				if (loadedPart == 0)
				{
					Console.SetCursorPosition(0, 8);
					Console.Write(new string(' ', Console.WindowWidth));
					Console.SetCursorPosition(0, 8);
					Console.Write($"[{new string(' ', ProgressBarLength)}]");
				}
				else if (e.BytesReceived == e.TotalBytesToReceive)
				{
					Console.SetCursorPosition(0, 8);
					Console.Write(new string(' ', ProgressBarLength + 2));
				}
				else if (loadedPart != ProgressBarLength)
				{
					Console.SetCursorPosition(1, 8);
					Console.Write(new string('#', loadedPart));
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