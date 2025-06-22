using Client.Services;
using System.Diagnostics;

string serverAddress = "https://localhost:7275";
string filePath = "testfile.txt";
int tries = 100;
int fileSizeInMB = 50;

var httpTimes = new List<long>();
var grpcTimes = new List<long>();
var stopWatch = new Stopwatch();

var channel = Grpc.Net.Client.GrpcChannel.ForAddress(serverAddress);
var client = new FileUpload.FileUploadClient(channel);

var fileUploadService = new FileUploadService(client, serverAddress);
var fileGenerationService = new FileGenerationService();

// make a ping to ensure the server is reachable and init connection
var pingResult = await fileUploadService.PingGrpcAsync();

await fileGenerationService.GenerateTextFileAsync(fileSizeInMB, filePath);

Console.WriteLine($"Running {tries} upload tests with a {fileSizeInMB} MB file...\n");

// gRPC upload times
for (int i = 0; i < tries; i++)
{
    stopWatch.Restart();
    await fileUploadService.UploadFileGrpc(filePath);
    stopWatch.Stop();
    grpcTimes.Add(stopWatch.ElapsedMilliseconds);
}

// HTTP upload times
for (int i = 0; i < tries; i++)
{
    stopWatch.Restart();
    await fileUploadService.UploadFileHttp(filePath);
    stopWatch.Stop();
    httpTimes.Add(stopWatch.ElapsedMilliseconds);
}

void PrintTimes(string label, List<long> times)
{
    Console.WriteLine($"{label} Upload Times (ms):");
    Console.WriteLine("  " + string.Join(", ", times));
    Console.WriteLine($"{label} Upload Average: {times.Average():F2} ms\n");
}

PrintTimes("gRPC", grpcTimes);
PrintTimes("HTTP", httpTimes);

double httpAvg = httpTimes.Average();
double grpcAvg = grpcTimes.Average();
double speedFactor = httpAvg / grpcAvg;

Console.WriteLine($"Summary:");
Console.WriteLine($"  Number of tries: {tries}");
Console.WriteLine($"  File size: {fileSizeInMB} MB");
Console.WriteLine($"  HTTP Average Upload Time: {httpAvg:F2} ms");
Console.WriteLine($"  gRPC Average Upload Time: {grpcAvg:F2} ms");
Console.WriteLine($"  gRPC is {speedFactor:F2}x faster than HTTP");
