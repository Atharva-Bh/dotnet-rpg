using dotnet_rpg.Dtos.Weapon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
            
        }

        [HttpPost]

        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon)
        {
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
        
    }
}