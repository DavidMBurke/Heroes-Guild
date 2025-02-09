using UnityEngine;

public class Attribute
{
    public string name;
    public int level;

    public Attribute(string name, int level = 0)
    {
        this.name = name;
        this.level = level;
    }
}
