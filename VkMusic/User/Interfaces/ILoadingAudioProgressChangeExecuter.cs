using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Domain.Interfaces;

namespace VkMusic.User.Interfaces
{
	interface ILoadingAudioProgressChangeExecuter
	{
		public void Invoke(object sender, (long bitesRecived, long totaBitesToRecive) e);
	}
}
