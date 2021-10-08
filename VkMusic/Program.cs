using AutoMapper;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;
using VkMusic.Infrastructure;

namespace VkMusic
{
	public static class Program
	{
		static void Main(string[] args)
		{
			AutomapperUtil.Initilize();

			var ownerId = 123456789;
			var token = "";

			IAudioRepository audioRepository = new AudioRepository(ownerId, token);
			IAudioPlayer audioPlayer = new AudioPlayer();
			IAudioPlaylist audioPlaylist = new AudioPlaylist(audioPlayer, audioRepository);

			audioRepository.LoadingAudioProgressChanged += WriteProcent;
			audioPlaylist.AudioPlayer.SongChanged += (s, e) => WriteAudioTitle(s, audioPlaylist.CurrentAudio);

			audioPlaylist.PlayNext();

			while (true)
			{
				var keyInfo = Console.ReadKey(true);

				switch (keyInfo.Key)
				{
					case ConsoleKey.LeftArrow:
						audioPlaylist.PlayPrevious();
						break;

					case ConsoleKey.RightArrow:
						audioPlaylist.PlayNext();
						break;

					case ConsoleKey.Spacebar:
						audioPlaylist.AudioPlayer.OnPause = !audioPlaylist.AudioPlayer.OnPause;
						break;

					case ConsoleKey.Escape:
						return;

					default:
						break;
				}
			}
		}

		private static void WriteProcent(object sender, (long bitesRecived, long totaBitesToRecive) e)
		{
			lock (sender)
			{
				var procent = (int)(100.0 * e.bitesRecived / e.totaBitesToRecive);

				if (procent == 100)
					Console.Write($"     ");
				else
					Console.Write($"{procent}%");

				Console.CursorLeft = 0;
			}
		}

		private static void WriteAudioTitle(object sender, AudioDTO audio)
		{
			lock (sender)
			{
				Console.WriteLine($"Playing {audio.Title} - {audio.Artist} - {audio.Id}");
			}
		}
	}
}
