namespace dotnet_rpg.Service.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
            new Character() , 
            new Character{
                ID = 1 , 
                Name = "Sam"
            }
        };
        private readonly IMapper _mapper;
        public CharacterService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characterDto = _mapper.Map<Character>(newCharacter);
            characterDto.ID = characters.Max(c => c.ID) + 1;
            characters.Add(characterDto);
            serviceResponse.Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try{
            var character = characters.First(c => c.ID == id);
            if(character is null)
            {
                throw new Exception($"Character with ID {id} , to be deleted , not found!");
            }
            characters.Remove(character);
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
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
            serviceResponse.Data = _mapper.Map<List<GetCharacterDto>>(characters);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterByID(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try{
            var character = characters.FirstOrDefault(c => c.ID == id);
            if(character is null)
            {
                throw new Exception($"Character with ID {id} not found!");
            }
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
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
            var character = characters.FirstOrDefault(c => c.ID == updatedCharacter.ID);
            if(character is null)
            {
                throw new Exception($"Character with ID {updatedCharacter.ID} not found!");
            }
            character.Name = updatedCharacter.Name;
            character.HitPoints = updatedCharacter.HitPoints;
            character.Strength = updatedCharacter.HitPoints;
            character.Intelligence = updatedCharacter.Intelligence;
            character.Defense = updatedCharacter.Defense;
            character.Class = updatedCharacter.Class;
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