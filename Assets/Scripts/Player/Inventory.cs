using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<InventoryCell> cells;
    public List<Image> cellIcons;

    private int inventorySize;
    private Color cellColor;
    //private Regex numKeyCheck;

    /// <summary>
    /// ƒобавить список предметов, чтобы они уничтожались при использовании
    /// </summary>


    private void Start()
    {
        //numKeyCheck = new Regex(@"\d");
        inventorySize = 4;
        GetCellsIcons();
        ClearCells();
        cellColor = cellIcons[0].color;
    }

    private void Update()
    {
        UseCell();
    }

    private void GetCellsIcons()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            cellIcons.Add(GameObject.Find($"item_{i + 1}").GetComponent<Image>());
        }
    }

    private void AddItem(int idCell, int idItem, GameObject item, Sprite sprite)
    {
        cells[idCell].idItem = idItem;
        cells[idCell].item = item;
        cells[idCell].countItem++;
        cellIcons[idCell].sprite = sprite;
        cellIcons[idCell].color = Color.white;
        cellIcons[idCell].GetComponentInChildren<TextMeshProUGUI>().text = cells[idCell].countItem.ToString();
    }

    private void UseItem(int idCell)
    {
        cells[idCell].countItem--;
        cellIcons[idCell].GetComponentInChildren<TextMeshProUGUI>().text = cells[idCell].countItem.ToString();

        if (cells[idCell].countItem == 0)
        {
            cells[idCell].idItem = 0;
            cells[idCell].item = null;
            cellIcons[idCell].sprite = null;
            cellIcons[idCell].color = cellColor;
        }
    }

    public void GetItem(GameObject item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        int idItem = item.GetComponent<Item>().id;

        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].countItem == 0 || cells[i].idItem == idItem)
            {
                AddItem(i, idItem, item, sprite);
                break;
            }
        }

    }

    private void ClearCells()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            cells[i].countItem = 0;
            cells[i].item = null;
            cells[i].idItem = 0;
            cellIcons[i].sprite = null;
            cellIcons[i].color = cellColor;
        }
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
        else
        {
            return;
        }

        if (cells[cellInd].countItem > 0)
        {
            if (cells[cellInd].item.CompareTag("Potion"))
            {
                Potion potion = cells[cellInd].item.GetComponent<Potion>();
                potion.GetEffect();
                /*cells[cellInd].countItem--;

                if(cells[cellInd].countItem == 0)
                {
                    cells[cellInd].item = null;
                    cellIcons[cellInd].sprite = null;
                    cellIcons[cellInd].color = cellColor;

                }*/

                UseItem(cellInd);
            }
        }
    }


}
