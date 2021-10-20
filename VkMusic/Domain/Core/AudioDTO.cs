using System;

namespace VkMusic.Domain.Core
{
	public class AudioDTO
	{
		public string Id { get; internal set; }
		public string Artist { get; internal set; }
		public string Title { get; internal set; }
		public Uri Url { get; internal set; }
		public long DurationInSeconds { get; internal set; }
	}
}
