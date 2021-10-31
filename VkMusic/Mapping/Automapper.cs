using AutoMapper;
using System;
using VkMusic.Domain.Core;
using VkMusic.Mapping;
using VkNet.Model.Attachments;

namespace VkMusic.Infrastructure
{
	public static class Automapper
	{
		private static MapperConfiguration _config;

		public static IMapper GetMapper()
		{
			if (_config == null)
				throw new NullReferenceException($"{nameof(_config)} is null");

			return _config.CreateMapper();
		}

		public static void Initilize()
		{
			var profile = new AudioProfile();
			_config = new MapperConfiguration(e => e.AddProfile(profile));
		}
	}
}
