namespace dotnet_rpg
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Character , GetCharacterDto>();
            CreateMap<AddCharacterDto , Character>();
        }
    }
}