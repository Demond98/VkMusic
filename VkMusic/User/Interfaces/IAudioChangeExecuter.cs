using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;

namespace VkMusic.User.Interfaces
{
	interface IAudioChangeExecuter
	{
		public IAudioPlaylist AudioPlaylist { get; }

		public void Invoke(object sender, EventArgs args);
	}
}
