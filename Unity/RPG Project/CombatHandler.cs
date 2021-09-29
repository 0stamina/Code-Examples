using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[Serializable]
public struct CombatUnit
{
    public Unit unit;
    public int[] temp_stats;
    public CombatEvent[] positive_effects;
    public CombatEvent[] negative_effects;
    public bool exists;
    public bool back_line;
    public bool alive;

    public CombatUnit(Unit unit, bool back_line)
    {
        this.unit = unit;
        this.back_line = back_line;
        temp_stats = new int[8];
        for (int i = 0; i < 8; i++)
        {
            temp_stats[i] = unit.stats[i];
        }
        positive_effects = new CombatEvent[8];
        negative_effects = new CombatEvent[8];
        exists = true;
        alive = true;
    }
}

[Serializable]
public struct CombatUnitText
{
    public Text ui_text;
    public Image ui_background;
}

[Serializable]
public struct CombatEvent
{
    public bool player_unit;
    public int unit;
    public bool target_allies;
    public int target;
    public ActionInfo action_info;
    public int turn_amt;
}

public class CombatHandler : MonoBehaviour
{
    public GameObject unit_sprite_preset;

    public MainCombatState main_combat_state;
    public TargetingCombatState targeting_combat_state;
    public CombatState curr_combat_state;

    public bool player_turn;
    public int curr_unit;

    public List<CombatEvent> combat_actions;

    public CombatUnit[] player_team = new CombatUnit[5];
    public CombatUnit[] enemy_team = new CombatUnit[5];
    public CombatUnitText[] player_unit_text = new CombatUnitText[5];
    public CombatUnitText[] enemy_unit_text = new CombatUnitText[5];

