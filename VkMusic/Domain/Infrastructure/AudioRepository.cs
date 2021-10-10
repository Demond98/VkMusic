using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;
using VkNet;
using VkNet.Model.Attachments;

namespace VkMusic.Infrastructure
{
	public sealed class AudioRepository : IAudioRepository
	{
		public static string FilesDirectoryName = "audio";

		public event EventHandler<(long bitesRecived, long totaBitesToRecive)> LoadingAudioProgressChanged;

		private int _ownerId;
		private string _token;

		public AudioRepository(int ownerId, string token)
		{
			_ownerId = ownerId;
			_token = token;
		}

		public LinkedList<AudioDTO> GetAllAudios()
		{
			var vk = new VkApi();

			var authParams = new VkNet.Model.ApiAuthParams()
			{
				AccessToken = _token,
			};

			vk.Authorize(authParams);

			var getParams = new VkNet.Model.RequestParams.AudioGetParams()
			{
				OwnerId = _ownerId,
				Count = 6000,
			};

			var audiosIds = vk.Audio
				.Get(getParams)
				.Select(a => $"{_ownerId}_{a.Id}");

			var audios = vk.Audio.GetById(audiosIds);

			var audiosDTO = Automapper.GetMapper()
				.Map<IEnumerable<Audio>, IEnumerable<AudioDTO>>(audios);

			return new LinkedList<AudioDTO>(audiosDTO);
		}

		public Stream GetAudioStream(AudioDTO audioInfo)
		{
			if (!Directory.Exists(FilesDirectoryName))
				Directory.CreateDirectory(FilesDirectoryName);

			var filePath = Path.Combine(FilesDirectoryName, $"{audioInfo.Id}.mp3");

			if (!File.Exists(filePath))
			{
				using var webClient = new WebClient();
				webClient.DownloadProgressChanged += DownloadProgressChangedHandler;
				webClient.DownloadFileTaskAsync(audioInfo.Url, filePath).Wait();
			}

			return File.OpenRead(filePath);
		}

		private void DownloadProgressChangedHandler(object sender, DownloadProgressChangedEventArgs e)
		{
			LoadingAudioProgressChanged?.Invoke(this, (e.BytesReceived, e.TotalBytesToReceive));
		}

		public void Dispose()
		{

		}
	}
}
