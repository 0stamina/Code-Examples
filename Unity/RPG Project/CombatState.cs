using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatState : MonoBehaviour
{
    public CombatHandler combat_handler;
    public abstract void StartState();
    public abstract void RunState();
    public abstract void EndState();
}
