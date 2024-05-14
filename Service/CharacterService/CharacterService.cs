
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
        public List<Character> AddCharacter(Character newCharacter)
        {
            characters.Add(newCharacter);
            return characters;
        }

        public List<Character> GetAllCharacters()
        {
             return characters;
        }

        public Character GetCharacterByID(int id)
        {
           return characters.FirstOrDefault(c => c.ID == id);
        }
    }
}