using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputState 
{
    //void Move(Vector2 vector, Player player);
    void MouseClick(int mBtn, MousePressed mousePressed, Player player);
    void CallMenu(KeyCode key);
    void HotBarUse(int id, Inventory inventory);
}
