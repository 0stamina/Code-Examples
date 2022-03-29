// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""10bafe11-37e0-4ffb-949a-d297a29dc74e"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""30eb6a15-41b5-4a76-9c61-d8ac52321182"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""5af41b9e-9e93-40c0-97fa-8d38c48433bc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Gaurd"",
                    ""type"": ""Button"",
                    ""id"": ""87af30c5-bbb8-44f7-80bb-15bc7b850f76"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dodge"",
                    ""type"": ""Button"",
                    ""id"": ""e8cf7090-a366-4a53-9b0f-ac1a05dc8540"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Light Attack"",
                    ""type"": ""Button"",
                    ""id"": ""e27e8ab5-037f-4575-82a3-82538129ab3d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""fd358fae-f8db-48fc-8272-d9ef6f9367f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AutoAim"",
                    ""type"": ""Button"",
                    ""id"": ""04c1785c-e6a8-4c80-9c49-bf1eb37beb22"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""1bc0da1e-788f-4552-a4a9-10d161b91765"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0e95370c-945d-4b60-a8b0-eb40b2287749"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2ddee10c-a83a-402d-a027-91980fbb0efb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""6e2d2d55-3d3d-4fa9-8617-ccbc8edd4893"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""f0909f97-e919-421a-9274-d816e9af3668"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c540289b-aa2d-4e77-a67c-5ce035ec0f2e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e494587-14cb-4067-91e7-6ba5ed19c24d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f769254-ef24-48ec-b2a2-02d53121a1ad"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""992e5d3b-9720-4b11-b34f-1411c027bef4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18768b24-f66b-473c-b565-965067b523f0"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dodge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""105e24a3-584a-42cf-816c-bdc8369bc520"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Light Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e84fba48-71b5-4d20-8cdb-49deaebf6dda"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Light Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57c15cce-f0a8-4c41-8938-36a4de2b89c0"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""354ecf6e-70b2-43d0-8438-132c5e6fd485"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15729001-85ba-4a19-9b37-ead6ad797bc8"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Gaurd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a02879e4-3d32-4e40-b363-7ad3df7df360"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard Control"",
                    ""action"": ""Gaurd"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8553c03d-89b1-4d93-9d03-8c42f0a79ca3"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AutoAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard Control"",
            ""bindingGroup"": ""Keyboard Control"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_Aim = m_Character.FindAction("Aim", throwIfNotFound: true);
        m_Character_Gaurd = m_Character.FindAction("Gaurd", throwIfNotFound: true);
        m_Character_Dodge = m_Character.FindAction("Dodge", throwIfNotFound: true);
        m_Character_LightAttack = m_Character.FindAction("Light Attack", throwIfNotFound: true);
        m_Character_HeavyAttack = m_Character.FindAction("HeavyAttack", throwIfNotFound: true);
        m_Character_AutoAim = m_Character.FindAction("AutoAim", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Character
    private readonly InputActionMap m_Character;
    private ICharacterActions m_CharacterActionsCallbackInterface;
    private readonly InputAction m_Character_Movement;
    private readonly InputAction m_Character_Aim;
    private readonly InputAction m_Character_Gaurd;
    private readonly InputAction m_Character_Dodge;
    private readonly InputAction m_Character_LightAttack;
    private readonly InputAction m_Character_HeavyAttack;
    private readonly InputAction m_Character_AutoAim;
    public struct CharacterActions
    {
        private @PlayerInputActions m_Wrapper;
        public CharacterActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @Aim => m_Wrapper.m_Character_Aim;
        public InputAction @Gaurd => m_Wrapper.m_Character_Gaurd;
        public InputAction @Dodge => m_Wrapper.m_Character_Dodge;
        public InputAction @LightAttack => m_Wrapper.m_Character_LightAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_Character_HeavyAttack;
        public InputAction @AutoAim => m_Wrapper.m_Character_AutoAim;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnMovement;
                @Aim.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAim;
                @Gaurd.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnGaurd;
                @Gaurd.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnGaurd;
                @Gaurd.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnGaurd;
                @Dodge.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDodge;
                @Dodge.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDodge;
                @Dodge.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnDodge;
                @LightAttack.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLightAttack;
                @LightAttack.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLightAttack;
                @LightAttack.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnLightAttack;
                @HeavyAttack.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHeavyAttack;
                @HeavyAttack.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHeavyAttack;
                @HeavyAttack.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnHeavyAttack;
                @AutoAim.started -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAutoAim;
                @AutoAim.performed -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAutoAim;
                @AutoAim.canceled -= m_Wrapper.m_CharacterActionsCallbackInterface.OnAutoAim;
            }
            m_Wrapper.m_CharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Gaurd.started += instance.OnGaurd;
                @Gaurd.performed += instance.OnGaurd;
                @Gaurd.canceled += instance.OnGaurd;
                @Dodge.started += instance.OnDodge;
                @Dodge.performed += instance.OnDodge;
                @Dodge.canceled += instance.OnDodge;
                @LightAttack.started += instance.OnLightAttack;
                @LightAttack.performed += instance.OnLightAttack;
                @LightAttack.canceled += instance.OnLightAttack;
                @HeavyAttack.started += instance.OnHeavyAttack;
                @HeavyAttack.performed += instance.OnHeavyAttack;
                @HeavyAttack.canceled += instance.OnHeavyAttack;
                @AutoAim.started += instance.OnAutoAim;
                @AutoAim.performed += instance.OnAutoAim;
                @AutoAim.canceled += instance.OnAutoAim;
            }
        }
    }
    public CharacterActions @Character => new CharacterActions(this);
    private int m_KeyboardControlSchemeIndex = -1;
    public InputControlScheme KeyboardControlScheme
    {
        get
        {
            if (m_KeyboardControlSchemeIndex == -1) m_KeyboardControlSchemeIndex = asset.FindControlSchemeIndex("Keyboard Control");
            return asset.controlSchemes[m_KeyboardControlSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface ICharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnGaurd(InputAction.CallbackContext context);
        void OnDodge(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
        void OnAutoAim(InputAction.CallbackContext context);
    }
}
