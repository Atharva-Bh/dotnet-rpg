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

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
             var response = new ServiceResponse<AttackResultDto>();
            try 
            {
                var attacker = await _dataContext.Characters.Include(c => c.Skills).FirstOrDefaultAsync(c => c.ID == request.AttackerID);
                var opponent = await _dataContext.Characters.FirstOrDefaultAsync(c => c.ID == request.OpponentID);
                if(attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception("Something fishy is going on here...");
                }
                var skill = attacker.Skills.FirstOrDefault(s => s.ID == request.SkillID);
                if(skill is null)
                {
                    response.Success = false;
                    response.Message = " The attacker " + attacker.Name + " does not the required skill";
                    return response;
                }
                int damage = skill.Damage + new Random().Next(attacker.Intelligence);
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

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultDto>{
                Data = new FightResultDto()
            };
            try
            {
                var characters = await _dataContext.Characters.Include(c => c.Skills).
                Include(c => c.Weapon).Where(c => request.CharacterIDs.Contains(c.ID)).
                ToListAsync();
                bool defeated = false;
                while(!defeated)
                {
                    foreach(var attacker in characters)
                    {
                        var opponents = characters.Where(c => c.ID != attacker.ID).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];
                        int damage = 0;
                        string attackUsed = string.Empty;
                        bool useWeapon = new Random().Next(2)==0;
                        if(useWeapon && attacker.Weapon is not null)
                        {

                        }
                        else if(!useWeapon && attacker.Skills is not null)
                        {

                        }
                        else
                        {
                            response.Data.FightLogs
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
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