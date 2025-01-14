public class NonCombatSkills
{
    public int cooking;
    public int sentry;
    public int fletching;
    public int trapping;
    public int herbalism;
    public int medicine;
    public int leatherWorking;
    public int tailoring;
    public int alchemy;
    public int armorSmithing;
    public int weaponSmithing;
    public int enchanting;
    public int mechanisms;
    public int jewelryCrafting;
    public int mining;
    public int animalHandling;
    public int cartography;
    public int barter;

    public NonCombatSkills(
        int cooking = 0,
        int sentry = 0,
        int fletching = 0,
        int trapping = 0,
        int herbalism = 0,
        int medicine = 0,
        int leatherWorking = 0,
        int tailoring = 0,
        int alchemy = 0,
        int armorSmithing = 0,
        int weaponSmithing = 0,
        int enchanting = 0,
        int mechanisms = 0,
        int jewelryCrafting = 0,
        int mining = 0,
        int monsterWrangling = 0,
        int cartography = 0,
        int barter = 0
        )
    {
        this.cooking = cooking;
        this.sentry = sentry;
        this.fletching = fletching;
        this.trapping = trapping;
        this.herbalism = herbalism;
        this.medicine = medicine;
        this.leatherWorking = leatherWorking;
        this.tailoring = tailoring;
        this.alchemy = alchemy;
        this.armorSmithing = armorSmithing;
        this.weaponSmithing = weaponSmithing;
        this.enchanting = enchanting;
        this.mechanisms = mechanisms;
        this.jewelryCrafting = jewelryCrafting;
        this.mining = mining;
        this.animalHandling = monsterWrangling;
        this.cartography = cartography;
        this.barter = barter;
    }

    public static NonCombatSkills RollBaseSkills(NonCombatSkills mod)
    {
        NonCombatSkills skills = new NonCombatSkills();
        Character.RollStat(ref skills.cooking, 2 + mod.cooking, -1, 2);
        Character.RollStat(ref skills.sentry, 2 + mod.sentry, -1, 2);
        Character.RollStat(ref skills.fletching, 2 + mod.fletching, -1, 2);
        Character.RollStat(ref skills.trapping, 2 + mod.trapping, -1, 2);
        Character.RollStat(ref skills.herbalism, 2 + mod.herbalism, -1, 2);
        Character.RollStat(ref skills.medicine, 2 + mod.medicine, -1, 2);
        Character.RollStat(ref skills.leatherWorking, 2 + mod.leatherWorking, -1, 2);
        Character.RollStat(ref skills.tailoring, 2 + mod.tailoring, -1, 2);
        Character.RollStat(ref skills.alchemy, 2 + mod.alchemy, -1, 2);
        Character.RollStat(ref skills.armorSmithing, 2 + mod.armorSmithing, -1, 2);
        Character.RollStat(ref skills.weaponSmithing, 2 + mod.weaponSmithing, -1, 2);
        Character.RollStat(ref skills.enchanting, 2 + mod.enchanting, -1, 2);
        Character.RollStat(ref skills.mechanisms, 2 + mod.mechanisms, -1, 2);
        Character.RollStat(ref skills.jewelryCrafting, 2 + mod.jewelryCrafting, -1, 2);
        Character.RollStat(ref skills.mining, 2 + mod.mining, -1, 2);
        Character.RollStat(ref skills.animalHandling, 2 + mod.animalHandling, -1, 2);
        Character.RollStat(ref skills.cartography, 2 + mod.cartography, -1, 2);
        Character.RollStat(ref skills.barter, 2 + mod.barter, -1, 2);
        return skills;
    }
}
