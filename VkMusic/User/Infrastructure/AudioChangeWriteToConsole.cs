using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.User.Interfaces;

namespace VkMusic.User.Infrastructure
{
	public class AudioChangeWriteToConsole : IAudioChangeExecuter
	{
		public AudioChangeWriteToConsole(IAudioPlaylist audioPlaylist)
		{
			AudioPlaylist = audioPlaylist;
		}

		public IAudioPlaylist AudioPlaylist { get; }

		public void Invoke(object sender, EventArgs args)
		{
			lock (sender)
			{
				var audio = AudioPlaylist.CurrentAudio;

				Console.WriteLine($"Playing {audio.Title} - {audio.Artist} - {audio.Id}");
			}
		}
	}
}
