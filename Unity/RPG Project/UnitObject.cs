using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * make sure to take advantage of struct inheritence and static structs when transferring to c++
 */


public enum Stats { None = -1, HP, MP, Strength, Endurance, Wisdom, Spirit, Dexterity, Agility }
[Flags]
public enum WeaponType { None = -1, Universal = 0, Everything = 255, Sword = 1, Axe = 2, Dagger = 4, Spear = 8, Bow = 16, Staff = 32, Gauntlet = 64, Hammer = 128 }
[Flags]
public enum ArmorType { None = -1, Universal = 0, Everything = 7, Light = 1, Medium = 2, Heavy = 4 }
public enum CombatCategory { Physical = 2, Magic = 4, Status }
public enum CombatRange { Melee, Ranged }

[Serializable]
public struct Unit
{
    public int class_id;
    public int level;
    public int experience;
    public string name;
    public int[] stats;
    public float[] stat_growths;
    public int weapon_id;
    public int armor_id;
    public Color skin_color;
    public Color hair_color;
    public Sprite sprite;
    public Material skin_hair_mat;

    public Unit(string name, int level = 1)
    {
        this.level = Mathf.Clamp(level, 1, 99);
        experience = 50 * level * ((level - 1)/2);

        if (name.Length > 10)
            name = name.Remove(10);

        this.name = name;
        stats = new int[8];
        stat_growths = new float[8];
        weapon_id = -1;
        armor_id = -1;
        class_id = -1;
        skin_color = new Color();
        hair_color = new Color();
        sprite = null;
        skin_hair_mat = null;
    }
}

[Serializable]
public struct ActionInfo
{
    public CombatCategory category;
    public CombatRange range;
    public Stats stat;
    public bool can_target_self;
    public bool can_target_all;
    public int value;
}

[Serializable]
public struct Skill
{
    public string name;
    public ActionInfo action_info;
}

[Serializable]
public struct Armor
{
    public string name;
    public ArmorType armor_type;
    public int physical_defense;
    public int magic_defense;
    public int cost;
}

[Serializable]
public struct Weapon
{
    public string name;
    public WeaponType weapon_type;
    public ActionInfo action_info;
    public int cost;
}

[Serializable]
public struct UnitClass
{
    public string name;
    public WeaponType usable_weapons;
    public ArmorType usable_armor;
    public int[] stats;
    public float[] stat_growths;
}

