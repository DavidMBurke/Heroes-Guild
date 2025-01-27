public class CombatSkills
{
    public int dodge;
    public int block;
    public int stealth;
    public int melee;
    public int ranged;
    public int healing;
    public int auras;
    public int evocation;

    public CombatSkills(
        int dodge = 0,
        int block = 0,
        int stealth = 0,
        int melee = 0,
        int ranged = 0,
        int healing= 0,
        int auras = 0,
        int evocation = 0
        )
    {
        this.dodge = dodge;
        this.block = block;
        this.stealth = stealth;
        this.melee = melee;
        this.ranged = ranged;
        this.healing = healing;
        this.auras = auras;
        this.evocation = evocation;
    }

    /// <summary>
    /// Roll stat die for each combat skill
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    public static CombatSkills RollBaseSkills(CombatSkills mod)
    {
        CombatSkills skills = new CombatSkills();
        PlayerCharacter.RollStat(ref skills.dodge, 2 + mod.dodge, 0, 2);
        PlayerCharacter.RollStat(ref skills.block, 2 + mod.block, 0, 2);
        PlayerCharacter.RollStat(ref skills.stealth, 2 + mod.stealth, 0, 2);
        PlayerCharacter.RollStat(ref skills.melee, 2 + mod.melee, 0, 2);
        PlayerCharacter.RollStat(ref skills.ranged, 2 + mod.ranged, 0, 2);
        PlayerCharacter.RollStat(ref skills.healing, 2 + mod.healing, 0, 2);
        PlayerCharacter.RollStat(ref skills.auras, 2 + mod.auras, 0, 2);
        PlayerCharacter.RollStat(ref skills.evocation, 2 + mod.evocation, 0, 2);
        return skills;
    }
}
