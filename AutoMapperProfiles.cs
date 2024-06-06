using dotnet_rpg.Dtos.Fight;
using dotnet_rpg.Dtos.Skill;
using dotnet_rpg.Dtos.Weapon;

namespace dotnet_rpg
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Character , GetCharacterDto>();
            CreateMap<AddCharacterDto , Character>();
            CreateMap<Weapon , GetWeaponDto>();
            CreateMap<Skill , GetSkillDto>();
            CreateMap<Character , HighScoreDto>();
        }
    }
}