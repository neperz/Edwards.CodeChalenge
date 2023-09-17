using Edwards.CodeChallenge.Domain.Interfaces;
using Edwards.CodeChallenge.Domain.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;

namespace  Edwards.CodeChallenge.Infra
{
    public class FileService : IFileService
    {
        private readonly FileConfig _fileConfig;

        public FileService(IOptions<FileConfig> fileConfig)
        {
            _fileConfig = fileConfig.Value;
        }

        public FileServiceResult DumpDataToDisk<T>(T data)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_fileConfig.FilePath))
                {
                    throw new IOException("Error writing data to file.");
                }
                var json = JsonConvert.SerializeObject(data);
                File.WriteAllText(_fileConfig.FilePath, json);

                return new FileServiceResult
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new FileServiceResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
