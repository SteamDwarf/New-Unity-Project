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

    private bool contextMenuShowed = false;
    

    public void ShowContextMenu()
    {
        if (!contextMenuShowed)
        {
            contextMenu.SetActive(true);
            contextMenuShowed = true;
        }
        else
        {
            contextMenu.SetActive(false);
            contextMenuShowed = false;
        }
           
    }
}
