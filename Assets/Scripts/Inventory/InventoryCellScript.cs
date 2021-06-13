using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCellScript : MonoBehaviour
{
    [SerializeField] private GameObject contextMenu;
    [SerializeField] private GameObject useBtn;
    [SerializeField] private GameObject descriptionBtn;
    [SerializeField] private GameObject dropBtn;
    [SerializeField] private int cellId;

    private Inventory inventory;
    private bool contextMenuShowed = false;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
    }

    public void ShowContextMenu()
    {
        inventory.ShowContextMenu(cellId);
    }
}
