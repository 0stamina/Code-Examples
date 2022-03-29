using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions actions;
    public InputAction movement;

    public PlayerActor character;

    public void OnEnable()
    {
        movement = actions.Character.Movement;
        movement.Enable();

        actions.Character.Dodge.performed += DoDodge;
        actions.Character.Dodge.Enable();
        actions.Character.LightAttack.performed += DoLightAttack;
        actions.Character.LightAttack.Enable();
        actions.Character.HeavyAttack.performed += DoHeavyAttack;
        actions.Character.HeavyAttack.Enable();
    }

    public void Awake()
    {
        actions = new PlayerInputActions();
    }

    public void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FindCharacter();
        }
    }

    public void FixedUpdate()
    {
        if(character != null)
        {
            character.Move(movement.ReadValue<Vector2>().normalized);
        }
    }

    public void FindCharacter()
    {
        PlayerActor[] chars = FindObjectsOfType<PlayerActor>();
        foreach(PlayerActor c in chars)
        {
            if(c.isPlayerControlled && c.player == null)
            {
                c.player = this;
                transform.parent = c.transform;
                character = c;
                break;
            }
        }
    }

    public void DoDodge(InputAction.CallbackContext obj)
    {
        if(character != null)
        {
            character.Dodge();
        }
    }
    public void DoLightAttack(InputAction.CallbackContext obj)
    {
    }
    public void DoHeavyAttack(InputAction.CallbackContext obj)
    {
    }
}
