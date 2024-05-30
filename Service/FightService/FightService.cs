using dotnet_rpg.Dtos.Fight;

namespace dotnet_rpg.Service.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;

        public FightService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try 
            {
                var attacker = await _dataContext.Characters.Include(c => c.Weapon).FirstOrDefaultAsync(c => c.ID == request.AttackerID);
                var opponent = await _dataContext.Characters.FirstOrDefaultAsync(c => c.ID == request.OpponentID);
                if(attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception("Something fishy is going on here...");
                }
                int damage = attacker.Weapon.Damage + new Random().Next(attacker.Strength);
                damage -= new Random().Next(opponent.Defense);
                if(damage > 0)
                {
                    opponent.HitPoints -= damage;
                }
                if(opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated! Better luck next time!";
                }
                await _dataContext.SaveChangesAsync();
                response.Data = new AttackResultDto 
                {
                    Attacker = attacker.Name , 
                    Opponent = opponent.Name , 
                    AttackerHP = attacker.HitPoints , 
                    OpponentHP = opponent.HitPoints , 
                    Damage = damage
                };
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }
    }
}