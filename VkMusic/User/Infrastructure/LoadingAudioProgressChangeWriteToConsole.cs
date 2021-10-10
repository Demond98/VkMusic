using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Domain.Interfaces;
using VkMusic.User.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class LoadingAudioProgressChangeWriteToConsole : ILoadingAudioProgressChangeExecuter
	{
		public LoadingAudioProgressChangeWriteToConsole(IAudioRepository audioRepository)
		{
			AudioRepository = audioRepository;
		}

		public IAudioRepository AudioRepository { get; }

		public void Invoke(object sender, (long bitesRecived, long totaBitesToRecive) e)
		{
			lock (sender)
			{
				var procent = (int)(100.0 * e.bitesRecived / e.totaBitesToRecive);

				if (procent == 100)
					Console.Write(new string(' ', 4));
				else
					Console.Write($"{procent}%");

				Console.CursorLeft = 0;
			}
		}
	}
}
