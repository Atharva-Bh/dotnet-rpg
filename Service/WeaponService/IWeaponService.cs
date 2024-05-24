namespace dotnet_rpg.Service.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);

    }
}