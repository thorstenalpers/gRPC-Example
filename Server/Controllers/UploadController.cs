using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFilesHttp");
        Directory.CreateDirectory(path);

        var filePath = Path.Combine(path, file.FileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return Ok(new { file = file.FileName, savedTo = filePath });
    }
}
