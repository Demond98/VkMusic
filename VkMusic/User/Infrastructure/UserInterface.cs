using System;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;
using VkMusic.User.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class UserInterface : IUserInterface
	{
		private readonly IAudioPlaylist _audioPlaylist;
		private readonly IAudioPlayer _audioPlayer;
		private readonly IAudioRepository _audioRepository;
		private readonly IAudioChangeExecuter _audioChangeExecuter;
		private readonly ILoadingAudioProgressChangeExecuter _loadingAudioProgressChangeExecuter;

		public UserInterface(IAudioPlayer audioPlayer,
			IAudioPlaylist audioPlaylist,
			IAudioRepository audioRepository,
			IAudioChangeExecuter audioChangeExecuter,
			ILoadingAudioProgressChangeExecuter loadingAudioProgressChangeExecuter)
		{
			_audioPlayer = audioPlayer;
			_audioPlaylist = audioPlaylist;
			_audioRepository = audioRepository;
			_audioChangeExecuter = audioChangeExecuter;
			_loadingAudioProgressChangeExecuter = loadingAudioProgressChangeExecuter;
		}

		public async Task Invoke()
		{
			Console.CursorVisible = false;
			DrawInterface();

			if (_audioPlaylist.CurrentAudio == null)
				_audioPlaylist.Next();

			var readKeyTask = Task.Run(ReadConsoleKeys);

			while (true)
			{
				var audioToPlay = _audioPlaylist.CurrentAudio;

				var progress = new Progress<(long, long)>(_loadingAudioProgressChangeExecuter.ProgressHandler);
				using var audioStream = await _audioRepository.GetAudioStream(audioToPlay, progress);
				
				_audioChangeExecuter.Invoke(audioToPlay);
				
				await _audioPlayer.PlayAudio(audioStream);

				if (audioToPlay == _audioPlaylist.CurrentAudio)
					_audioPlaylist.Next();
			}

			await readKeyTask;
		}

		public async Task ReadConsoleKeys()
		{
			while (true)
			{
				switch (Console.ReadKey(true).Key)
				{
					case ConsoleKey.LeftArrow:
					case ConsoleKey.A:
						_audioPlaylist.Previous();
						await _audioPlayer.Stop();
						break;

					case ConsoleKey.RightArrow:
					case ConsoleKey.D:
						_audioPlaylist.Next();
						await _audioPlayer.Stop();
						break;

					case ConsoleKey.Spacebar:
						await HandlePause();
						break;
				}
			}
		}

		private Task HandlePause()
		{
			return _audioPlayer.CurrentState switch
			{
				PlayerState.Paused => _audioPlayer.Unpause(),
				PlayerState.Playing => _audioPlayer.Pause(),
				_ => Task.CompletedTask,
			};
		}

		private static void DrawInterface()
		{
			Console.Clear();
			Console.WriteLine("VkMusic");
			Console.WriteLine("[A] - Previos song");
			Console.WriteLine("[D] - Next song");
			Console.WriteLine("[Spacebar] - Pause/unpause");
		}
	}
}