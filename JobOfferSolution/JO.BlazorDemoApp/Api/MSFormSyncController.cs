using System.ComponentModel.DataAnnotations;
using JO.Service.Constants;
using JO.Service.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JO.BlazorDemoApp.Api
{
    [ApiController]
    [Route("api/MSFormSync")]
    //[Authorize(Roles = JOUserRole.Admin)]
    public class MSFormSyncController : ControllerBase
    {
        private readonly IMSFormSyncService _syncService;
        private readonly ILogger<MSFormSyncController> _logger;

        public MSFormSyncController(IMSFormSyncService syncService, ILogger<MSFormSyncController> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        //[HttpPost]
        [HttpGet]
        [ProducesResponseType(typeof(MSFormSyncResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MSFormSyncResponse>> SyncMSFormsAsync()
        {
            try
            {
                const int userId = 0;
                var responses = await _syncService.SaveCandidateResponseRawData(userId);

                return Ok(new MSFormSyncResponse(
                    responses.Count,
                    responses.Count(response => response.InvalidResponse == true)));
            }
            catch (ValidationException ex)
            {
                return Problem(statusCode: StatusCodes.Status400BadRequest,
                    title: "Workbook needs attention",
                    detail: $"{ex.Message} Correct the source workbook, then try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to sync MS-Forms responses.");
                return Problem(statusCode: StatusCodes.Status500InternalServerError,
                    title: "Sync failed",
                    detail: "The responses could not be synced. Check the workbook format and database connection, then try again.");
            }
        }
    }

    public sealed record MSFormSyncResponse(int TotalCount, int InvalidResponseCount);
}
