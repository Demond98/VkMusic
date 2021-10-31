using VkMusic.Infrastructure;
using System.Reflection;
using Ninject;

namespace VkMusic
{
	public static class Program
	{
		private const string Config = "config.json";

		static void Main(string[] args)
		{
			var container = InitilizeApplication();
			StartupApplication(container);
		}

		private static StandardKernel InitilizeApplication()
		{
			Automapper.Initilize();

			var config = ConfigReader<Config>.ReadJsonFromFile(Config);
			var vkBinding = new VkBinding(config);
			var container = new StandardKernel(vkBinding);
			var currentAssembly = Assembly.GetExecutingAssembly();
			container.Load(currentAssembly);

			return container;
		}

		private static void StartupApplication(StandardKernel container)
		{
			var userInterface = container.Get<IUserInterface>();
			
			userInterface.Invoke();
		}
	}
}