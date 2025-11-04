using Microsoft.Extensions.DependencyInjection;

using MusicFileOrganizer.DTO;
using MusicFileOrganizer.Utils;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicFileOrganizer.Services
{
    public class Organizer
    {
        private readonly FileMetaRepository fileMetaRepository;
        private readonly DirectoryManager directoryManager;

        private string dstPath = string.Empty;
        private bool stop = false;

        public Organizer(FileMetaRepository? repo = null, DirectoryManager? dirMgr = null)
        {
            fileMetaRepository = repo ?? App.Services.GetRequiredService<FileMetaRepository>()!;
            directoryManager = dirMgr ?? App.Services.GetRequiredService<DirectoryManager>()!;
        }
        public FileMetaRepository FileMetaRepository => this.fileMetaRepository;
        public DirectoryManager DirectoryManager => this.directoryManager;


        public string DstPath
        {
            get => directoryManager.DstPath!;
            set
            {
                dstPath = directoryManager.DstPath = value;
            }
        }

        private async Task<DirectoryInfo> CreateFolderAsync(FileMeta file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File metadata cannot be null.");
            }
            return await directoryManager.CreateFolderAsync(file);
        }

        public void SetSrcPath(string srcPath)
        {
            if (string.IsNullOrEmpty(srcPath))
            {
                throw new ArgumentException("Source path cannot be null or empty.", nameof(srcPath));
            }
            directoryManager.SrcPath = srcPath;
        }

        public void SetDestPath(string destPath)
        {
            if (string.IsNullOrEmpty(destPath))
            {
                throw new ArgumentException("Destination path cannot be null or empty.", nameof(destPath));
            }
            directoryManager.DstPath = destPath;
        }

        public async Task<IAsyncEnumerable<FileMeta>> CreateFileMetaAsync()
        {
            Stopwatch stopwatch = new Stopwatch();

            var files = await Task.Run(()
                => FileMetaFactory.CreateBulkAsync(directoryManager.SrcPath!, FileUtil.LoadFile(directoryManager.SrcPath!)));

            await fileMetaRepository.AddAll(files);

            return files;
        }

        public string CreateTreeView()
        {
            return StringUtil.CreateFolderTreePreview(fileMetaRepository.CreateArtistAlbumPair());
        }

        public string SourcePath()
        {
            return directoryManager.SrcPath!;
        }

        public async Task Start()
        {
            await foreach (FileMeta file in fileMetaRepository.GetAll().ToAsyncEnumerable())
            {
                if (stop)
                {
                    break;
                }
                DirectoryInfo directoryInfo = await directoryManager.CreateFolderAsync(file!);
                await FileUtil.CopyFileAsync(directoryInfo, file.Path!, file.Name!);
            }

        }
        public void Stop()
        {
            stop = true;
        }
    }
}

