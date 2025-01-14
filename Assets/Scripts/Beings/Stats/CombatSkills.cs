public class CombatSkills
{
    public int dodge;
    public int block;
    public int stealth;
    public int melee;
    public int ranged;
    public int healing;
    public int auras;
    public int attackSpells;
    public int areaSpells;

    public CombatSkills(
        int dodge = 0,
        int block = 0,
        int stealth = 0,
        int melee = 0,
        int ranged = 0,
        int healing= 0,
        int auras = 0,
        int attackSpells = 0,
        int areaSpells = 0
        )
    {
        this.dodge = dodge;
        this.block = block;
        this.stealth = stealth;
        this.melee = melee;
        this.ranged = ranged;
        this.healing = healing;
        this.auras = auras;
        this.attackSpells = attackSpells;
        this.areaSpells = areaSpells;
    }

    public static CombatSkills RollBaseSkills(CombatSkills mod)
    {
        CombatSkills skills = new CombatSkills();
        Character.RollStat(ref skills.dodge, 2 + mod.dodge, 0, 2);
        Character.RollStat(ref skills.block, 2 + mod.block, 0, 2);
        Character.RollStat(ref skills.stealth, 2 + mod.stealth, 0, 2);
        Character.RollStat(ref skills.melee, 2 + mod.melee, 0, 2);
        Character.RollStat(ref skills.ranged, 2 + mod.ranged, 0, 2);
        Character.RollStat(ref skills.healing, 2 + mod.healing, 0, 2);
        Character.RollStat(ref skills.auras, 2 + mod.auras, 0, 2);
        Character.RollStat(ref skills.attackSpells, 2 + mod.attackSpells, 0, 2);
        Character.RollStat(ref skills.areaSpells, 2 + mod.areaSpells, 0, 2);
        return skills;
    }
}
