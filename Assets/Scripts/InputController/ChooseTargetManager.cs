using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChooseTargetManager : MonoBehaviour
{
    [SerializeField] private GameObject currentItemGO;
    private InputController inputController;
    private Item choosedItem;
    private Player player;
    private Sprite itemSprite;
    // Start is called before the first frame update
    void Start()
    {
        inputController = GetComponent<InputController>();
    }
    void Update()
    {
        
    }

    public void EnterState(Item item, Sprite sprite) {
        Debug.Log("Enter");
        choosedItem = item;
        itemSprite = sprite;
        currentItemGO.GetComponent<Image>().sprite = sprite;
        currentItemGO.SetActive(true);
    }
    public void ExitState() {
        choosedItem = null;
        itemSprite = null;
        currentItemGO.SetActive(false);
        inputController.SwitchState<InGameState>();
    }
    public void BeginAction() {
        Camera cam = Camera.main;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 choosedPos = new Vector3(mousePos.x, mousePos.y);

        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        GameObject newItem = ItemPrefabBuilder.BuildThrowingItemPrefab(choosedItem, itemSprite);
        Debug.Log("кидаю");

        player.ThrowAmmo(choosedPos, newItem);
        ExitState();
    }

}
