using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NLog;
using src.DriverRatings.Server.Infrastructure.Commands;
using src.DriverRatings.Server.Infrastructure.Commands.Identity;
using src.DriverRatings.Server.Infrastructure.DTO;
using src.DriverRatings.Server.Infrastructure.Extensions;
using src.DriverRatings.Server.Infrastructure.Queries;

namespace src.DriverRatings.Server.Api.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AccountController : ApiControllerBase
  {
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IMemoryCache _memoryCache;

    public AccountController(
      IMemoryCache memoryCache,
      ICommandDispatcher commandDispatcher,
      IQueryDispatcher queryDispatcher)
      : base(commandDispatcher, queryDispatcher)
      => this._memoryCache = memoryCache;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] CreateUser command)
    {
      Logger.Info($"Call register user api (email: {command.Email}, username: {command.Username}, role: {command.Role}).");
      var userDto = await this.DispatchCommandAsync<CreateUser, UserDto>(command);
      return Created($"users/{userDto.Email}", new object());
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] Login command)
    {
      Logger.Info($"Call login user api. User name {command.Username}");
      command.CacheId = Guid.NewGuid();
      await this.DispatchCommandAsync(command);
      var jwt = this._memoryCache.GetJwt(command.CacheId);

      return Ok(jwt);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetAuth()
      => Content($"You are authorized!. Hello {this.User.Identity.Name}!");
  }
}