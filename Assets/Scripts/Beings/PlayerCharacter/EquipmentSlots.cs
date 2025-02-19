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
        Body,
        Legs,
        Hands,
        Feet,
        Ammo,
        Neck,
        Wrist,
        Finger1,
        Finger2,
        Trinket1,
        Trinket2,
        LeftHand,
        RightHand,
        Pouch1,
        Pouch2,
        Pouch3,
        Pouch4
    }

    public static readonly Dictionary<Enum, string> Names = new() 
    {
        { Enum.Head, "Head" },
        { Enum.Body, "Body" },
        { Enum.Legs, "Legs" },
        { Enum.Hands, "Hands" },
        { Enum.Feet, "Feet" },
        { Enum.Ammo, "Ammo" },
        { Enum.Neck, "Neck" },
        { Enum.Wrist, "Wrist" },
        { Enum.Finger1, "Finger 1" },
        { Enum.Finger2, "Finger 2" },
        { Enum.Trinket1, "Trinket 1" },
        { Enum.Trinket2, "Trinket 2" },
        { Enum.LeftHand, "Left Hand" },
        { Enum.RightHand, "Right Hand" },
        { Enum.Pouch1, "Pouch 1" },
        { Enum.Pouch2, "Pouch 2" },
        { Enum.Pouch3, "Pouch 3" },
        { Enum.Pouch4, "Pouch 4" }
    };
}

