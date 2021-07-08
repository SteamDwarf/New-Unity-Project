using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuState : InputState
{
    InterfaceManager interfaceManager;
    IStateSwitcher stateSwitcher;
    public MenuState(IStateSwitcher stateSwitcher, InterfaceManager interfaceManager){
        this.interfaceManager = interfaceManager;
        this.stateSwitcher = stateSwitcher;
    }

/*     public  void Move(Vector2 vector, Player player) {
        return;
    } */
    public void MouseClick(Player player) {
        return;
    }
    public void HotBarUse(int id, Inventory inventory) {
        return;
    }
    public void CallMenu(KeyCode key) {
        if(key == KeyCode.Escape) {
            interfaceManager.ShowHidePauseMenu();
            stateSwitcher.SwitchState<InGameState>();
        }
    }
}
