global using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill{
                    ID = 1 , 
                    Name = "Regrowth" , 
                    Damage = 300
                } , 
                new Skill{
                    ID = 2 , 
                    Name = "Immortality" , 
                    Damage = 400
                } , 
                new Skill{
                    ID = 3 , 
                    Name = "Invisibility" , 
                    Damage = 100
                }
            );
        }
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();
    }
}