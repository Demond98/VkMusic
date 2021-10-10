using System;

namespace VkMusic.Domain.Core
{
	public class AudioDTO
	{
		public string Id { get; set; }
		public string Artist { get; set; }
		public string Title { get; set; }
		public Uri Url { get; set; }
		public long DurationInSeconds { get; set; }
	}
}
