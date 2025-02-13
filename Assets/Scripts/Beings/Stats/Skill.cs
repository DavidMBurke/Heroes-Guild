using System.Collections.Generic;

public class Skill
{
    public string name;
    public int level;
    int experience;

    public Skill(string name, int level = 1, int experience = 0)
    {
        this.name = name;
        this.level = level;
        this.experience = experience;
    }

    public void AddXP(int addedXP)
    {
        experience += addedXP;
        CheckForLevelUp();
    }

    private void CheckForLevelUp()
    {
        while (experience > LevelExperienceRequirement[level].Item2)
        {
            level++;
        }
    }

    public static List<(int, int)> LevelExperienceRequirement = new List<(int, int)>
    {
        (1, 0),
        (2, 100),
        (3, 200),
        (4, 400),
        (5, 700),
        (6, 1200),
        (7, 2000),
        (8, 3300),
        (9, 5400),
        (10, 8800),
        (11, 14300),
        (12, 23200),
        (13, 37600),
        (14, 60900),
        (15, 98600),
        (16, 159600),
        (17, 258300),
        (18, 41800),
        (19, 676400),
        (20, 1094500),
        (21, 1771000),
        (22, 2865600),
        (23, 4636700),
        (24, 7502400),
        (25, 12139200),
        (26, 19641700),
        (27, 31781000),
        (28, 51422800),
        (29, 83203900),
        (30, 134626800)
    };
}
