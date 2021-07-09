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
    private Item choosedItem;
    private Player player;
    private Sprite itemSprite;
    private Inventory inventory;
    private int itemId;
    void Start()
    {
        cam = Camera.main;
        inputController = GetComponent<InputController>();
    }

    public void EnterState(Item item, Sprite sprite, int id) {
        choosedItem = item;
        itemSprite = sprite;
        itemId = id;
        //currentItemGO.GetComponent<Image>().sprite = sprite;
        currentItemGO.SetActive(true);
    }
    public void ExitState() {
        choosedItem = null;
        itemSprite = null;
        currentItemGO.SetActive(false);
        inputController.SwitchState<InGameState>();
        itemId = -1;
    }
    public void BeginAction() {

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 choosedPos = new Vector3(mousePos.x, mousePos.y);

        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        GameObject newItem = ItemPrefabBuilder.BuildThrowingItemPrefab(choosedItem, 1, itemSprite);
        player.ThrowAmmo(choosedPos, newItem);

        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        inventory.DecreaseItemNumber(itemId, 1);

        ExitState();
    }

}
