namespace dotnet_rpg.Service.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<Character>>> GetAllCharacters();
        Task<ServiceResponse<Character>> GetCharacterByID(int id);
        Task<ServiceResponse<List<Character>>> AddCharacter(Character newCharacter);
    }
}