    public void Start()
    {
        { 
            GameObject o;

            for (int i = 0; i < God.team.Count; i++)
            {
                player_team[i] = new CombatUnit(God.barracks[God.team[i].i], God.team[i].b);
                o = Instantiate(unit_sprite_preset, (God.team[i].b ? targeting_combat_state.player_backline : targeting_combat_state.player_frontline));
            }

            for (int i = 0; i < 5; i++)
            {
                enemy_team[i] = new CombatUnit(new Unit("Wolf", 10), false);
                enemy_team[i].temp_stats[(int)Stats.HP] = enemy_team[0].unit.stats[(int)Stats.HP] = 10;
                enemy_team[i].temp_stats[(int)Stats.MP] = enemy_team[0].unit.stats[(int)Stats.MP] = 0;
                o = Instantiate(unit_sprite_preset, targeting_combat_state.enemy_frontline);
                o.transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        StartTurn(true);
    }

    public void Update()
    {
        //display unit data
        {
            for (int i = 0; i < 5; i++)
            {
                if (player_team[i].exists)
                {
                    player_unit_text[i].ui_text.alignment = (TextAnchor)(player_team[i].back_line ? 5 : 3);
                    player_unit_text[i].ui_text.text = ("LV" + player_team[i].unit.level).PadRight(7) + player_team[i].unit.name.PadRight(13)
                        + (player_team[i].temp_stats[(int)Stats.HP].ToString() + '\\' + player_team[i].unit.stats[(int)Stats.HP].ToString()).PadRight(12)
                        + (player_team[i].temp_stats[(int)Stats.MP].ToString() + '\\' + player_team[i].unit.stats[(int)Stats.MP].ToString()).PadRight(9);
                }
                else
                {
                    player_unit_text[i].ui_text.alignment = (TextAnchor)4;
                    player_unit_text[i].ui_text.text = "";
                }

            }

            for (int i = 0; i < 5; i++)
            {
                if (enemy_team[i].exists)
                {
                    enemy_unit_text[i].ui_text.alignment = (TextAnchor)(enemy_team[i].back_line ? 3 : 5);
                    enemy_unit_text[i].ui_text.text = ("LV" + enemy_team[i].unit.level).PadRight(7) + enemy_team[i].unit.name.PadRight(10);
                }
                else
                {
                    enemy_unit_text[i].ui_text.alignment = (TextAnchor)4;
                    enemy_unit_text[i].ui_text.text = "";
                }
            }
        }

        if(player_turn)
        {
            curr_combat_state.RunState();
        }
    }

    public void StartTurn(bool player_turn)
    {
        this.player_turn = player_turn;
        if (player_turn)
        {
            InitUnit(0);
        }
        else
        {
            for(;combat_actions.Count > 0; combat_actions.RemoveAt(0))
            {
                CombatEventResult(combat_actions[0]);
            }
            player_unit_text[curr_unit].ui_background.color = Color.clear;
            //StartTurn(true);
        }
    }

    public void CombatEventResult(CombatEvent e)
    {
        bool negative = (e.action_info.value & short.MinValue) == short.MinValue;
        bool target_player_team = !(e.player_unit ^ e.target_allies);
        int switch_val = (negative ? 1 : 0) + (target_player_team ? 2 : 0) + ((int)e.action_info.stat <= 1 ? 4 : 0);
        switch (switch_val)
        {
            case 0:
                {
                    for(int i = (e.target == 6 ? 0 : e.target); i <= (e.target == 6 ? 5 : e.target); i++)
                    {
                        if (enemy_team[i].positive_effects[(int)e.action_info.stat].action_info.value < e.action_info.value)
                        {
                            int v = enemy_team[i].temp_stats[(int)e.action_info.stat] - enemy_team[i].positive_effects[(int)e.action_info.stat].action_info.value + e.action_info.value;
                            v = Mathf.Clamp(v, 0, 9999);
                            enemy_team[i].positive_effects[(int)e.action_info.stat].action_info.value = e.action_info.value;
                        }

                        if (enemy_team[i].positive_effects[(int)e.action_info.stat].turn_amt > e.turn_amt)
                        {
                            enemy_team[i].positive_effects[(int)e.action_info.stat].turn_amt = e.turn_amt;
                        }
                    }
                    break;
                }

            case 1:
                {
                    for (int i = (e.target == 6 ? 0 : e.target); i <= (e.target == 6 ? 5 : e.target); i++)
                    {
                        if (enemy_team[i].negative_effects[(int)e.action_info.stat].action_info.value < e.action_info.value)
                        {
                            int v = enemy_team[i].temp_stats[(int)e.action_info.stat] - enemy_team[i].negative_effects[(int)e.action_info.stat].action_info.value + e.action_info.value;
                            v = Mathf.Clamp(v, 0, 9999);
                            enemy_team[i].negative_effects[(int)e.action_info.stat].action_info.value = e.action_info.value;
                        }

                        if (enemy_team[i].negative_effects[(int)e.action_info.stat].turn_amt > e.turn_amt)
                        {
                            enemy_team[i].negative_effects[(int)e.action_info.stat].turn_amt = e.turn_amt;
                        }
                    }
                    break;
                }

            case 2:
                {
                    for (int i = (e.target == 6 ? 0 : e.target); i <= (e.target == 6 ? 5 : e.target); i++)
                    {
                        if (player_team[i].positive_effects[(int)e.action_info.stat].action_info.value < e.action_info.value)
                        {
                            int v = player_team[i].temp_stats[(int)e.action_info.stat] - player_team[i].positive_effects[(int)e.action_info.stat].action_info.value + e.action_info.value;
                            v = Mathf.Clamp(v, 0, 9999);
                            player_team[i].positive_effects[(int)e.action_info.stat].action_info.value = e.action_info.value;
                        }

                        if (player_team[i].positive_effects[(int)e.action_info.stat].turn_amt > e.turn_amt)
                        {
                            player_team[i].positive_effects[(int)e.action_info.stat].turn_amt = e.turn_amt;
                        }
                    }
                    break;
                }

            case 3:
                {
                    for (int i = (e.target == 6 ? 0 : e.target); i <= (e.target == 6 ? 5 : e.target); i++)
                    {
                        if (player_team[i].negative_effects[(int)e.action_info.stat].action_info.value < e.action_info.value)
                        {
                            int v = player_team[i].temp_stats[(int)e.action_info.stat] - player_team[i].negative_effects[(int)e.action_info.stat].action_info.value + e.action_info.value;
                            v = Mathf.Clamp(v, 0, 9999);
                            player_team[i].negative_effects[(int)e.action_info.stat].action_info.value = e.action_info.value;
                        }

                        if (player_team[i].negative_effects[(int)e.action_info.stat].turn_amt > e.turn_amt)
                        {
                            player_team[i].negative_effects[(int)e.action_info.stat].turn_amt = e.turn_amt;
                        }
                    }
                    break;
                }

            case 4:
            case 5:
                {
                    for (int i = (e.target == 6 ? 0 : e.target); i <= (e.target == 6 ? 4 : e.target); i++)
                    {
                        int v = enemy_team[i].temp_stats[(int)e.action_info.stat] + e.action_info.value;
                        v = Mathf.Clamp(v, 0, 9999);
                        enemy_team[i].temp_stats[(int)e.action_info.stat] = (ushort)v;
                    }
                    break;
                }

            case 6:
            case 7:
                {
                    for (int i = (e.target == 6 ? 0 : e.target); i <= (e.target == 6 ? 5 : e.target); i++)
                    {
                        int v = player_team[i].temp_stats[(int)e.action_info.stat] + e.action_info.value;
                        v = Mathf.Clamp(v, 0, 9999);
                        player_team[i].temp_stats[(int)e.action_info.stat] = (ushort)v;
                    }
                    break;
                }
        }

    }
    public void InitUnit(int unit)
    {
        if (unit < 0)
        {
            unit = 0;
        }

        if(unit >= player_team.Length || !player_team[unit].exists)
        {
            StartTurn(false);
            return;
        }

        curr_combat_state = main_combat_state;
        curr_combat_state.StartState();

        player_unit_text[curr_unit].ui_background.color = Color.clear;
        player_unit_text[unit].ui_background.color = Color.white/2;

        curr_unit = unit;
    }
}

