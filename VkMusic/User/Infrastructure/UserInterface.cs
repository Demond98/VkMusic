using System;
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

		public void Invoke()
		{
			Console.CursorVisible = false;
			DrawInterface();

			Task.Run(PlayNext);
			
			while (true)
			{
				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.LeftArrow:
					case ConsoleKey.A:
						Task.Run(PlayNext);
						break;

					case ConsoleKey.RightArrow:
					case ConsoleKey.D:
						Task.Run(PlayPrevious);
						break;

					case ConsoleKey.Spacebar:
						Task.Run(HandlePause);
						break;

					case ConsoleKey.Escape:
						return;
				}
			}
		}

		private Task HandlePause()
		{
			return _audioPlaylist.CurrentState switch
			{
				PlayerState.Paused => _audioPlaylist.UnPause(),
				PlayerState.Playing => _audioPlaylist.Pause(),
				_ => Task.CompletedTask,
			};
		}

		private Task PlayNext()
		{
			return PlayAudio(_audioPlaylist.PlayNext, _audioPlaylist.NextAudio)
				.ContinueWith(async _ => await PlayNext(), TaskContinuationOptions.DenyChildAttach);
		}

		private Task PlayPrevious()
		{
			return PlayAudio(_audioPlaylist.PlayPrevious, _audioPlaylist.PreviousAudio)
				.ContinueWith(async _ => await PlayNext(), TaskContinuationOptions.DenyChildAttach);
		}

		private static Task PlayAudio(Func<Action<(long, long)>, Task> playAudioFunc, AudioDTO currentAudio)
		{
			var task = playAudioFunc(ProgressHandler);
			DrawAudioInfo(currentAudio);

			return task;
		}

		private static void DrawAudioInfo(AudioDTO currentAudio)
		{
			if (currentAudio == null)
				return;

			var title = currentAudio.Title.Replace('\n', ' ');
			var artist = currentAudio.Artist.Replace('\n', ' ');
			var duration = currentAudio.DurationInSeconds;

			var info = $"{title} - {artist} - {duration}sec";
			var infoToShow = info.Truncate(Console.WindowWidth - 1);

			Console.SetCursorPosition(0, 6);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, 6);
			Console.WriteLine(infoToShow);
		}

		private static readonly object _syncObject = new();
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