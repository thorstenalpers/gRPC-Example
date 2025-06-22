using Grpc.Core;
using GrpcFileTransfer;

namespace Server.Services
{
    public class FileUploadService(IWebHostEnvironment env) : FileUpload.FileUploadBase
    {
        private readonly IWebHostEnvironment _env = env;
        private const int BufferSize = 4 * 1024; // 4 KB

        public override async Task<UploadStatus> Upload(IAsyncStreamReader<FileChunk> requestStream, ServerCallContext context)
        {
            var uploadsPath = Path.Combine(_env.ContentRootPath, "UploadedFiles");
            Directory.CreateDirectory(uploadsPath);

            string? filePath = null;
            FileStream? fileStream = null;

            try
            {
                await foreach (var chunk in requestStream.ReadAllAsync())
                {
                    if (fileStream == null)
                    {
                        filePath = Path.Combine(uploadsPath, chunk.FileName);

                        fileStream = new FileStream(
                            filePath,
                            FileMode.Create,
                            FileAccess.Write,
                            FileShare.None,
                            BufferSize,
                            useAsync: true);
                    }
                    await fileStream.WriteAsync(chunk.Data.ToByteArray());
                }

                if (fileStream != null)
                {
                    await fileStream.FlushAsync();
                    await fileStream.DisposeAsync();
                    return new UploadStatus { Success = true, Message = $"Datei erfolgreich gespeichert: {filePath}" };
                }
                else
                {
                    return new UploadStatus { Success = false, Message = "Keine Daten empfangen." };
                }
            }
            catch (Exception ex)
            {
                if (fileStream != null)
                    await fileStream.DisposeAsync();

                return new UploadStatus { Success = false, Message = $"Fehler: {ex.Message}" };
            }
        }
    }
}
