

using System.Security.Claims;

namespace dotnet_rpg.Service.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context; 
        private readonly IHttpContextAccessor _httpContextAccessor; 
        public CharacterService(IMapper mapper , DataContext context , IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
           _context = context;
           _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characterDto = _mapper.Map<Character>(newCharacter);
            characterDto.User = await _context.Users.FirstOrDefaultAsync(u => u.ID == GetUserID());
            _context.Characters.Add(characterDto);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.Characters.Where(c => c.User!.ID == GetUserID()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }
        private int GetUserID() => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try{
            var character = await _context.Characters.FirstAsync(c => c.ID == id && c.User!.ID == GetUserID());
            if(character is null)
            {
                throw new Exception($"Character with ID {id} , to be deleted , not found!");
            }
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(_context.Characters.Where(c => c.User!.ID == GetUserID()));
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Where(c => c.User!.ID == GetUserID()).ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(dbCharacters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterByID(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{
            var dbCharacter= await _context.Characters.FirstOrDefaultAsync(c => c.ID == id && c.User!.ID == GetUserID());
            if(dbCharacter is null)
            {
                throw new Exception($"Character with ID {id} not found!");
            }
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{
            var character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(c => c.ID == updatedCharacter.ID);
            if(character is null || character.User!.ID != GetUserID())
            {
                throw new Exception($"Character with ID {updatedCharacter.ID} not found!");
            }
            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.HitPoints;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Defense = updatedCharacter.Defense;
            character.Class = updatedCharacter.Class;
            await _context.SaveChangesAsync();
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}