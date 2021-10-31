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
		private readonly IAudioPlayer _audioPlayer;
		private readonly IAudioRepository _audioRepository;
		private readonly LinkedList<AudioDTO> _audios;
		private LinkedListNode<AudioDTO> _currentAudioNode;

		public AudioPlaylist(IAudioPlayer audioPlayer, IAudioRepository audioRepository)
		{
			_audioPlayer = audioPlayer;
			_audioRepository = audioRepository;
			_audios = _audioRepository.GetAllAudios().Result;
			_currentAudioNode = null;
		}

		private LinkedListNode<AudioDTO> NextAudioNode => _currentAudioNode?.Next ?? _audios.First;
		private LinkedListNode<AudioDTO> PreviousAudioNode => _currentAudioNode?.Previous ?? _audios.Last;

		public AudioDTO CurrentAudio => _currentAudioNode?.Value;
		public AudioDTO NextAudio => NextAudioNode.Value;
		public AudioDTO PreviousAudio => PreviousAudioNode.Value;

		public PlayerState CurrentState
			=> _audioPlayer.CurrentState;

		public Task Pause()
			=> _audioPlayer.Pause();

		public Task UnPause()
			=> _audioPlayer.Unpause();

		public Task PlayNext(Action<(long, long )> progress)
			=> PlayAudio(NextAudioNode, progress);

		public Task PlayPrevious(Action<(long, long)> progress)
			=> PlayAudio(PreviousAudioNode, progress);

		private async Task PlayAudio(LinkedListNode<AudioDTO> audioNode, Action<(long, long)> onProgress)
		{
			_currentAudioNode = audioNode;
			var progress = new Progress<(long, long)>(onProgress);
			using var stream = await _audioRepository.GetAudioStream(_currentAudioNode.Value, progress);
			await _audioPlayer.PlayAudio(stream);
		}
	}
}
