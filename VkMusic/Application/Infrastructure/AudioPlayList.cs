using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkMusic.Application.Interfaces;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;

namespace VkMusic.Application.Infrastructure
{
	public class AudioPlaylist : IAudioPlaylist
	{
		private IAudioPlayer _audioPlayer;
		private IAudioRepository _audioRepository;
		private readonly LinkedList<AudioDTO> _audios;
		private LinkedListNode<AudioDTO> _currentAudioNode;

		public AudioDTO CurrentAudio => _currentAudioNode.Value;

		public AudioPlaylist(IAudioPlayer audioPlayer, IAudioRepository audioRepository)
		{
			_audioPlayer = audioPlayer;
			_audioRepository = audioRepository;
			_audios = _audioRepository.GetAllAudios().Result;
			_currentAudioNode = null;
		}

		public PlayerState CurrentState
			=> _audioPlayer.CurrentState;

		public Task Pause()
			=> _audioPlayer.Pause();

		public Task Unpase()
			=> _audioPlayer.Unpause();

		public Task PlayNext(Action<(long, long )> progress)
			=> PlayAudio(_currentAudioNode?.Next ?? _audios.First, progress);

		public Task PlayPrevious(Action<(long, long)> progress)
			=> PlayAudio(_currentAudioNode?.Previous ?? _audios.Last, progress);

		private async Task PlayAudio(LinkedListNode<AudioDTO> audioNode, Action<(long, long)> onProgress)
		{
			_currentAudioNode = audioNode;
			var progress = new Progress<(long, long)>(onProgress);
			var stream = await _audioRepository.GetAudioStream(_currentAudioNode.Value, progress);
			await _audioPlayer.PlayAudio(stream);
		}
	}
}
