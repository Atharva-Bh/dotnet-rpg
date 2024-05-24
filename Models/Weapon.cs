namespace dotnet_rpg.Models
{
    public class Weapon
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        public Character? Character{ get; set; }
        public int CharacterID { get; set; }
    }
}