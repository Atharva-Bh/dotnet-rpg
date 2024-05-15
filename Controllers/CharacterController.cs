using Microsoft.AspNetCore.Mvc;
namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService= characterService;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<Character>>> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Character>>> GetSingle(int id)
        {
            return Ok(await _characterService.GetCharacterByID(id));
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> Add(Character newCharacter)
        {
            return Ok(await _characterService.AddCharacter(newCharacter));
        }
    }
}