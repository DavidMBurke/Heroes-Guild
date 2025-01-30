using UnityEngine;

public class EquipmentSlots : MonoBehaviour
{

    EquipmentSlot head;
    EquipmentSlot body;
    EquipmentSlot legs;
    EquipmentSlot gloves;
    EquipmentSlot feet;
    EquipmentSlot neck;
    EquipmentSlot wrist;
    EquipmentSlot finger1;
    EquipmentSlot finger2;
    EquipmentSlot trinket1;
    EquipmentSlot trinket2;
}

public enum EquipmentSlotEnum
{
    head = 0,
    body = 1,
    legs = 2,
    gloves = 3,
    feet = 4,
    neck = 5,
    wrist = 6,
    finger1 = 7,
    finger2 = 8,
    trinket1 = 9,
    trinket2 = 10
}
