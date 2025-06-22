using System.Text;

namespace Client.Services;

public class FileGenerationService
{
    public async Task GenerateTextFileAsync(int sizeInMB, string filePath)
    {
        long targetSizeBytes = sizeInMB * 1024L * 1024L;
        string line = "Dies ist eine Testzeile für die Datei.\n";
        byte[] lineBytes = Encoding.UTF8.GetBytes(line);

        await using var fs = new FileStream(
            filePath, FileMode.Create, FileAccess.Write,
            FileShare.None, 4096, useAsync: true);

        while (fs.Length + lineBytes.Length <= targetSizeBytes)
        {
            await fs.WriteAsync(lineBytes, 0, lineBytes.Length);
        }

        Console.WriteLine($"Textdatei '{filePath}' mit ca. {sizeInMB} MB erstellt.");
    }
}
