using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : InputState
{
    InterfaceManager interfaceManager;
    IStateSwitcher stateSwitcher;
    public InventoryState(IStateSwitcher stateSwitcher, InterfaceManager interfaceManager) {
         this.interfaceManager = interfaceManager;
         this.stateSwitcher = stateSwitcher;
     }

/*      public void Move(Vector2 vector, Player player) {
        return;
    } */
    public void MouseClick(int mBtn, MousePressed mousePressed, Player player) {
        return;
    }
    public void HotBarUse(int id, Inventory inventory) {
        return;
    }
    public void CallMenu(KeyCode key) {
        if(key == KeyCode.Escape) {
            interfaceManager.ShowHideInventory();
            stateSwitcher.SwitchState<InGameState>();
        }
        if(key == KeyCode.I) {
            interfaceManager.ShowHideInventory();
            stateSwitcher.SwitchState<InGameState>();
        }
    }
    
}
