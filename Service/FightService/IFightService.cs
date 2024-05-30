using dotnet_rpg.Dtos.Fight;

namespace dotnet_rpg.Service.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);

    }
}