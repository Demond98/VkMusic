using VkMusic.Infrastructure;
using VkMusic.Mapping;
using Xunit;

namespace VkMusicTests
{
	public class MapperTests
	{
		[Fact]
		public void Is_automapper_config_valid()
		{
			Automapper.Initilize();
			Automapper.Config.AssertConfigurationIsValid();
		}
	}
}
