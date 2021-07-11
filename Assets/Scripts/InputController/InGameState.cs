using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameState : InputState
{
    IStateSwitcher stateSwitcher;
    InterfaceManager interfaceManager;
    public InGameState(IStateSwitcher stateSwitcher, InterfaceManager interfaceManager){
        this.stateSwitcher = stateSwitcher;
        this.interfaceManager = interfaceManager;
    }
/*     public void Move(Vector2 vector, Player player) {
        player.Move(vector);//Передать Vector2
    } */
    public void MouseClick(Player player) {
        if (Input.GetMouseButtonDown(0)){
            player.Attack();
        }

        if (Input.GetMouseButton(1)) {
            player.Blocking();
        }

        if (Input.GetMouseButtonUp(1)){
            player.EndBlock();
        }
    }
    public void HotBarUse(int id, Inventory inventory) {
        inventory.UseItem(id);
    }
    public void CallMenu(KeyCode key) {
        if(key == KeyCode.Escape) {
            interfaceManager.ShowHidePauseMenu();
            stateSwitcher.SwitchState<MenuState>();
        }
        if (key == KeyCode.I){
            interfaceManager.ShowHideInventory();
            stateSwitcher.SwitchState<InventoryState>();
        }
    }
}
