using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;

namespace VkMusic.User.Interfaces
{
	public interface IAudioChangeExecuter
	{
		public void Invoke(AudioDTO audioInfo);
	}
}
