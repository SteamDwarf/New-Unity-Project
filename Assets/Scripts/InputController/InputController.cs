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
    public InputState currentState {get; private set;}
    private ChooseTargetManager targetManager;

    void Start()
    {
        interfaceManager = gm.GetComponent<InterfaceManager>();
        targetManager = GetComponent<ChooseTargetManager>();

        inputStates = new List<InputState>() {
            new MenuState(this, interfaceManager),
            new InGameState(this, interfaceManager),
            new InventoryState(this, interfaceManager),
            new InventoryItemInteractionState(this, interfaceManager),
            new ChooseTargetState(this, interfaceManager, targetManager)
        };

        currentState = inputStates[1];
    }

    // Update is called once per frame
    void Update()
    {
        UseCell();
        CallUI();
        MouseClick();
        PlayerAction();
    }
    private void FixedUpdate() {
        PlayerMove();
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

    private void MouseClick() {
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        currentState.MouseClick(player);
    }

    private void PlayerMove() {
        if(player == null) {
            return;
        }
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        currentState.PlayerMove(inputVector, player);
    }

    private void PlayerAction() {
        if(Input.GetKeyDown(KeyCode.F)) {
            currentState.PlayerAction(KeyCode.F, player);
        }
    }

    public void SwitchState<T>() where T : InputState {
        InputState state = inputStates.FirstOrDefault(s => s is T);
        currentState = state;
        Debug.Log(currentState);
    }
}
