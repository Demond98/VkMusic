using System;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Interfaces;
using VkMusic.Infrastructure;
using System.IO;
using VkMusic.User.Interfaces;
using VkMusic.User.Infrastructure;
using Ninject.Modules;
using VkMusic.Application.Infrastructure;

namespace VkMusic.Utils
{
	public class VkBinding : NinjectModule
	{
		private readonly Config _config;

		public VkBinding(Config config)
		{
			_config = config;
		}

		public override void Load()
		{
			Bind<IAudioRepository>().To<VkAudioRepository>()
				.InSingletonScope()
				.WithConstructorArgument(_config.OwnerId)
				.WithConstructorArgument(_config.VkToken);

			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				Bind<IAudioPlayer>().To<AudioPlayerNAudio>()
					.InSingletonScope();
			}
			else
				throw new PlatformNotSupportedException("Current OS not supported");

			Bind<IAudioPlaylist>().To<AudioPlaylist>()
				.InSingletonScope();

			Bind<ILoadingAudioProgressChangeExecuter>().To<LoadingAudioProgressChangeWriteToConsole>();
			Bind<IAudioChangeExecuter>().To<AudioChangeWriteToConsole>();
			Bind<IUserInterface>().To<UserInterface>();
		}
	}
}