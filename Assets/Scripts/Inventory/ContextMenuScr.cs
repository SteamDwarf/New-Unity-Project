using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextMenuScr : MonoBehaviour
{
    private Inventory inventory;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }

    public void Use()
    {
        inventory.UpdateItemInformation();
    }
}
