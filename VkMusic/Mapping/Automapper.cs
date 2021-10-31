using AutoMapper;
using System;
using VkMusic.Domain.Core;
using VkMusic.Mapping;
using VkNet.Model.Attachments;

namespace VkMusic.Infrastructure
{
	public static class Automapper
	{
		public static MapperConfiguration Config { get; private set; }

		public static IMapper GetMapper()
		{
			if (Config == null)
				throw new NullReferenceException($"{nameof(Config)} is null");

			return Config.CreateMapper();
		}

		public static void Initilize()
		{
			var profile = new AudioProfile();
			Config = new MapperConfiguration(e => e.AddProfile(profile));
		}
	}
}
