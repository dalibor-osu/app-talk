using AppTalk.API.Managers;
using AppTalk.Core.Extensions;
using AppTalk.Models.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ControllerBase = AppTalk.Core.Base.ControllerBase;

namespace AppTalk.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ServerController(ServerManager serverManager, ServerMemberManager serverMemberManager) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAsync(
        [FromBody] ServerDto server)
    {
        var userId = User.GetUserId();
        var result = await serverManager.AddAsync(userId, server);
        return CreateActionResultResponse(result);
    }
    
    [HttpPost("join/{serverId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> JoinAsync(
        [FromRoute] Guid serverId)
    {
        var userId = User.GetUserId();
        var result = await serverMemberManager.JoinAsync(userId, serverId);
        return CreateActionResultResponse(result);
    }
}