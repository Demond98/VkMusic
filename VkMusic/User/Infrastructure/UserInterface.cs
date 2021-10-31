using System;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.User.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class UserInterface : IUserInterface
	{
		private const int ProgressBarLength = 20;

		private readonly IAudioPlaylist _audioPlaylist;
		private readonly IAudioChangeExecuter _audioChangeExecuter;
		private readonly ILoadingAudioProgressChangeExecuter _loadingAudioProgressChangeExecuter;

		public UserInterface(IAudioPlaylist audioPlaylist,
					   IAudioChangeExecuter audioChangeExecuter,
					   ILoadingAudioProgressChangeExecuter loadingAudioProgressChangeExecuter)
		{
			_audioPlaylist = audioPlaylist;
			_audioChangeExecuter = audioChangeExecuter;
			_loadingAudioProgressChangeExecuter = loadingAudioProgressChangeExecuter;
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
			return PlayAudio(_audioPlaylist.PlayNext)
				.ContinueWith(async _ => await PlayNext(), TaskContinuationOptions.DenyChildAttach);
		}

		private Task PlayPrevious()
		{
			return PlayAudio(_audioPlaylist.PlayPrevious)
				.ContinueWith(async _ => await PlayNext(), TaskContinuationOptions.DenyChildAttach);
		}

		private Task PlayAudio(Func<Action<(long, long)>, Task> playAudioFunc)
		{
			var task = playAudioFunc(_loadingAudioProgressChangeExecuter.ProgressHandler);
			_audioChangeExecuter.Invoke();

			return task;
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