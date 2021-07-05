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

    private List<GameObject> cellUI;
    [SerializeField] private List<InventoryCell> cellSO;

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
    private int copiedCellID = -1;
    private CellType copiedCellType;
    private GameObject descriptionMenu;
    private Image itemDescImage;
    private TextMeshProUGUI itemDescName;
    private TextMeshProUGUI itemDescDescription;
    private bool inventoryBlocked;
    

    private void Awake()
    {
        cellUI = GameObject.FindGameObjectsWithTag("InventoryItem").ToList<GameObject>();
        currentItem = GameObject.FindGameObjectWithTag("CurrentItem");
        currentItemImage = currentItem.GetComponent<Image>();
        descriptionMenu = GameObject.FindGameObjectWithTag("ItemInteractMenu");
        itemDescImage = GameObject.FindGameObjectWithTag("ItemSprite").GetComponent<Image>();
        itemDescName = GameObject.FindGameObjectWithTag("ItemName").GetComponent<TextMeshProUGUI>();
        itemDescDescription = GameObject.FindGameObjectWithTag("ItemDescription").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        defaultCellColor = new Color(204, 153, 74, 0);
        currentItem.SetActive(false);
        descriptionMenu.SetActive(false);
        ClearCells();
        saveCell_1.Clear();
        saveCell_2.Clear();
        inventoryBlocked = true;
    }

    private void ClearCells()
    {
        for (int i = 0; i < cellUI.Count; i++){
            cellUI[i].GetComponent<Image>().color = defaultCellColor;
            cellUI[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            cellSO[i].Clear();
        }
    }

    public void GetItem(GameObject item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        int idItem = item.GetComponent<Item>().id;
        int cellId = FindAppropriateCell(idItem);

        if(cellId == -1) {
            cellId = FindFreeCell();
        }
        if(cellId == -1) {
            return;
        }

        cellSO[cellId].SetItem(item);
        cellUI[cellId].GetComponent<Image>().sprite = sprite;
        cellUI[cellId].GetComponent<Image>().color = Color.white;
        cellUI[cellId].GetComponentInChildren<TextMeshProUGUI>().text = cellSO[cellId].countItem.ToString();

    }

    private int FindAppropriateCell(int idItem) {
        int cellId = -1;

        for(int i = 0; i < cellSO.Count; i++) {
            if(cellSO[i].idItem == idItem) {
                cellId = i;
                break;
            }
        }

        return cellId;
    }

    private int FindFreeCell() {
        int cellId = -1;

        for (int i = 0; i < cellSO.Count; i++) {
            if (cellSO[i].countItem == 0) {
                cellId = i;
                break;
            }
        }

        return cellId;
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

    public void HideContextMenu()
    {
        descriptionMenu.SetActive(false);
        choosenCellId = -1;
        inventoryBlocked = false;
    }
/*     public void HideContextMenu()
    {
        Destroy(instantiatedContextMenu);
        choosenCellId = -1;
    } */

/*     public void CreateContextMenu(int id, Vector2 position) {
        if(cellSO[id].item == null) {
            return;
        }

        if(instantiatedContextMenu != null) {
            Destroy(instantiatedContextMenu);
        }
        instantiatedContextMenu = Instantiate(contextMenu, position, Quaternion.identity, cellUI[id].transform);
        instantiatedContextMenu.SetActive(true);
        choosenCellId = id;
    } */

    public void ShowContextMenu(int id) {
        if(inventoryBlocked) {
            return;
        }
        if(cellSO[id].item == null) {
            return;
        }

        choosenCellId = id;
        if(cellSO[choosenCellId].item != null){
            itemDescImage.sprite = cellSO[choosenCellId].sprite;
            itemDescName.text = cellSO[choosenCellId].itemName;
            itemDescDescription.text = cellSO[choosenCellId].description;
            descriptionMenu.SetActive(true);
            inventoryBlocked = true;
        }
    }

    public void DecreaseItemNumber() {
        int itemCount = cellSO[choosenCellId].DecreaseItemNumber();
        cellUI[choosenCellId].GetComponentInChildren<TextMeshProUGUI>().text = itemCount.ToString();

        if (itemCount == 0) {
            cellUI[choosenCellId].GetComponent<Image>().sprite = null;
            cellUI[choosenCellId].GetComponent<Image>().color = defaultCellColor;
        }       
    }
    public void DecreaseItemNumber(int id) {
        int itemCount = cellSO[id].DecreaseItemNumber();
        cellUI[id].GetComponentInChildren<TextMeshProUGUI>().text = itemCount.ToString();

        if (itemCount == 0) {
            cellUI[id].GetComponent<Image>().sprite = null;
            cellUI[id].GetComponent<Image>().color = defaultCellColor;
        }       
    }

    public void UseItem() {
        if(cellSO[choosenCellId].item == null) {
            HideContextMenu();
            return;
        }

        if(cellSO[choosenCellId].item.GetComponent<Item>() != null){
            Item item = cellSO[choosenCellId].item.GetComponent<Item>();
            item.UseItem();

            DecreaseItemNumber();
        }

        HideContextMenu();
    }

// TODO: Когда открыт инвентарь цифры не работают

    public void UseItem(int id) {
        if(!inventoryBlocked) {
            return;
        }
        if(cellSO[id].item == null) {
            return;
        }
        
        if(cellSO[id].item.GetComponent<Item>() != null){
            Item item = cellSO[id].item.GetComponent<Item>();
            item.UseItem();

            DecreaseItemNumber(id);
        }
    }

    public void MoveItem(int id) {
        if(inventoryBlocked) {
            return;
        }

        if(cellSO[id].item == null && saveCell_1.item == null) {
            return;
        }

        if(saveCell_1.item == null) {
            saveCell_1 = cellSO[id].Clone();
            copiedCellID = id;
            ShowCurrentItem(saveCell_1.sprite);

        } else if(saveCell_1.item != null && cellSO[id].item != null) {
            saveCell_2 = cellSO[id].Clone();

            cellSO[id] = saveCell_1.Clone();
            cellUI[id].GetComponent<Image>().sprite = saveCell_1.sprite;
            cellUI[id].GetComponent<Image>().color = Color.white;
            cellUI[id].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_1.countItem.ToString();

            cellSO[copiedCellID] = saveCell_2.Clone();
            cellUI[copiedCellID].GetComponent<Image>().sprite = saveCell_2.sprite;
            cellUI[copiedCellID].GetComponent<Image>().color = Color.white;
            cellUI[copiedCellID].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_2.countItem.ToString();

            saveCell_1.Clear();
            saveCell_2.Clear();
            copiedCellID = -1;
            HideCurrentItem();

        } else if(saveCell_1.item != null && cellSO[id].item == null) {
            cellSO[id] = saveCell_1.Clone();

            cellUI[id].GetComponent<Image>().sprite = saveCell_1.sprite;
            cellUI[id].GetComponent<Image>().color = Color.white;
            cellUI[id].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_1.countItem.ToString();

            cellSO[copiedCellID].Clear();
            cellUI[copiedCellID].GetComponent<Image>().sprite = null;
            cellUI[copiedCellID].GetComponent<Image>().color = defaultCellColor;
            cellUI[copiedCellID].GetComponentInChildren<TextMeshProUGUI>().text = "0";

            saveCell_1.Clear();
            copiedCellID = -1;
            HideCurrentItem();
        }
    }

    public void OpenMenu() {
        inventoryBlocked = false;
    }

    public void CloseMenu() {
        HideContextMenu();
        HideCurrentItem();
        saveCell_1.Clear();
        saveCell_2.Clear();
        copiedCellID = -1;
        inventoryBlocked = true;
    }

    public void ShowItemDescription() {
        if(cellSO[choosenCellId].item == null) {
            HideContextMenu();
            return;
        }

        if(cellSO[choosenCellId].item != null){
            itemDescImage.sprite = cellSO[choosenCellId].sprite;
            itemDescName.text = cellSO[choosenCellId].itemName;
            itemDescDescription.text = cellSO[choosenCellId].description;
            descriptionMenu.SetActive(true);
        }

        HideContextMenu();
    }

    public void DropItem() {
        if(cellSO[choosenCellId].item == null && saveCell_1.item == null) {
            HideContextMenu();
            return;
        }

        GameObject newItem = ItemPrefabBuilder.BuildPotionPrefab(cellSO[choosenCellId].item, 5, cellSO[choosenCellId].sprite);
        newItem.GetComponent<Item>().DropItem();
        newItem.transform.position = new Vector3(transform.position.x + 5, transform.position.y + 5, transform.position.z);

        HideContextMenu();
    }
}
