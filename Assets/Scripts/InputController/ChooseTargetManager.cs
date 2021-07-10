using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseTargetManager : MonoBehaviour
{
    [SerializeField] private GameObject currentItemGO;
    private Camera cam;
    private InputController inputController;
    private Player player;
    private Inventory inventory;
    private int cellId;
    private int itemId;
    void Start()
    {
        cam = Camera.main;
        inputController = GetComponent<InputController>();
    }

    public void EnterState(int cellId, int itemId) {
        this.cellId = cellId;
        this.itemId = itemId;
        currentItemGO.SetActive(true);
    }
    public void ExitState() {
        currentItemGO.SetActive(false);
        inputController.SwitchState<InGameState>();
        cellId = -1;
        itemId = -1;
    }
    public void BeginAction() {
        GameObject newItem;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 choosedPos = new Vector3(mousePos.x, mousePos.y);

        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        newItem = ItemPrefabBuilder.GetItemByResources(itemId);
        player.ThrowAmmo(choosedPos, newItem);

        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        inventory.DecreaseItemNumber(cellId, 1);

        ExitState();
    }

}
