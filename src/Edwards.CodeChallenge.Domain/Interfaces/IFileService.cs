using Edwards.CodeChallenge.Domain.Models;

namespace Edwards.CodeChallenge.Domain.Interfaces;

    public interface IFileService
{
    FileServiceResult DumpDataToDisk<T>(T data);
}
