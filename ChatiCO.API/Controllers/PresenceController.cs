using ChatiCO.Application.Interfaces;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatiCO.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PresenceController : ControllerBase
    {
        private readonly IPresenceService _presenceService;
        private readonly IUserRegistrationRepository _userRegistrationRepository;

        public PresenceController(IPresenceService presenceService, IUserRegistrationRepository userRegistrationRepository)
        {
            _presenceService = presenceService;
            _userRegistrationRepository = userRegistrationRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(string userId)
        {
            var online = await _presenceService.IsUserOnlineAsync(userId);
            var connections = await _presenceService.GetConnectionCountAsync(userId);

            var user = await _userRegistrationRepository.GetUserByIdAsync(int.TryParse(userId, out var id) ? id : 0);

            return Ok(new
            {
                UserId = userId,
                IsOnline = online,
                ConnectionCount = connections,
                LastSeen = user?.LastSeen
            });
        }
    }
}
