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
    private List<GameObject> cellInterface;
    private List<GameObject> cellInterfaceHB;
    [SerializeField] private List<InventoryCell> cellObject;
    [SerializeField] private List<InventoryCell> cellObjectHB;

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
        cellInterface = GameObject.FindGameObjectsWithTag("InventoryItem").ToList<GameObject>();
        cellInterfaceHB = GameObject.FindGameObjectsWithTag("HotbarItem").ToList<GameObject>();
        currentItem = GameObject.FindGameObjectWithTag("CurrentItem");
        currentItemImage = currentItem.GetComponent<Image>();
    }

    private void Start()
    {
        defaultCellColor = new Color(204, 153, 74, 0);
        currentItem.SetActive(false);
        ClearCells(cellInterface, cellObject);
        ClearCells(cellInterfaceHB, cellObjectHB);
        saveCell_1.Clear();
        saveCell_2.Clear();
    }

    private void ClearCells(List<GameObject> cellUI, List<InventoryCell> cellSO)
    {
        for (int i = 0; i < cellUI.Count; i++)
        {
            cellUI[i].GetComponent<Image>().color = defaultCellColor;
            cellUI[i].GetComponentInChildren<TextMeshProUGUI>().text = 0.ToString();
            cellSO[i].Clear();
        }
    }

    public void GetItem(GameObject item)
    {
        Sprite sprite = item.GetComponent<SpriteRenderer>().sprite;
        int idItem = item.GetComponent<Item>().id;
        List<GameObject> freeCellUI =  new List<GameObject>();
        List<InventoryCell> freeCellSO = new List<InventoryCell>();

        CellType cellType = CellType.inventory;
        int cellId = FindAppropriateCell(idItem, out cellType);

        if(cellId >= 0) {
            if(cellType == CellType.inventory) {
                freeCellUI = cellInterface;
                freeCellSO = cellObject;
            } else if(cellType == CellType.hotBar) {
                freeCellUI = cellInterfaceHB;
                freeCellSO = cellObjectHB;
            }
        } else {
            cellId = FindFreeCell(idItem, out cellType);

            if(cellId >= 0) {
                if(cellType == CellType.inventory) {
                    freeCellUI = cellInterface;
                    freeCellSO = cellObject;
                } else if(cellType == CellType.hotBar) {
                    freeCellUI = cellInterfaceHB;
                    freeCellSO = cellObjectHB;
                }
            }else {
                return;
            }
        }

        freeCellSO[cellId].SetItem(item);
        freeCellUI[cellId].GetComponent<Image>().sprite = sprite;
        freeCellUI[cellId].GetComponent<Image>().color = Color.white;
        freeCellUI[cellId].GetComponentInChildren<TextMeshProUGUI>().text = freeCellSO[cellId].countItem.ToString();

    }

    private int FindAppropriateCell(int idItem, out CellType cellType) {
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
    public void DecreaseItemNumber(int id) {
        int itemCount = cellObjectHB[id].DecreaseItemNumber();
        cellInterfaceHB[id].GetComponentInChildren<TextMeshProUGUI>().text = itemCount.ToString();

        if (itemCount == 0) {
            cellInterfaceHB[id].GetComponent<Image>().sprite = null;
            cellInterfaceHB[id].GetComponent<Image>().color = defaultCellColor;
        }       
    }

    public void UseItem() {
        if(cellObject[choosenCellId].item == null) {
            HideContextMenu();
            return;
        }

        if(cellObject[choosenCellId].item.CompareTag("Potion")){
            Potion potion = cellObject[choosenCellId].item.GetComponent<Potion>();
            potion.UseItem();

            DecreaseItemNumber();
        }

        HideContextMenu();
    }

    public void UseItem(int id) {
        if(cellObjectHB[id].item == null) {
            return;
        }
        
        if(cellObjectHB[id].item.CompareTag("Potion")){
            Potion potion = cellObjectHB[id].item.GetComponent<Potion>();
            potion.UseItem();

            DecreaseItemNumber(id);
        }
    }

    public void MoveItem(int id, CellType cellType) {
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
    }

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
