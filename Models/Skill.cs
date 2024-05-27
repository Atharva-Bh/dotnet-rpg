namespace dotnet_rpg.Models
{
    public class Skill
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Damage { get; set; }
        public List<Character>? Characters { get; set; }
    }
}