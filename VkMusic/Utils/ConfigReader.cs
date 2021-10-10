using System;
using System.IO;
using System.Text.Json;

namespace VkMusic.Infrastructure
{
	public static class ConfigReader<T>
	{
		public static T ReadFromFile(string path)
		{
			if (!File.Exists(path))
				throw new Exception($"Config file '{path}' not found");

			var json = File.ReadAllText(path);
			return JsonSerializer.Deserialize<T>(json);
		}
	}
}
