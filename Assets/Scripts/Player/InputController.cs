using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        UseCell();
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
        else
        {
            return;
        }

        inventory.UseItem(cellInd);
    }
}
