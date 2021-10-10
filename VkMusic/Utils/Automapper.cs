using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Domain.Core;
using VkNet.Model.Attachments;

namespace VkMusic.Infrastructure
{
	public static class Automapper
	{
		private static MapperConfiguration _config;

		public static IMapper GetMapper()
		{
			if (_config == null)
				throw new Exception($"{nameof(_config)} is null");

			return _config.CreateMapper();
		}

		public static void Initilize()
		{
			_config = new MapperConfiguration(a =>
				a.CreateMap<Audio, AudioDTO>()
				.ForMember(a => a.Id, a => a.MapFrom(m => $"{m.OwnerId}_{m.Id}"))
				.ForMember(a => a.DurationInSeconds, a => a.MapFrom(m => m.Duration))
			);
		}
	}
}
