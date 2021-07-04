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
/*     private List<GameObject> cellInterface;
    private List<GameObject> cellInterfaceHB; *//* 
    [SerializeField] private List<InventoryCell> cellObject;
    [SerializeField] private List<InventoryCell> cellObjectHB; */

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

    private void Awake()
    {
        cellUI = GameObject.FindGameObjectsWithTag("InventoryItem").ToList<GameObject>();
/*         cellInterface = GameObject.FindGameObjectsWithTag("InventoryItem").ToList<GameObject>();
        cellInterfaceHB = GameObject.FindGameObjectsWithTag("HotbarItem").ToList<GameObject>(); */
        currentItem = GameObject.FindGameObjectWithTag("CurrentItem");
        currentItemImage = currentItem.GetComponent<Image>();
    }

    private void Start()
    {
        defaultCellColor = new Color(204, 153, 74, 0);
        currentItem.SetActive(false);
        ClearCells();
/*         ClearCells(cellInterface, cellObject);
        ClearCells(cellInterfaceHB, cellObjectHB); */
        saveCell_1.Clear();
        saveCell_2.Clear();
    }

    private void ClearCells()
    {
        for (int i = 0; i < cellUI.Count; i++){
            cellUI[i].GetComponent<Image>().color = defaultCellColor;
            cellUI[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            cellSO[i].Clear();
        }
    }
/*     private void ClearCells(List<GameObject> cellUI, List<InventoryCell> cellSO)
    {
        for (int i = 0; i < cellUI.Count; i++)
        {
            cellUI[i].GetComponent<Image>().color = defaultCellColor;
            cellUI[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            cellSO[i].Clear();
        }
    } */

    public void GetItem(GameObject item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        int idItem = item.GetComponent<Item>().id;
/*         List<GameObject> freeCellUI =  new List<GameObject>();
        List<InventoryCell> freeCellSO = new List<InventoryCell>(); */

        //CellType cellType = CellType.inventory;
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
    /* private int FindAppropriateCell(int idItem, out CellType cellType) {
        int cellId = -1;
        cellType = CellType.inventory;

        for(int i = 0; i < cellObjectHB.Count; i++) {
            if(cellObjectHB[i].idItem == idItem) {
                cellId = i;
                cellType = CellType.hotBar;
                break;
            }
        }

        if(cellId == -1) {
            for(int i = 0; i < cellObject.Count; i++) {
                if(cellObject[i].idItem == idItem) {
                    cellId = i;
                    cellType = CellType.inventory;
                    break;
                }
            }
        }

        return cellId;
    }

    private int FindFreeCell(int idItem, out CellType cellType) {
        int cellId = -1;
        cellType = CellType.inventory;

        for (int i = 0; i < cellObjectHB.Count; i++) {
            if (cellObjectHB[i].countItem == 0) {
                cellId = i;
                cellType = CellType.hotBar;
                break;
            }
        }

        if(cellId == -1) {
            for(int i = 0; i < cellObject.Count; i++) {
                if(cellObject[i].idItem == idItem) {
                    cellId = i;
                    cellType = CellType.inventory;
                    break;
                }
            }
        }

        return cellId;
    } */

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
        Destroy(instantiatedContextMenu);
        choosenCellId = -1;
    }

    public void CreateContextMenu(int id, Vector2 position) {
        if(cellSO[id].item == null) {
            return;
        }

        if(instantiatedContextMenu != null) {
            Destroy(instantiatedContextMenu);
        }
        instantiatedContextMenu = Instantiate(contextMenu, position, Quaternion.identity, cellUI[id].transform);
        instantiatedContextMenu.SetActive(true);
        choosenCellId = id;
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

        if(cellSO[choosenCellId].item.CompareTag("Potion")){
            Potion potion = cellSO[choosenCellId].item.GetComponent<Potion>();
            potion.UseItem();

            DecreaseItemNumber();
        }

        HideContextMenu();
    }

    public void UseItem(int id) {
        if(cellSO[id].item == null) {
            return;
        }
        
        if(cellSO[id].item.CompareTag("Potion")){
            Potion potion = cellSO[id].item.GetComponent<Potion>();
            potion.UseItem();

            DecreaseItemNumber(id);
        }
    }

    public void MoveItem(int id) {

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
    /* public void MoveItem(int id, CellType cellType) {
        List<GameObject> choosenCellsUI = new List<GameObject>();
        List<InventoryCell> choosenCellsSO = new List<InventoryCell>();

        if(cellType == CellType.inventory) {
            choosenCellsUI = cellInterface;
            choosenCellsSO = cellObject;
        } else if(cellType == CellType.hotBar) {
            choosenCellsUI = cellInterfaceHB;
            choosenCellsSO = cellObjectHB;
        }

        if(choosenCellsSO[id].item == null && saveCell_1.item == null) {
            return;
        }

        if(saveCell_1.item == null) {
            saveCell_1 = choosenCellsSO[id].Clone();
            copiedCellID = id;
            copiedCellType = cellType;
            ShowCurrentItem(saveCell_1.sprite);

        } else if(saveCell_1.item != null && choosenCellsSO[id].item != null) {
            List<GameObject> copiedCellsUI = new List<GameObject>();
            List<InventoryCell> copiedCellsSO = new List<InventoryCell>();

            if(copiedCellType == CellType.inventory) {
                copiedCellsUI = cellInterface;
                copiedCellsSO = cellObject;
            }else if(copiedCellType == CellType.hotBar) {
                copiedCellsUI = cellInterfaceHB;
                copiedCellsSO = cellObjectHB;
            }

            saveCell_2 = choosenCellsSO[id].Clone();

            choosenCellsSO[id] = saveCell_1.Clone();
            choosenCellsUI[id].GetComponent<Image>().sprite = saveCell_1.sprite;
            choosenCellsUI[id].GetComponent<Image>().color = Color.white;
            choosenCellsUI[id].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_1.countItem.ToString();

            copiedCellsSO[copiedCellID] = saveCell_2.Clone();
            copiedCellsUI[copiedCellID].GetComponent<Image>().sprite = saveCell_2.sprite;
            copiedCellsUI[copiedCellID].GetComponent<Image>().color = Color.white;
            copiedCellsUI[copiedCellID].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_2.countItem.ToString();

            saveCell_1.Clear();
            saveCell_2.Clear();
            copiedCellID = -1;
            HideCurrentItem();

        } else if(saveCell_1.item != null && choosenCellsSO[id].item == null) {
            List<GameObject> copiedCellsUI = new List<GameObject>();
            List<InventoryCell> copiedCellsSO = new List<InventoryCell>();

            if(copiedCellType == CellType.inventory) {
                copiedCellsUI = cellInterface;
                copiedCellsSO = cellObject;
            }else if(copiedCellType == CellType.hotBar) {
                copiedCellsUI = cellInterfaceHB;
                copiedCellsSO = cellObjectHB;
            }

            choosenCellsSO[id] = saveCell_1.Clone();

            choosenCellsUI[id].GetComponent<Image>().sprite = saveCell_1.sprite;
            choosenCellsUI[id].GetComponent<Image>().color = Color.white;
            choosenCellsUI[id].GetComponentInChildren<TextMeshProUGUI>().text = saveCell_1.countItem.ToString();

            copiedCellsSO[copiedCellID].Clear();
            copiedCellsUI[copiedCellID].GetComponent<Image>().sprite = null;
            copiedCellsUI[copiedCellID].GetComponent<Image>().color = defaultCellColor;
            copiedCellsUI[copiedCellID].GetComponentInChildren<TextMeshProUGUI>().text = "0";

            saveCell_1.Clear();
            copiedCellID = -1;
            HideCurrentItem();
        }
    } */

    public void CloseMenu() {
        HideContextMenu();
        HideCurrentItem();
        saveCell_1.Clear();
        saveCell_2.Clear();
        copiedCellID = -1;
    }

    public void ShowItemDescription() {

    }
}
