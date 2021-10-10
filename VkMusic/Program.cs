using AutoMapper;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Infrastructure;
using System.Text.Json.Serialization;
using System.Text.Json;
using VkMusic.User.Interfaces;
using System.Reflection;
using Ninject;

namespace VkMusic
{
	public static partial class Program
	{
		static void Main(string[] args)
		{
			var container = InitilizeApplication();
			StartupApplication(container);
		}

		private static StandardKernel InitilizeApplication()
		{
			Automapper.Initilize();

			var config = ConfigReader<Config>.ReadFromFile("config.json");
			var container = new StandardKernel(new VkBinding(config));
			container.Load(Assembly.GetExecutingAssembly());

			return container;
		}

		private static void StartupApplication(StandardKernel container)
		{
			var audioPlaylist = container.Get<IAudioPlaylist>();
			var audioProgressChangeExecuter = container.Get<ILoadingAudioProgressChangeExecuter>();
			var audioChangeExecuter = container.Get<IAudioChangeExecuter>();
			var userInterface = container.Get<IUserInterface>();
			
			audioPlaylist.PlayNext();
			userInterface.Invoke();
		}
	}
}