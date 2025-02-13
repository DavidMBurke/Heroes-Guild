using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlots
{
    public Dictionary<Enum, EquipmentSlot> equipmentSlots = new();

    public EquipmentSlots()
    {
        foreach(var equipmentEnum in Names)
        {
            equipmentSlots[equipmentEnum.Key] = new EquipmentSlot(Names[equipmentEnum.Key]);
        }
    }

    public void EquipItem(Enum equipmentSlotEnum, Item item)
    {
        EquipmentSlot slot = equipmentSlots[equipmentSlotEnum];
        slot.item = item;
    }
    
    public enum Enum
    {
        Head = 0,
        Body = 1,
        Legs = 2,
        Gloves = 3,
        Feet = 4,
        Neck = 5,
        Wrist = 6,
        Finger1 = 7,
        Finger2 = 8,
        Trinket1 = 9,
        Trinket2 = 10
    }

    public static readonly Dictionary<Enum, string> Names = new() 
    {
        { Enum.Head, "Head" },
        { Enum.Body, "Body" },
        { Enum.Legs, "Legs" },
        { Enum.Gloves, "Gloves" },
        { Enum.Feet, "Feet" },
        { Enum.Neck, "Neck" },
        { Enum.Wrist, "Wrist" },
        { Enum.Finger1, "Finger 1" },
        { Enum.Finger2, "Finger 2" },
        { Enum.Trinket1, "Trinket 1" },
        { Enum.Trinket2, "Trinket 2" }
    };
}

