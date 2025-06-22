using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 110 * 1024 * 1024;
});


services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;
});
services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["application/grpc"]);

});
var app = builder.Build();

app.UseResponseCompression();
app.UseGrpcWeb();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapGrpcService<FileUploadService>();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
