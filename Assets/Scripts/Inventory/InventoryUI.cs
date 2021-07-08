using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> inventoryCell;
    [SerializeField] private GameObject inputControllerGO;
    [SerializeField] private GameObject gameManagerGO;
    private InterfaceManager interfaceManager;
    private InputController inputController;
    private void Start() {
        inputController = inputControllerGO.GetComponent<InputController>();
        interfaceManager = gameManagerGO.GetComponent<InterfaceManager>();
    }
    public List<GameObject> GetInventoryCell() {
        return inventoryCell;
    }
    public void CloseInventory() {
        interfaceManager.ShowHideInventory();
        inputController.SwitchState<InGameState>();
    }
}
