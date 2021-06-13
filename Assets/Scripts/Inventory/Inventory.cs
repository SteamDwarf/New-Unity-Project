using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    public List<GameObject> cellInterface;
    public List<InventoryCell> cellObject;
    public List<GameObject> contextMenus;

    private Color defaultCellColor;
    private int showedContextmenu = -1;

    //��� ������� ��������� ��� ����������� ������ ������ �����������


    private void Awake()
    {
        //inventoryItemInterface = new List<GameObject>(GameObject.FindGameObjectsWithTag("InventoryItem"));

    }

    private void Start()
    {
        defaultCellColor = new Color(204, 153, 74, 0);
        ClearCells();
    }

    private void Update()
    {
        /*itemImage.color = Color.red;
        countItemText.text = 9.ToString();*/
    }

    private void ClearCells()
    {
        for (int i = 0; i < cellInterface.Count; i++)
        {
            cellInterface[i].GetComponent<Image>().color = defaultCellColor;
            cellInterface[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            cellObject[i].countItem = 0;
            cellObject[i].description = "";
            cellObject[i].idItem = 0;
            cellObject[i].item = null;
            cellObject[i].itemName = "";
            cellObject[i].sprite = null;
        }
    }

    public void GetItem(GameObject item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        int idItem = item.GetComponent<Item>().id;

        for (int i = 0; i < cellObject.Count; i++)
        {
            if (cellObject[i].countItem == 0 || cellObject[i].idItem == idItem)
            {
                AddItem(i, idItem, item, sprite);
                break;
            }
        }

    }

    private void AddItem(int idCell, int idItem, GameObject item, Sprite sprite)
    {
        cellObject[idCell].idItem = idItem;
        cellObject[idCell].item = item;
        cellObject[idCell].countItem++;
        cellInterface[idCell].GetComponent<Image>().sprite = sprite;
        cellInterface[idCell].GetComponent<Image>().color = Color.white;
        cellInterface[idCell].GetComponentInChildren<TextMeshProUGUI>().text = cellObject[idCell].countItem.ToString();
    }

    public void ShowContextMenu(int id)
    {
        if (cellObject[id].item == null)
            return;

        if(!contextMenus[id].activeInHierarchy)
        {
            contextMenus[id].SetActive(true);
            showedContextmenu = id;
        } 
        else
        {
            contextMenus[id].SetActive(false);
            showedContextmenu = -1;
        }
            

        for (int i = 0; i < contextMenus.Count; i++)
        {
            if (i != id)
            {
                contextMenus[i].SetActive(false);
            }
        }
    }

    public void HideContextMenu()
    {
        for (int i = 0; i < contextMenus.Count; i++)
        {
            contextMenus[i].SetActive(false);
        }

        showedContextmenu = -1;
    }

    public void HideContextMenu(int id)
    {
        contextMenus[id].SetActive(false); 
        showedContextmenu = -1;
    }

    public void UpdateItemInformation()
    {
        if(showedContextmenu == -1)
            return;

        UseItem(showedContextmenu);

        cellObject[showedContextmenu].countItem--;
        cellInterface[showedContextmenu].GetComponentInChildren<TextMeshProUGUI>().text = cellObject[showedContextmenu].countItem.ToString();

        if (cellObject[showedContextmenu].countItem == 0)
        {
            cellObject[showedContextmenu].idItem = 0;
            cellObject[showedContextmenu].item = null;
            cellInterface[showedContextmenu].GetComponent<Image>().sprite = null;
            cellInterface[showedContextmenu].GetComponent<Image>().color = defaultCellColor;
        }

       
        HideContextMenu(showedContextmenu);

    }

    private void UseItem(int id)
    {
        if(cellObject[id].item.CompareTag("Potion"))
        {
            Potion potion = cellObject[id].item.GetComponent<Potion>();
            potion.GetEffect();
        }
    }
}
