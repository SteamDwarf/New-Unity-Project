using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputState 
{
    //void Move(Vector2 vector, Player player);
    void MouseClick(Player player);
    void CallMenu(KeyCode key);
    void HotBarUse(int id, Inventory inventory);
    void PlayerMove(Vector2 inputVector, Player player);
    void PlayerAction(KeyCode key, Player player);
}
