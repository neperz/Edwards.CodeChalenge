using Edwards.CodeChallenge.Domain.Interfaces;
using Edwards.CodeChallenge.Domain.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;

namespace Edwards.CodeChallenge.Infra;

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
            string jsonData = JsonSerializer.Serialize(data);

            File.WriteAllText(_fileConfig.FilePath, jsonData);

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

