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
    public void MouseClick(int i, MousePressed mousePressed, Player player) {
        player.Combat(); //Передать Input
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
