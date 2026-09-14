using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JO.Service.Services.Contracts;

namespace JO.BlazorDemoApp.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class Approval : ControllerBase
    {
        private readonly IApprovalService _approvalService;

        public Approval(IApprovalService approvalService)
        {
            _approvalService = approvalService;
        }

        [HttpGet("ApproveViaEmail")]
        public async Task<IActionResult> ApproveViaEmail(
            [FromQuery] int jobOfferId,
            [FromQuery] int workFlowId,
            [FromQuery] int roleId,
            [FromQuery] int actionId)
        {
            await _approvalService.ApproveViaEmail(jobOfferId, workFlowId, roleId, actionId);
            return Ok();
        }

        [HttpGet("SendbackViaEmail")]
        public async Task<IActionResult> SendbackViaEmail(
            [FromQuery] int jobOfferId,
            [FromQuery] int roleId)
        {
            await _approvalService.SendbackViaEmail(jobOfferId, roleId);
            return Ok();
        }
    }
}
