using MusicFileOrganizer.Services;

using System;

namespace MusicFileOrganizer.Helper
{
    [Obsolete]
    class FileLoadPipeLine
    {
        FileMetaRepository fileMetaRepository;
        DirectoryManager directorymanager;
        /*

        // 1) 파일 로딩 (동기→비동기로 감싼 예)
        Func<string, Task<IEnumerable<string>>> loadFilesAsync = async sourcePath =>
        {
            return await Task.Run(() => FileUtil.LoadFileNames(sourcePath));
        };

        // 2) 파일명 등록
                Func<IEnumerable<string>, Task> addAllAsync =
                    async (registry, files) =>
                    {
                        // 동기 코드를 Task.Run 으로 분리해도 되고, 바로 실행해도 무방
                        return await Task.Run(() => registry.AddAll(files));
                    };
        
        // 3) FileMeta 생성 (예시)
        Func<string, IAsyncEnumerable<string>, Task<IAsyncEnumerable<FileMeta>>> createMetaAsync =
            async (srcPath, names) =>
            {
                return await FileMetaFactory.CreateBulkAsync(srcPath, names);
            };

        // 4) FileMeta 등록
        Func<FileMetaRepository, IAsyncEnumerable<FileMeta>, Task> saveMetaAsync =
            async (repo, metas) =>
            {
                await foreach (var m in metas)
                    repo.Add(m);
            };
        */
    }
}
