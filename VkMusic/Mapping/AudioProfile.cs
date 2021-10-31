using AutoMapper;
using VkMusic.Domain.Core;
using VkNet.Model.Attachments;

namespace VkMusic.Mapping
{
	public class AudioProfile : Profile
	{
		public AudioProfile()
		{
			CreateMap<Audio, AudioDTO>()
				.ForMember(a => a.Id, a => a.MapFrom(m => $"{m.OwnerId}_{m.Id}"))
				.ForMember(a => a.DurationInSeconds, a => a.MapFrom(m => m.Duration));
		}
	}
}
