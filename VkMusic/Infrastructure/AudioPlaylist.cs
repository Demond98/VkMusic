using System;
using System.Collections.Generic;
using System.Text;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;

namespace VkMusic.Infrastructure
{
	public class AudioPlaylist : IAudioPlaylist
	{
		private LinkedList<AudioDTO> _audios;
		private LinkedListNode<AudioDTO> _currentAudioNode;

		public IAudioPlayer AudioPlayer { get; }
		public IAudioRepository AudioRepository { get; }
		public AudioDTO CurrentAudio => _currentAudioNode.Value;

		public AudioPlaylist(IAudioPlayer audioPlayer, IAudioRepository audioRepository)
		{
			AudioPlayer = audioPlayer;
			AudioRepository = audioRepository;
			_audios = AudioRepository.GetAllAudiosInfo();
			_currentAudioNode = null;
			AudioPlayer.AudioPlayingStopped += PlayNext;
		}

		public void PlayNext()
			=> PlayAudio(_currentAudioNode?.Next ?? _audios.First);

		public void PlayPrevious()
			=> PlayAudio(_currentAudioNode?.Previous ?? _audios.Last);

		private void PlayNext(object sender, EventArgs e)
		{
			lock (sender)
			{
				PlayNext();
			}
		}

		private void PlayAudio(LinkedListNode<AudioDTO> audioNode)
		{
			_currentAudioNode = audioNode;
			var stream = AudioRepository.GetAudioStream(_currentAudioNode.Value);
			AudioPlayer.PlayAudio(stream);
		}
	}
}
