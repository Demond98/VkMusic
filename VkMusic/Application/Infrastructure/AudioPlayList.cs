using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;

namespace VkMusic.Application.Infrastructure
{
	public class AudioPlaylist : IAudioPlaylist
	{
		private readonly object _syncObject;
		private readonly IAudioRepository _audioRepository;
		private readonly LinkedList<AudioDTO> _audios;
		private LinkedListNode<AudioDTO> _currentAudioNode;

		public AudioPlaylist(IAudioRepository audioRepository)
		{
			_audioRepository = audioRepository;
			_audios = _audioRepository.GetAllAudios().Result;
			_currentAudioNode = null;
			_syncObject = new();
		}

		public AudioDTO Next()
		{
			lock (_syncObject)
			{
				return (_currentAudioNode = _currentAudioNode?.Next ?? _audios.First).Value;
			}
		}

		public AudioDTO Previous()
		{
			lock (_syncObject)
			{
				return (_currentAudioNode = _currentAudioNode?.Previous ?? _audios.Last).Value;
			}
		}

		public AudioDTO CurrentAudio
		{
			get
			{
				lock (_syncObject)
				{
					return _currentAudioNode?.Value;
				}
			}
		}
	}
}
