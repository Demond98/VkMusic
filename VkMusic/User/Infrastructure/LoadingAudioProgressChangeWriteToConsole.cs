using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Domain.Interfaces;
using VkMusic.User.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class LoadingAudioProgressChangeWriteToConsole : ILoadingAudioProgressChangeExecuter
	{
		const int ProgressBarLength = 20;
		
		public LoadingAudioProgressChangeWriteToConsole(IAudioRepository audioRepository)
		{
			AudioRepository = audioRepository;
		}

		public IAudioRepository AudioRepository { get; }

		public void Invoke(object sender, (long bitesRecived, long totaBitesToRecive) e)
		{
			lock (sender)
			{
				var loadedPart = (int)(ProgressBarLength * e.bitesRecived / e.totaBitesToRecive);
				
				if (loadedPart == 0)
				{
					Console.SetCursorPosition(0, 6);
					Console.Write(new string(' ', Console.WindowWidth));
					Console.SetCursorPosition(0, 6);
					Console.Write($"[{new string(' ', ProgressBarLength)}]");
				}
				else if (e.bitesRecived == e.totaBitesToRecive)
				{
					Console.SetCursorPosition(0, 6);
					Console.Write(new string(' ', ProgressBarLength + 2));
				}
				else if (loadedPart != ProgressBarLength)
				{
					Console.SetCursorPosition(1, 6);
					Console.Write(new string('#', loadedPart));
				}
			}
		}
	}
}
