using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingCombatState : CombatState
{
    public Transform player_frontline;
    public Transform player_backline;
    public Transform enemy_frontline;
    public Transform enemy_backline;
    public List<Transform> enemy_unit_list = new List<Transform>();
    public List<Transform> player_unit_list = new List<Transform>();

    public ActionInfo action_info;

    public int option;
    public int curr_target;
    public bool targeting_all;
    public bool targeting_allies;

    public override void StartState()
    {
        if(option == 0)
        {
            int weapon = combat_handler.player_team[combat_handler.curr_unit].unit.weapon_id;
            if (weapon != -1)
            {
                action_info = God.weapons[weapon].action_info;
            }
            else
            {
                action_info = new ActionInfo() { value = -1 };
            }
        }

        targeting_allies = (action_info.value & short.MinValue) != short.MinValue;

        curr_target = 0;
        if(!targeting_allies)
        {
            if (combat_handler.enemy_team[curr_target].temp_stats[(int)Stats.HP] <= 0 || combat_handler.enemy_team[curr_target].back_line)
            {
                int next = 0;
                bool first = true;
                for (int i = 1; i <= 5; i++, first = false)
                {
                    int check = i - (5 * Mathf.FloorToInt(i / 5.0f));

                    if (combat_handler.enemy_team[check].temp_stats[(int)Stats.HP] <= 0)
                    {
                        continue;
                    }
                    if (combat_handler.enemy_team[next].temp_stats[(int)Stats.HP] <= 0)
                    {
                        next = check;
                    }

                    if (action_info.range == CombatRange.Ranged || (action_info.range == CombatRange.Melee && !combat_handler.enemy_team[check].back_line))
                    {
                        next = check;
                        break;
                    }
                    else if (first)
                    {
                        next = check;
                        continue;
                    }
                }

                curr_target = next;
            }
            enemy_unit_list[curr_target].GetChild(0).gameObject.SetActive(true);
            combat_handler.enemy_unit_text[curr_target].ui_background.color = Color.yellow / 2;
        }
        else
        {
            if (combat_handler.player_team[curr_target].temp_stats[(int)Stats.HP] <= 0 || (!action_info.can_target_self && curr_target == combat_handler.curr_unit))
            {
                int next = 0;
                for (int i = 1; i <= 5; i++)
                {
                    int check = i - (5 * Mathf.FloorToInt(i / 5.0f));

                    if (combat_handler.player_team[check].temp_stats[(int)Stats.HP] <= 0 || (!action_info.can_target_self && check == combat_handler.curr_unit))
                    {
                        continue;
                    }
                    else
                    {
                        next = check;
                        break;
                    }
                }

                curr_target = next;
            }
            player_unit_list[curr_target].GetChild(0).gameObject.SetActive(true);
            combat_handler.player_unit_text[curr_target].ui_background.color = Color.yellow / 2;
        }

    }
    public override void RunState()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if(!action_info.can_target_all)
            {
                goto done;
            }

            if (!targeting_all)
            {
                if (!targeting_allies)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (combat_handler.player_team[i].alive)
                        {
                            enemy_unit_list[i].GetChild(0).gameObject.SetActive(true);
                            combat_handler.enemy_unit_text[i].ui_background.color = Color.yellow / 2;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (combat_handler.player_team[i].alive)
                        {
                            player_unit_list[i].GetChild(0).gameObject.SetActive(true);
                            combat_handler.player_unit_text[i].ui_background.color = Color.yellow / 2;
                        }
                    }
                }
            }
            else
            {
                if (!targeting_allies)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i != curr_target)
                        {
                            combat_handler.enemy_unit_text[i].ui_background.color = Color.clear;
                            enemy_unit_list[i].GetChild(0).gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (i != curr_target)
                        {
                            combat_handler.player_unit_text[i].ui_background.color = Color.clear;
                            player_unit_list[i].GetChild(0).gameObject.SetActive(false);
                        }
                    }
                    combat_handler.player_unit_text[combat_handler.curr_unit].ui_background.color = Color.white / 2;
                }
            }
            targeting_all = !targeting_all;
        done:;
        }
        else if (Input.GetButtonDown("Vertical"))
        {
            if (targeting_all)
            {
                goto done;
            }

            if (!targeting_allies)
            {
                combat_handler.enemy_unit_text[curr_target].ui_background.color = Color.clear;
                enemy_unit_list[curr_target].GetChild(0).gameObject.SetActive(false);
                int next = curr_target;
                int rel = -(int)Mathf.Sign(Input.GetAxis("Vertical"));
                bool first = true;
                for (int i = 1; i <= 5; i++, first = false)
                {
                    int check = curr_target + (i * rel);
                    check -= (5 * Mathf.FloorToInt(check / 5.0f));

                    if (combat_handler.enemy_team[check].temp_stats[(int)Stats.HP] <= 0)
                    {
                        continue;
                    }
                    if (combat_handler.enemy_team[next].temp_stats[(int)Stats.HP] <= 0)
                    {
                        next = check;
                    }

                    if (action_info.range == CombatRange.Ranged || (action_info.range == CombatRange.Melee && !combat_handler.enemy_team[check].back_line))
                    {
                        next = check;
                        break;
                    }
                    else if (first)
                    {
                        next = check;
                        continue;
                    }
                }

                if (next == curr_target && combat_handler.enemy_team[curr_target].temp_stats[(int)Stats.HP] <= 0)
                {
                    print("something very bad has happened in the tareting script\nfix it you dingus");
                    return;
                }

                curr_target = next;

                enemy_unit_list[curr_target].GetChild(0).gameObject.SetActive(true);
                combat_handler.enemy_unit_text[curr_target].ui_background.color = Color.yellow / 2;
            }
            else
            {
                combat_handler.player_unit_text[curr_target].ui_background.color = Color.clear;
                combat_handler.player_unit_text[combat_handler.curr_unit].ui_background.color = Color.white / 2;
                player_unit_list[curr_target].GetChild(0).gameObject.SetActive(false);
                int next = curr_target;
                int rel = -(int)Mathf.Sign(Input.GetAxis("Vertical"));
                for (int i = 1; i <= 5; i++)
                {
                    int check = curr_target + (i * rel);
                    check -= (5 * Mathf.FloorToInt(check / 5.0f));

                    if (combat_handler.player_team[check].temp_stats[(int)Stats.HP] <= 0 || (!action_info.can_target_self && check == combat_handler.curr_unit))
                    {
                        continue;
                    }
                    else
                    {
                        next = check;
                        break;
                    }
                }

                if (next == curr_target && combat_handler.player_team[curr_target].temp_stats[(int)Stats.HP] <= 0)
                {
                    print("something very bad has happened in the tareting script\nfix it you dingus");
                    return;
                }

                curr_target = next;

                player_unit_list[curr_target].GetChild(0).gameObject.SetActive(true);
                combat_handler.player_unit_text[curr_target].ui_background.color = Color.yellow / 2;
            }

        done:;
        }
        else if (Input.GetButtonDown("Horizontal"))
        {
            if (targeting_all)
            {
                if (!targeting_allies)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        combat_handler.enemy_unit_text[i].ui_background.color = Color.clear;
                        enemy_unit_list[i].GetChild(0).gameObject.SetActive(false);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (combat_handler.player_team[i].alive)
                        {
                            player_unit_list[i].GetChild(0).gameObject.SetActive(true);
                            combat_handler.player_unit_text[i].ui_background.color = Color.yellow / 2;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        combat_handler.player_unit_text[i].ui_background.color = Color.clear;
                        player_unit_list[i].GetChild(0).gameObject.SetActive(false);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        if (combat_handler.enemy_team[i].alive)
                        {
                            enemy_unit_list[i].GetChild(0).gameObject.SetActive(true);
                            combat_handler.enemy_unit_text[i].ui_background.color = Color.yellow / 2;
                        }
                    }
                    combat_handler.player_unit_text[combat_handler.curr_unit].ui_background.color = Color.white / 2;
                }
                goto done;
            }

            if (!targeting_allies)
            {
                combat_handler.enemy_unit_text[curr_target].ui_background.color = Color.clear;
                enemy_unit_list[curr_target].GetChild(0).gameObject.SetActive(false);
                if (combat_handler.player_team[curr_target].temp_stats[(int)Stats.HP] <= 0 || (!action_info.can_target_self && curr_target == combat_handler.curr_unit))
                {
                    int next = 0;
                    for (int i = 1; i <= 5; i++)
                    {
                        int check = i - (5 * Mathf.FloorToInt(i / 5.0f));

                        if (combat_handler.player_team[check].temp_stats[(int)Stats.HP] <= 0 || (!action_info.can_target_self && check == combat_handler.curr_unit))
                        {
                            continue;
                        }
                        else
                        {
                            next = check;
                            break;
                        }
                    }

                    curr_target = next;
                }
                player_unit_list[curr_target].GetChild(0).gameObject.SetActive(true);
                combat_handler.player_unit_text[curr_target].ui_background.color = Color.yellow / 2;
            }
            else
            {
                combat_handler.player_unit_text[curr_target].ui_background.color = Color.clear;
                combat_handler.player_unit_text[combat_handler.curr_unit].ui_background.color = Color.white / 2;
                player_unit_list[curr_target].GetChild(0).gameObject.SetActive(false);
                if (combat_handler.enemy_team[curr_target].temp_stats[(int)Stats.HP] <= 0 || combat_handler.enemy_team[curr_target].back_line)
                {
                    int next = 0;
                    bool first = true;
                    for (int i = 1; i <= 5; i++, first = false)
                    {
                        int check = i - (5 * Mathf.FloorToInt(i / 5.0f));

                        print(check);

                        if (combat_handler.enemy_team[check].temp_stats[(int)Stats.HP] <= 0)
                        {
                            continue;
                        }
                        if (combat_handler.enemy_team[next].temp_stats[(int)Stats.HP] <= 0)
                        {
                            next = check;
                        }

                        if (action_info.range == CombatRange.Ranged || (action_info.range == CombatRange.Melee && !combat_handler.enemy_team[check].back_line))
                        {
                            next = check;
                            break;
                        }
                        else if (first)
                        {
                            next = check;
                            continue;
                        }
                    }

                    curr_target = next;
                }
                enemy_unit_list[curr_target].GetChild(0).gameObject.SetActive(true);
                combat_handler.enemy_unit_text[curr_target].ui_background.color = Color.yellow / 2;
            }

        done:targeting_allies = !targeting_allies;
        }
        else if (Input.GetButtonDown("Submit"))
        {
            CombatEvent ce = new CombatEvent();
            ce.player_unit = true;
            ce.target = curr_target;
            ce.action_info = action_info;
            combat_handler.combat_actions.Add(ce);
            EndState();
            combat_handler.InitUnit(combat_handler.curr_unit + 1);
        }
        else if (Input.GetButtonDown("Back"))
        {
            EndState();
            combat_handler.InitUnit(combat_handler.curr_unit);
        }
    }
    public override void EndState()
    {
        if (!targeting_allies)
        {
            enemy_unit_list[curr_target].GetChild(0).gameObject.SetActive(false);
            combat_handler.enemy_unit_text[curr_target].ui_background.color = Color.clear;
        }
        else
        {
            player_unit_list[curr_target].GetChild(0).gameObject.SetActive(false);
            combat_handler.player_unit_text[curr_target].ui_background.color = Color.clear;
            combat_handler.player_unit_text[combat_handler.curr_unit].ui_background.color = Color.white/2;
        }
    }
}
