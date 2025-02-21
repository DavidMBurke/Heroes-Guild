[System.Serializable]
public class EquipmentSlot
{
    public string name = string.Empty;
    
    #nullable enable
    public Item? item = null;
    #nullable disable

    public EquipmentSlot(string name)
    {
        this.name = name;
    }
}
