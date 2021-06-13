using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public List<GameObject> inventoryItemInterface;
    public List<InventoryCell> inventoryItemsObj;
    private Color defaultCellColor;

    private void Awake()
    {
        inventoryItemInterface = new List<GameObject>(GameObject.FindGameObjectsWithTag("InventoryItem"));
        defaultCellColor = new Color(204, 153, 74, 0);
        ClearCells();
    }

    private void Start()
    {

    }

    private void Update()
    {
        /*itemImage.color = Color.red;
        countItemText.text = 9.ToString();*/
    }

    private void ClearCells()
    {
        for (int i = 0; i < inventoryItemInterface.Count; i++)
        {
            inventoryItemInterface[i].GetComponent<Image>().color = defaultCellColor;
            inventoryItemInterface[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            inventoryItemsObj[i].countItem = 0;
            inventoryItemsObj[i].description = "";
            inventoryItemsObj[i].idItem = 0;
            inventoryItemsObj[i].item = null;
            inventoryItemsObj[i].itemName = "";
            inventoryItemsObj[i].sprite = null;
        }
    }

    public void GetItem(GameObject item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        int idItem = item.GetComponent<Item>().id;

        for (int i = 0; i < inventoryItemsObj.Count; i++)
        {
            if (inventoryItemsObj[i].countItem == 0 || inventoryItemsObj[i].idItem == idItem)
            {
                AddItem(i, idItem, item, sprite);
                break;
            }
        }

    }

    private void AddItem(int idCell, int idItem, GameObject item, Sprite sprite)
    {
        inventoryItemsObj[idCell].idItem = idItem;
        inventoryItemsObj[idCell].item = item;
        inventoryItemsObj[idCell].countItem++;
        inventoryItemInterface[idCell].GetComponent<Image>().sprite = sprite;
        inventoryItemInterface[idCell].GetComponent<Image>().color = Color.white;
        inventoryItemInterface[idCell].GetComponentInChildren<TextMeshProUGUI>().text = inventoryItemsObj[idCell].countItem.ToString();
    }

}
