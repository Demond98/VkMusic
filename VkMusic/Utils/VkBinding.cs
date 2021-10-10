using System;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Interfaces;
using VkMusic.Infrastructure;
using System.IO;
using VkMusic.User.Interfaces;
using VkMusic.User.Infrastructure;
using Ninject.Modules;
using VkMusic.Application.Infrastructure;

namespace VkMusic
{
	public static partial class Program
	{
		public class VkBinding : NinjectModule
		{
			private readonly Config config;

			public VkBinding(Config config)
			{
				this.config = config;
			}

			public override void Load()
			{
				Bind<IAudioRepository>().To<AudioRepository>()
					.InSingletonScope()
					.WithConstructorArgument(config.OwnerId)
					.WithConstructorArgument(config.VkToken);

				Bind<IAudioPlayer>().To<AudioPlayerNAudio>()
					.InSingletonScope();

				Bind<IAudioPlaylist>().To<AudioPlaylist>()
					.InSingletonScope();

				Bind<ILoadingAudioProgressChangeExecuter>().To<LoadingAudioProgressChangeWriteToConsole>()
					.OnActivation(a => a.AudioRepository.LoadingAudioProgressChanged += a.Invoke);

				Bind<IAudioChangeExecuter>().To<AudioChangeWriteToConsole>()
					.OnActivation(a => a.AudioPlaylist.AudioPlayer.SongChanged += a.Invoke);

				Bind<IUserInterface>().To<UserInterface>();
			}
		}
	}
}