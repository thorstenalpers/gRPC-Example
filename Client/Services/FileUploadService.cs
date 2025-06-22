using Google.Protobuf;
using Grpc.Core;
using System.Net.Http.Headers;

namespace Client.Services;

public class FileUploadService(FileUpload.FileUploadClient client, string serverAddress)
{
    private readonly string _serverAddress = serverAddress;
    const int _chunkSize = 80 * 1024; // 80 kb
    private readonly FileUpload.FileUploadClient _client = client;

    public async Task UploadFileGrpc(string file)
    {
        var headers = new Metadata
    {
        { "grpc-encoding", "gzip" }
    };

        using var call = _client.Upload(headers);

        var buffer = new byte[_chunkSize];
        int bytesRead;
        int chunkIndex = 0;

        using var stream = File.OpenRead(file);
        while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
        {
            var chunk = new FileChunk
            {
                FileName = Path.GetFileName(file),
                ChunkNumber = chunkIndex++,
                Data = ByteString.CopyFrom(buffer, 0, bytesRead)
            };

            await call.RequestStream.WriteAsync(chunk);
        }

        await call.RequestStream.CompleteAsync();

        var response = await call.ResponseAsync;
    }

    public async Task UploadFileHttp(string file)
    {
        using var httpClient = new HttpClient();
        using var form = new MultipartFormDataContent();
        using var fileStream = File.OpenRead(file);

        form.Add(new StreamContent(fileStream)
        {
            Headers =
        {
            ContentLength = fileStream.Length,
            ContentType = new MediaTypeHeaderValue("application/octet-stream")
        }
        }, "file", Path.GetFileName(file));

        var response = await httpClient.PostAsync($"{_serverAddress}/api/upload", form);
        var content = await response.Content.ReadAsStringAsync();
    }

    public async Task<bool> PingGrpcAsync()
    {
        try
        {
            var reply = await _client.PingAsync(new PingRequest());
            return reply.Status == "OK";
        }
        catch
        {
            return false;
        }
    }
}