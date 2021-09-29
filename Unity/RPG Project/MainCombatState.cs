using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCombatState : CombatState
{
    public Image panel;
    public Image[] options;
    public int option;
    public override void StartState()
    {
        for(int i = 0; i < options.Length; i++)
        {
            options[i].gameObject.SetActive(true);
        }
        panel.enabled = true;
        option = 0;
        options[0].color = Color.white/2;
    }

    public override void RunState()
    {
        if (Input.GetButtonDown("Vertical"))
        {
            options[option].color = Color.clear;
            int rel = -(int)Mathf.Sign(Input.GetAxis("Vertical"));
            option = option + rel < 0 ? options.Length - 1 : (option + rel > options.Length - 1 ? 0 : option + rel);
            options[option].color = Color.white / 2;
        }
        else if (Input.GetButtonDown("Horizontal"))
        {
            options[option].color = Color.clear;
            switch (option)
            {
                case 4:
                    {
                        option = 0;
                        break;
                    }
                default:
                    {
                        option = options.Length - 1;
                        break;
                    }
            }
            options[option].color = Color.white / 2;
        }
        else if(Input.GetButtonDown("Back"))
        {
            option = 5;
            EndState();
        }
        else if (Input.GetButtonDown("Submit"))
        {
            EndState();
        }
    }

    public override void EndState()
    {
        options[option].color = Color.clear;
        switch (option)
        {
            case 0:
                {
                    for (int i = 0; i < options.Length; i++)
                    {
                        options[i].gameObject.SetActive(false);
                    }
                    panel.enabled = false;
                    combat_handler.curr_combat_state = combat_handler.targeting_combat_state;
                    combat_handler.targeting_combat_state.StartState();
                    combat_handler.targeting_combat_state.option = option;
                    break;
                }
            case 3:
                {
                    combat_handler.InitUnit(combat_handler.curr_unit + 1);
                    break;
                }
            case 4:
                {
                    combat_handler.InitUnit(combat_handler.curr_unit - 1);
                    break;
                }
        }
        
    }
}
