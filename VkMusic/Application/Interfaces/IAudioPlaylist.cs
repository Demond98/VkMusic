using VkMusic.Domain.Core;

namespace VkMusic.Application.Interfaces
{
	public interface IAudioPlaylist
	{
		AudioDTO CurrentAudio { get; }
		AudioDTO Next();
		AudioDTO Previous();
	}
}