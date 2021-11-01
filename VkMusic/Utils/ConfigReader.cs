using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace VkMusic.Utils
{
	public static class ConfigReader<T>
	{
		public static T ReadJsonFromFile(string path)
		{
			if (!File.Exists(path))
				throw new Exception($"Config file '{path}' not found");

			var json = File.ReadAllText(path);

			return JsonSerializer.Deserialize<T>(json);
		}
	}
}
