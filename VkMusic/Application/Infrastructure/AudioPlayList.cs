using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;

namespace VkMusic.Application.Infrastructure
{
	public class AudioPlayList : IAudioPlaylist
	{
		private readonly LinkedList<AudioDTO> _audios;
		private LinkedListNode<AudioDTO> _currentAudioNode;

		public IAudioPlayer AudioPlayer { get; }
		public IAudioRepository AudioRepository { get; }
		public AudioDTO CurrentAudio => _currentAudioNode.Value;

		public AudioPlayList(IAudioPlayer audioPlayer, IAudioRepository audioRepository)
		{
			AudioPlayer = audioPlayer;
			AudioRepository = audioRepository;
			_audios = AudioRepository.GetAllAudios();
			_currentAudioNode = null;
			AudioPlayer.AudioPlayingEnded += PlayNext;
		}

		public async Task PlayNext()
			=> await PlayAudio(_currentAudioNode?.Next ?? _audios.First);

		public async Task PlayPrevious()
			=> await PlayAudio(_currentAudioNode?.Previous ?? _audios.Last);

		public async Task HandlePause()
			=> await AudioPlayer.HandlePause();
		
		private void PlayNext(object sender, EventArgs e)
		{
			lock (sender)
			{
				PlayNext().Wait();
			}
		}

		private async Task PlayAudio(LinkedListNode<AudioDTO> audioNode)
		{
			_currentAudioNode = audioNode;
			var stream = AudioRepository.GetAudioStream(_currentAudioNode.Value);
			await AudioPlayer.PlayAudio(stream);
		}
	}
}
