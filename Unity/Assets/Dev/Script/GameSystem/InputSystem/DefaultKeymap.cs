//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Dev/Data/DefaultKeymap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @DefaultKeymap: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultKeymap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DefaultKeymap"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""2a21b6ac-1b20-440f-b6f5-3e06dc8fdfd2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""5b51b5fa-c834-48bd-8767-15860c5e0051"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""PassThrough"",
                    ""id"": ""690a4b11-ab1b-41db-af4f-b43422d821e9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""5bb6e3fe-0e7f-46c4-84ab-0423b6fde981"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""5a61ab1f-d552-456e-b080-856e8b62fffe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Fishing"",
                    ""type"": ""Button"",
                    ""id"": ""bf848985-0be4-4eb9-9a2b-a24e80494ac9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""46219aa2-8bbd-4014-9c6f-2e21b5d8f522"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""02a1f54e-7310-414a-8ed4-c4571d571d34"",
                    ""path"": ""<Keyboard>/#(W)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e7250860-4bad-417f-9468-46c3f33c812b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f0308ced-dc32-4b57-8780-36fe9e15a523"",
                    ""path"": ""<Keyboard>/#(A)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""eddaaf4d-f106-4b13-a6b7-b666f65a64e2"",
                    ""path"": ""<Keyboard>/#(D)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""905ea5a5-328d-4777-9017-1ac8bf617cdc"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""109f2777-561c-45fb-acf6-4c913737fc3a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8c53c89b-1df0-4e25-adcc-0c1157f539cb"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3a084c0-4769-4084-a75e-3704eb8bb21b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Fishing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""4ff4bc3e-38eb-4c34-9074-7ef91a24d0ff"",
            ""actions"": [
                {
                    ""name"": ""DialogueSkip"",
                    ""type"": ""Button"",
                    ""id"": ""af70dc0b-fd27-46fd-8984-44a875fb5e03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSlotScroll"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a235a3c1-4b77-4660-919b-dea1fdfde0d5"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""QuickSlotScrollButton"",
                    ""type"": ""Value"",
                    ""id"": ""a7b46c3b-5d7c-450a-a7f5-c33ed7b72ad8"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Inventory"",
                    ""type"": ""Button"",
                    ""id"": ""a7289533-7395-46f2-b56e-2313d41dea27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Setting"",
                    ""type"": ""Button"",
                    ""id"": ""02f361aa-256e-47d4-b0c2-29387a5ccde3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RecipeBook"",
                    ""type"": ""Button"",
                    ""id"": ""60ae9654-7f99-48fd-b89a-ee3998e2d639"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SlotQuickMove"",
                    ""type"": ""Button"",
                    ""id"": ""5f5fc189-3d99-4baa-933c-6b1b570ec556"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0815a371-eff4-41d9-b269-460395c73a27"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""DialogueSkip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""968ac20b-0c18-444f-b343-b7b846044199"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""DialogueSkip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87e54dfa-80e8-4b12-ab7c-7f7a89acd7a5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""DialogueSkip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3fabb9e-feb2-4dd7-8ec4-03a075ff66f8"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""QuickSlotScroll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""704ec141-7102-4a1e-882f-690fbfb0bb88"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": ""Scale"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6dbc36ce-d465-4e36-b5d2-3d87c5cc13a5"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=2)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09f20a05-33ce-4341-bb50-9331879eaee6"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=3)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95939182-1efc-4f20-af9a-9c6f8f396d1b"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=4)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eca71a7b-d8c4-40eb-8c85-3f5f08d1f212"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=5)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7346549d-384b-4319-9cad-d0b6b8edd36c"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=6)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8ff45f52-1791-4dbf-9ccb-9481bc07264c"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=7)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b94cf763-ef70-41b5-8501-4585fdff443e"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=8)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""854ab4d9-1915-4a7a-aab1-723cb5225b73"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=9)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d3a889e-7eef-4f6f-a7ae-b178c480d8ba"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": ""Scale(factor=10)"",
                    ""groups"": """",
                    ""action"": ""QuickSlotScrollButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db6c3337-f47a-40c5-a447-c1fc1768b0b1"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Inventory"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84798a6b-ab02-48c3-b9c6-f9cd01b202c8"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""Setting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5911955e-52b7-402b-9cd0-12c7f660a41a"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""RecipeBook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""897a1926-8769-4446-bfde-844d9586bcd2"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""SlotQuickMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Minigame"",
            ""id"": ""0bb1c64c-1863-4e2f-9c29-72fae1679f85"",
            ""actions"": [
                {
                    ""name"": ""BakeryKeyPressed"",
                    ""type"": ""Button"",
                    ""id"": ""1474e479-e9c2-45bb-a1b3-06135685195a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BakeryOvenHit"",
                    ""type"": ""Button"",
                    ""id"": ""9e819dc6-52a2-44c5-94e7-967ad1e27063"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5767ed08-724c-433a-bc99-4032c84e331b"",
                    ""path"": ""<Keyboard>/#(F)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""BakeryKeyPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""446fc144-f5da-4740-89f4-47c7c8cdd614"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""WinPCScheme"",
                    ""action"": ""BakeryOvenHit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""WinPCScheme"",
            ""bindingGroup"": ""WinPCScheme"",
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
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        m_Player_Interaction = m_Player.FindAction("Interaction", throwIfNotFound: true);
        m_Player_UseItem = m_Player.FindAction("UseItem", throwIfNotFound: true);
        m_Player_Fishing = m_Player.FindAction("Fishing", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_DialogueSkip = m_UI.FindAction("DialogueSkip", throwIfNotFound: true);
        m_UI_QuickSlotScroll = m_UI.FindAction("QuickSlotScroll", throwIfNotFound: true);
        m_UI_QuickSlotScrollButton = m_UI.FindAction("QuickSlotScrollButton", throwIfNotFound: true);
        m_UI_Inventory = m_UI.FindAction("Inventory", throwIfNotFound: true);
        m_UI_Setting = m_UI.FindAction("Setting", throwIfNotFound: true);
        m_UI_RecipeBook = m_UI.FindAction("RecipeBook", throwIfNotFound: true);
        m_UI_SlotQuickMove = m_UI.FindAction("SlotQuickMove", throwIfNotFound: true);
        // Minigame
        m_Minigame = asset.FindActionMap("Minigame", throwIfNotFound: true);
        m_Minigame_BakeryKeyPressed = m_Minigame.FindAction("BakeryKeyPressed", throwIfNotFound: true);
        m_Minigame_BakeryOvenHit = m_Minigame.FindAction("BakeryOvenHit", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Sprint;
    private readonly InputAction m_Player_Interaction;
    private readonly InputAction m_Player_UseItem;
    private readonly InputAction m_Player_Fishing;
    public struct PlayerActions
    {
        private @DefaultKeymap m_Wrapper;
        public PlayerActions(@DefaultKeymap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
        public InputAction @Interaction => m_Wrapper.m_Player_Interaction;
        public InputAction @UseItem => m_Wrapper.m_Player_UseItem;
        public InputAction @Fishing => m_Wrapper.m_Player_Fishing;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @Interaction.started += instance.OnInteraction;
            @Interaction.performed += instance.OnInteraction;
            @Interaction.canceled += instance.OnInteraction;
            @UseItem.started += instance.OnUseItem;
            @UseItem.performed += instance.OnUseItem;
            @UseItem.canceled += instance.OnUseItem;
            @Fishing.started += instance.OnFishing;
            @Fishing.performed += instance.OnFishing;
            @Fishing.canceled += instance.OnFishing;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @Interaction.started -= instance.OnInteraction;
            @Interaction.performed -= instance.OnInteraction;
            @Interaction.canceled -= instance.OnInteraction;
            @UseItem.started -= instance.OnUseItem;
            @UseItem.performed -= instance.OnUseItem;
            @UseItem.canceled -= instance.OnUseItem;
            @Fishing.started -= instance.OnFishing;
            @Fishing.performed -= instance.OnFishing;
            @Fishing.canceled -= instance.OnFishing;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private List<IUIActions> m_UIActionsCallbackInterfaces = new List<IUIActions>();
    private readonly InputAction m_UI_DialogueSkip;
    private readonly InputAction m_UI_QuickSlotScroll;
    private readonly InputAction m_UI_QuickSlotScrollButton;
    private readonly InputAction m_UI_Inventory;
    private readonly InputAction m_UI_Setting;
    private readonly InputAction m_UI_RecipeBook;
    private readonly InputAction m_UI_SlotQuickMove;
    public struct UIActions
    {
        private @DefaultKeymap m_Wrapper;
        public UIActions(@DefaultKeymap wrapper) { m_Wrapper = wrapper; }
        public InputAction @DialogueSkip => m_Wrapper.m_UI_DialogueSkip;
        public InputAction @QuickSlotScroll => m_Wrapper.m_UI_QuickSlotScroll;
        public InputAction @QuickSlotScrollButton => m_Wrapper.m_UI_QuickSlotScrollButton;
        public InputAction @Inventory => m_Wrapper.m_UI_Inventory;
        public InputAction @Setting => m_Wrapper.m_UI_Setting;
        public InputAction @RecipeBook => m_Wrapper.m_UI_RecipeBook;
        public InputAction @SlotQuickMove => m_Wrapper.m_UI_SlotQuickMove;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void AddCallbacks(IUIActions instance)
        {
            if (instance == null || m_Wrapper.m_UIActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_UIActionsCallbackInterfaces.Add(instance);
            @DialogueSkip.started += instance.OnDialogueSkip;
            @DialogueSkip.performed += instance.OnDialogueSkip;
            @DialogueSkip.canceled += instance.OnDialogueSkip;
            @QuickSlotScroll.started += instance.OnQuickSlotScroll;
            @QuickSlotScroll.performed += instance.OnQuickSlotScroll;
            @QuickSlotScroll.canceled += instance.OnQuickSlotScroll;
            @QuickSlotScrollButton.started += instance.OnQuickSlotScrollButton;
            @QuickSlotScrollButton.performed += instance.OnQuickSlotScrollButton;
            @QuickSlotScrollButton.canceled += instance.OnQuickSlotScrollButton;
            @Inventory.started += instance.OnInventory;
            @Inventory.performed += instance.OnInventory;
            @Inventory.canceled += instance.OnInventory;
            @Setting.started += instance.OnSetting;
            @Setting.performed += instance.OnSetting;
            @Setting.canceled += instance.OnSetting;
            @RecipeBook.started += instance.OnRecipeBook;
            @RecipeBook.performed += instance.OnRecipeBook;
            @RecipeBook.canceled += instance.OnRecipeBook;
            @SlotQuickMove.started += instance.OnSlotQuickMove;
            @SlotQuickMove.performed += instance.OnSlotQuickMove;
            @SlotQuickMove.canceled += instance.OnSlotQuickMove;
        }

        private void UnregisterCallbacks(IUIActions instance)
        {
            @DialogueSkip.started -= instance.OnDialogueSkip;
            @DialogueSkip.performed -= instance.OnDialogueSkip;
            @DialogueSkip.canceled -= instance.OnDialogueSkip;
            @QuickSlotScroll.started -= instance.OnQuickSlotScroll;
            @QuickSlotScroll.performed -= instance.OnQuickSlotScroll;
            @QuickSlotScroll.canceled -= instance.OnQuickSlotScroll;
            @QuickSlotScrollButton.started -= instance.OnQuickSlotScrollButton;
            @QuickSlotScrollButton.performed -= instance.OnQuickSlotScrollButton;
            @QuickSlotScrollButton.canceled -= instance.OnQuickSlotScrollButton;
            @Inventory.started -= instance.OnInventory;
            @Inventory.performed -= instance.OnInventory;
            @Inventory.canceled -= instance.OnInventory;
            @Setting.started -= instance.OnSetting;
            @Setting.performed -= instance.OnSetting;
            @Setting.canceled -= instance.OnSetting;
            @RecipeBook.started -= instance.OnRecipeBook;
            @RecipeBook.performed -= instance.OnRecipeBook;
            @RecipeBook.canceled -= instance.OnRecipeBook;
            @SlotQuickMove.started -= instance.OnSlotQuickMove;
            @SlotQuickMove.performed -= instance.OnSlotQuickMove;
            @SlotQuickMove.canceled -= instance.OnSlotQuickMove;
        }

        public void RemoveCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IUIActions instance)
        {
            foreach (var item in m_Wrapper.m_UIActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_UIActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public UIActions @UI => new UIActions(this);

    // Minigame
    private readonly InputActionMap m_Minigame;
    private List<IMinigameActions> m_MinigameActionsCallbackInterfaces = new List<IMinigameActions>();
    private readonly InputAction m_Minigame_BakeryKeyPressed;
    private readonly InputAction m_Minigame_BakeryOvenHit;
    public struct MinigameActions
    {
        private @DefaultKeymap m_Wrapper;
        public MinigameActions(@DefaultKeymap wrapper) { m_Wrapper = wrapper; }
        public InputAction @BakeryKeyPressed => m_Wrapper.m_Minigame_BakeryKeyPressed;
        public InputAction @BakeryOvenHit => m_Wrapper.m_Minigame_BakeryOvenHit;
        public InputActionMap Get() { return m_Wrapper.m_Minigame; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MinigameActions set) { return set.Get(); }
        public void AddCallbacks(IMinigameActions instance)
        {
            if (instance == null || m_Wrapper.m_MinigameActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MinigameActionsCallbackInterfaces.Add(instance);
            @BakeryKeyPressed.started += instance.OnBakeryKeyPressed;
            @BakeryKeyPressed.performed += instance.OnBakeryKeyPressed;
            @BakeryKeyPressed.canceled += instance.OnBakeryKeyPressed;
            @BakeryOvenHit.started += instance.OnBakeryOvenHit;
            @BakeryOvenHit.performed += instance.OnBakeryOvenHit;
            @BakeryOvenHit.canceled += instance.OnBakeryOvenHit;
        }

        private void UnregisterCallbacks(IMinigameActions instance)
        {
            @BakeryKeyPressed.started -= instance.OnBakeryKeyPressed;
            @BakeryKeyPressed.performed -= instance.OnBakeryKeyPressed;
            @BakeryKeyPressed.canceled -= instance.OnBakeryKeyPressed;
            @BakeryOvenHit.started -= instance.OnBakeryOvenHit;
            @BakeryOvenHit.performed -= instance.OnBakeryOvenHit;
            @BakeryOvenHit.canceled -= instance.OnBakeryOvenHit;
        }

        public void RemoveCallbacks(IMinigameActions instance)
        {
            if (m_Wrapper.m_MinigameActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMinigameActions instance)
        {
            foreach (var item in m_Wrapper.m_MinigameActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MinigameActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MinigameActions @Minigame => new MinigameActions(this);
    private int m_WinPCSchemeSchemeIndex = -1;
    public InputControlScheme WinPCSchemeScheme
    {
        get
        {
            if (m_WinPCSchemeSchemeIndex == -1) m_WinPCSchemeSchemeIndex = asset.FindControlSchemeIndex("WinPCScheme");
            return asset.controlSchemes[m_WinPCSchemeSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnInteraction(InputAction.CallbackContext context);
        void OnUseItem(InputAction.CallbackContext context);
        void OnFishing(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnDialogueSkip(InputAction.CallbackContext context);
        void OnQuickSlotScroll(InputAction.CallbackContext context);
        void OnQuickSlotScrollButton(InputAction.CallbackContext context);
        void OnInventory(InputAction.CallbackContext context);
        void OnSetting(InputAction.CallbackContext context);
        void OnRecipeBook(InputAction.CallbackContext context);
        void OnSlotQuickMove(InputAction.CallbackContext context);
    }
    public interface IMinigameActions
    {
        void OnBakeryKeyPressed(InputAction.CallbackContext context);
        void OnBakeryOvenHit(InputAction.CallbackContext context);
    }
}
