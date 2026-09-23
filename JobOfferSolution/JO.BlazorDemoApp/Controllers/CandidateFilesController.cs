using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JO.BlazorDemoApp.Controllers;

[Authorize]
[ApiController]
[Route("candidate-files")]
public class CandidateFilesController(ICandidateFileService fileService) : ControllerBase
{
    [HttpGet("{candidateId:int}/{fileId:int}/download")]
    public async Task<IActionResult> Download(int candidateId, int fileId)
    {
        var download = await fileService.GetDownload(candidateId, fileId);
        if (download is null) return NotFound();
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        return PhysicalFile(download.Value.Path, "application/octet-stream", download.Value.FileName);
    }
}
