using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Inventory : MonoBehaviour, IMenu
{
    public List<GameObject> cellInterface;
    public List<InventoryCell> cellObject;

    [SerializeField] private GameObject contextMenu;
    [SerializeField] private InventoryCell saveCell_1;
    [SerializeField] private InventoryCell saveCell_2;
    [SerializeField] private GameObject cellCursorImage;

    private int choosenCellId = -1; 
    private GameObject currentItem;
    private Image currentItemImage;
    private GameObject instantiatedContextMenu;
    private GameObject instantiatedCursorImage;
    private Color defaultCellColor;
    private int copiedCell = -1;

    private void Awake()
    {
        cellInterface = GameObject.FindGameObjectsWithTag("InventoryItem").ToList<GameObject>();
        currentItem = GameObject.FindGameObjectWithTag("CurrentItem");
        currentItemImage = currentItem.GetComponent<Image>();
    }

    private void Start()
    {
        defaultCellColor = new Color(204, 153, 74, 0);
        currentItem.SetActive(false);
        ClearCells();
    }

    private void ClearCells()
    {
        for (int i = 0; i < cellInterface.Count; i++)
        {
            cellInterface[i].GetComponent<Image>().color = defaultCellColor;
            cellInterface[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            cellObject[i].Clear();
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
                cellObject[i].SetItem(item);
                cellInterface[i].GetComponent<Image>().sprite = sprite;
                cellInterface[i].GetComponent<Image>().color = Color.white;
                cellInterface[i].GetComponentInChildren<TextMeshProUGUI>().text = cellObject[i].countItem.ToString();
                //AddItem(i, idItem, item, sprite);
                break;
            }
        }

    }

/*     private void AddItem(int idCell, int idItem, GameObject item, Sprite sprite)
    {
        cellObject[idCell].idItem = idItem;
        cellObject[idCell].item = item;
        cellObject[idCell].countItem++;
        cellObject[idCell].sprite = sprite;
        cellInterface[idCell].GetComponent<Image>().sprite = sprite;
        cellInterface[idCell].GetComponent<Image>().color = Color.white;
        cellInterface[idCell].GetComponentInChildren<TextMeshProUGUI>().text = cellObject[idCell].countItem.ToString();
    } */


    public void HideContextMenu()
    {
        Destroy(instantiatedContextMenu);
        choosenCellId = -1;
    }

    public void CreateContextMenu(int id, Vector2 position) {
        if(cellObject[id].item == null) {
            return;
        }

        if(instantiatedContextMenu != null) {
            Destroy(instantiatedContextMenu);
        }
        instantiatedContextMenu = Instantiate(contextMenu, position, Quaternion.identity, cellInterface[id].transform);
        instantiatedContextMenu.SetActive(true);
        choosenCellId = id;
    }

    public void DecreaseItemNumber() {
        int itemCount = cellObject[choosenCellId].DecreaseItemNumber();
        cellInterface[choosenCellId].GetComponentInChildren<TextMeshProUGUI>().text = itemCount.ToString();

        if (itemCount == 0) {
            cellInterface[choosenCellId].GetComponent<Image>().sprite = null;
            cellInterface[choosenCellId].GetComponent<Image>().color = defaultCellColor;
        }       
    }

    public void UseItem() {
        
        if(cellObject[choosenCellId].item.CompareTag("Potion")){
            Potion potion = cellObject[choosenCellId].item.GetComponent<Potion>();
            potion.UseItem();

            DecreaseItemNumber();
        }

        HideContextMenu();
    }

    public void MoveItem(int id) {
        if(cellObject[id].item == null && saveCell_1 == null) {
            return;
        }

        if(saveCell_1.item == null) {
            saveCell_1 = cellObject[id].Clone();
            copiedCell = id;
            ShowCurrentItem(saveCell_1.sprite);

        } else if(saveCell_1.item != null && cellObject[id].item != null) {

            saveCell_2 = cellObject[id].Clone();
            cellObject[id] = saveCell_1.Clone();
            cellObject[copiedCell] = saveCell_2.Clone();

            cellInterface[id].GetComponent<Image>().sprite = saveCell_1.sprite;
            cellInterface[copiedCell].GetComponent<Image>().sprite = saveCell_2.sprite;
            cellInterface[id].GetComponent<Image>().color = Color.white;
            cellInterface[copiedCell].GetComponent<Image>().color = Color.white;
            cellInterface[id].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_1.countItem.ToString();
            cellInterface[copiedCell].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_2.countItem.ToString();

            saveCell_1.Clear();
            saveCell_2.Clear();
            copiedCell = -1;
            HideCurrentItem();

        } else if(saveCell_1.item != null && cellObject[id].item == null) {
            cellObject[id] = saveCell_1.Clone();

            cellInterface[id].GetComponent<Image>().sprite = saveCell_1.sprite;
            cellInterface[id].GetComponent<Image>().color = Color.white;
            cellInterface[id].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_1.countItem.ToString();

            cellObject[copiedCell].Clear();
            cellInterface[copiedCell].GetComponent<Image>().sprite = null;
            cellInterface[copiedCell].GetComponent<Image>().color = defaultCellColor;
            cellInterface[copiedCell].GetComponentInChildren<TextMeshProUGUI>().text = "0";

            saveCell_1.Clear();
            copiedCell = -1;
            HideCurrentItem();
        }
    }

    public void CloseMenu() {
        HideContextMenu();
        HideCurrentItem();
        saveCell_1.Clear();
        saveCell_2.Clear();
        copiedCell = -1;
    }

    private void ShowCurrentItem(Sprite sprite) {
        Vector4 color = new Vector4(255,255,255,1);
        currentItem.SetActive(true);
        currentItemImage.sprite = sprite;
        currentItemImage.color = color;
    }
    private void HideCurrentItem() {
        Vector4 color = new Vector4(255,255,255,0);
        currentItemImage.sprite = null;
        currentItemImage.color = color;
        currentItem.SetActive(false);
    }
}
