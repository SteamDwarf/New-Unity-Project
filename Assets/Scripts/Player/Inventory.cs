using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<InventoryCell> cells;
    public List<Image> cellIcons;
    private int inventorySize;
    //private Regex numKeyCheck;

    void Start()
    {
        //numKeyCheck = new Regex(@"\d");
        ClearCells();
        inventorySize = 4;
        GetCellsIcons();
    }

    private void Update()
    {
        UseCell();
    }

    public void GetItem(Item item)
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if(cells[i].isEmpty)
            {
                cells[i].item = item;
                cells[i].isEmpty = false;
                cells[i].sprite = item.sprite;
                cellIcons[i].sprite = item.sprite;
                cellIcons[i].color = Color.white;
                break;
            }
        }
    }

    private void ClearCells()
    {
        Debug.Log(cellIcons.Count);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].isEmpty = true;
            cells[i].item = null;
            //cellIcons[i].sprite = null;
        }
    }

    private void UseCell()
    {
        if(Input.GetKeyDown("1"))
        {
            if(!cells[0].isEmpty)
            {
                if(cells[0].item.CompareTag("Potion"))
                {
                    Potion potion = cells[0].item.GetComponent<Potion>();
                    potion.GetEffect();
                    
                }
            }
        }
    }

    private void GetCellsIcons()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            cellIcons.Add(GameObject.Find($"item_{i + 1}").GetComponent<Image>());
        }
    }
}
