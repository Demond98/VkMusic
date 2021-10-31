using System;
using VkMusic.Domain.Interfaces;
using VkMusic.User.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class LoadingAudioProgressChangeWriteToConsole : ILoadingAudioProgressChangeExecuter
	{
		private const int ProgressBarLength = 20;
		private static readonly object _syncObject = new();

		public LoadingAudioProgressChangeWriteToConsole(IAudioRepository audioRepository)
		{
			AudioRepository = audioRepository;
		}

		public IAudioRepository AudioRepository { get; }

		public void ProgressHandler((long BytesReceived, long TotalBytesToReceive) data)
		{
			var bytesReceived = data.BytesReceived;
			var totalBytesToReceive = data.TotalBytesToReceive;

			lock (_syncObject)
			{
				var loadedPart = (int)(ProgressBarLength * bytesReceived / totalBytesToReceive);

				if (loadedPart == 0)
				{
					Console.SetCursorPosition(0, 8);
					Console.Write(new string(' ', Console.WindowWidth));
					Console.SetCursorPosition(0, 8);
					Console.Write($"[{new string(' ', ProgressBarLength)}]");
				}
				else if (bytesReceived == totalBytesToReceive)
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
	}
}
