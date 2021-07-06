using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> inventoryCell;

    public List<GameObject> GetInventoryCell() {
        return inventoryCell;
    }
}
