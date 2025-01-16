
public class Affinities
{
    public int nature;
    public int arcana;
    public int celestial;
    public int spiritual;
    public int qi;

    public Affinities(int nature = 0, int arcana = 0, int celestial = 0, int spiritual = 0, int qi = 0)
    {
        this.nature = nature;
        this.arcana = arcana;
        this.celestial = celestial;
        this.spiritual = spiritual;
        this.qi = qi;
    }

    public static Affinities RollBaseAffinities(Affinities mods)
    {
        Affinities affinities = new Affinities();
        PlayerCharacter.RollStat(ref affinities.nature, 2 + mods.nature, 0, 2);
        PlayerCharacter.RollStat(ref affinities.arcana, 2 + mods.arcana, 0, 2);
        PlayerCharacter.RollStat(ref affinities.celestial, 2 + mods.celestial, 0, 2);
        PlayerCharacter.RollStat(ref affinities.spiritual, 2 + mods.spiritual, 0, 2);
        PlayerCharacter.RollStat(ref affinities.qi, 2 + mods.qi, 0, 2);
        return affinities;
    }
}