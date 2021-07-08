using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputController : MonoBehaviour, IStateSwitcher
{
    [SerializeField] private GameObject gm;
    private Inventory inventory;
    private InterfaceManager interfaceManager;
    private Player player;
    private List<InputState> inputStates;
    private InputState currentState;

    void Start()
    {
        interfaceManager = gm.GetComponent<InterfaceManager>();

        inputStates = new List<InputState>() {
            new MenuState(this, interfaceManager),
            new InGameState(this, interfaceManager),
            new InventoryState(this, interfaceManager),
            new InventoryItemInteractionState(this, interfaceManager)
        };

        currentState = inputStates[1];
    }

    // Update is called once per frame
    void Update()
    {
        UseCell();
        CallUI();
    }

    private void UseCell()
    {
        int cellInd;

        if (Input.GetKeyDown("1"))
            cellInd = 0;
        else if (Input.GetKeyDown("2"))
            cellInd = 1;
        else if (Input.GetKeyDown("3"))
            cellInd = 2;
        else if (Input.GetKeyDown("4"))
            cellInd = 3;
        else if (Input.GetKeyDown("5"))
            cellInd = 4;
        else{
            return;
        }

        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        currentState.HotBarUse(cellInd, inventory);
    }

    private void CallUI() {
        if (Input.GetKeyDown(KeyCode.I)){
            currentState.CallMenu(KeyCode.I);
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            currentState.CallMenu(KeyCode.Escape);
        }
    }

    public void SwitchState<T>() where T : InputState {
        InputState state = inputStates.FirstOrDefault(s => s is T);
        currentState = state;
        Debug.Log(currentState);
    }
}
