using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseTargetState : InputState
{
    InterfaceManager interfaceManager;
    IStateSwitcher stateSwitcher;
    ChooseTargetManager targetManager;
    
    public ChooseTargetState(IStateSwitcher stateSwitcher, InterfaceManager interfaceManager, ChooseTargetManager targetManager){
        this.interfaceManager = interfaceManager;
        this.stateSwitcher = stateSwitcher;
        this.targetManager = targetManager;
    }

    public void PlayerMove(Vector2 inputVector, Player player) {
        if(inputVector.x != 0 || inputVector.y != 0) {
            player.Move(inputVector);//Передать Vector2
        } else {
            player.Stop();
        }
    }
    public void MouseClick(Player player) {
        if (Input.GetMouseButtonDown(0)){
            targetManager.BeginAction();
        }
    }
    public void HotBarUse(int id, Inventory inventory) {
        return;
    }
    public void CallMenu(KeyCode key) {
        if(key == KeyCode.Escape) {
            targetManager.ExitState();
        }
    }
    public void PlayerAction(KeyCode key, Player player) {
        return;
    }
}
