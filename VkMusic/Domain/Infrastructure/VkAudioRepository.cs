using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VkMusic.Domain.Core;
using VkMusic.Domain.Interfaces;
using VkMusic.Mapping;
using VkNet;
using VkNet.Model.Attachments;

namespace VkMusic.Infrastructure
{
	public sealed class VkAudioRepository : IAudioRepository
	{
		public const string FilesDirectoryName = "audio";

		private readonly int _ownerId;
		private readonly string _token;

		public VkAudioRepository(int ownerId, string token)
		{
			_ownerId = ownerId;
			_token = token;
		}

		public async Task<LinkedList<AudioDTO>> GetAllAudiosAsync()
		{
			var vk = new VkApi();

			Console.Write("Authorize...");
			await vk.AuthorizeAsync(new VkNet.Model.ApiAuthParams()
			{
				AccessToken = _token,
			});
			Console.WriteLine("complite");

			Console.Write("Downloading ids...");
			var audios = await vk.Audio.GetAsync(new VkNet.Model.RequestParams.AudioGetParams()
			{
				OwnerId = _ownerId,
				Count = 6000,
			});
			Console.WriteLine("complite");

			var audiosIds = audios.Select(a => $"{_ownerId}_{a.Id}");

			Console.Write("Downloading audios info...");
			var audiosInfo = await vk.Audio.GetByIdAsync(audiosIds);
			Console.WriteLine("complite");

			var audiosDTO = Automapper.GetMapper()
				.Map<IEnumerable<Audio>, IEnumerable<AudioDTO>>(audiosInfo);

			return new LinkedList<AudioDTO>(audiosDTO);
		}

		public async Task<Stream> GetAudioStreamAsync(AudioDTO audioInfo, IProgress<(long BytesReceived, long TotalBytesToReceive)> progress)
		{
			Directory.CreateDirectory(FilesDirectoryName);

			var filePath = Path.Combine(FilesDirectoryName, $"{audioInfo.Id}.mp3");

			if (File.Exists(filePath))
				return File.OpenRead(filePath);
			
			using var webClient = new WebClient();
			webClient.DownloadProgressChanged += (s, e) => progress.Report((e.BytesReceived, e.TotalBytesToReceive));

			var data = await webClient.DownloadDataTaskAsync(audioInfo.Url);

			await File.WriteAllBytesAsync(filePath, data);

			return new MemoryStream(data);
		}

		public void Dispose()
		{

		}
	}
}