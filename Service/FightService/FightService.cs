using dotnet_rpg.Dtos.Fight;

namespace dotnet_rpg.Service.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _autoMapper;

        public FightService(DataContext dataContext , IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            _dataContext = dataContext;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
             var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _dataContext.Characters.
                Include(c => c.Skills).
                FirstOrDefaultAsync(c => c.ID == request.AttackerID);
                var opponent = await _dataContext.Characters.
                FirstOrDefaultAsync(c => c.ID == request.OpponentID);
                if (attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception("Something fishy is going on here...");
                }
                var skill = attacker.Skills.FirstOrDefault
                (s => s.ID == request.SkillID);
                if (skill is null)
                {
                    response.Success = false;
                    response.Message = " The attacker " + attacker.Name + " does not know the required skill";
                    return response;
                }
                int damage = DoSkillAttack(attacker, opponent, skill);
                if (opponent.HitPoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated! Better luck next time!";
                }
                await _dataContext.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            int damage = skill.Damage + new Random().Next(attacker.Intelligence);
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultDto>{
                Data = new FightResultDto()
            };
            try
            {
                var characters = await _dataContext.Characters.
                Include(c => c.Skills).
                Include(c => c.Weapon).
                Where(c => request.CharacterIDs.Contains(c.ID)).
                ToListAsync();
                bool defeated = false;
                while(!defeated)
                {
                    foreach(var attacker in characters)
                    {
                        var opponents = characters.
                        Where(c => c.ID != attacker.ID).ToList();
                        var opponent = opponents[new Random().
                        Next(opponents.Count)];
                        int damage = 0;
                        string attackUsed = string.Empty;
                        bool useWeapon = new Random().Next(2)==0;
                        if(useWeapon && attacker.Weapon is not null)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker , opponent);
                        }
                        else if(!useWeapon && attacker.Skills is not null)
                        {
                            var skill = attacker.Skills[new Random().
                            Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker , opponent , skill);
                        }
                        else
                        {
                            response.Data.FightLogs.Add($"{attacker.Name} wasnt able to attack!");
                            continue;
                        }
                        response.Data.FightLogs.Add
                        ($"{attacker.Name} has attacked {opponent.Name} with {attackUsed} and has caused {(damage >= 0 ? damage : 0)} damage");
                        if(opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.FightLogs.Add($"{opponent.Name} has been defeated!");
                            response.Data.FightLogs.
                            Add($"{attacker.Name} wins with {attacker.HitPoints} HP left!");
                            break;
                        }
                    }
                }
                characters.ForEach(c => {
                    c.Fights++;
                    c.HitPoints = 100;
                });
                await _dataContext.SaveChangesAsync();
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
                var attacker = await _dataContext.Characters.
                Include(c => c.Weapon).
                FirstOrDefaultAsync(c => c.ID == request.AttackerID);
                var opponent = await _dataContext.Characters.FirstOrDefaultAsync
                (c => c.ID == request.OpponentID);
                if (attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception("Something fishy is going on here...");
                }
                int damage = DoWeaponAttack(attacker, opponent);
                if (opponent.HitPoints <= 0)
                {
                    response.Message =
                     $"{opponent.Name} has been defeated! Better luck next time!";
                }
                await _dataContext.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if(attacker.Weapon is null)
            {
                throw new Exception("This attacker cannot attack!");
            }
            int damage = attacker.Weapon.Damage + new Random().Next(attacker.Strength);
            damage -= new Random().Next(opponent.Defense);
            if (damage > 0)
            {
                opponent.HitPoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var fighters = await _dataContext.Characters.
            Where(f => f.Fights > 0).
            OrderByDescending(v => v.Victories).
            ThenBy(l => l.Defeats).ToListAsync();
            var response = new ServiceResponse
            <List<HighScoreDto>>() {
                Data = fighters.Select(
                    f => _autoMapper.Map<HighScoreDto>(f)).ToList()
            };
            return response;

        }
    }
}