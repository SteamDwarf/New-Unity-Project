using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ContextMenu : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private GameObject inputCountBlock;
    [SerializeField] private GameObject input;
    private string count;
    private Regex regex = new Regex(@"\D");

    public void UseItem() {
        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        inventory.UseItem();
    }

    public void ShowItemDescription() {
        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }
        inventory.ShowItemDescription();
    }

    public void HideContextMenu() {
        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }

        inventory.HideContextMenu();
        inputCountBlock.SetActive(false);
    }

    public void ShowCountInput() {
        inputCountBlock.SetActive(true);
    }
    public void HideCountInput() {
        inputCountBlock.SetActive(false);
    }

    public void DropItem() {
        if(inventory == null) {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        }

        //count = input.GetComponent<TextMeshProUGUI>().text;
        inventory.DropItem();
        inputCountBlock.SetActive(false);
    }

// TODO: При вводе символов в инпут текст должен фильтроваться
// TODO: Также хотбар должен блокироваться
    public void OnValueChanged() {
/*         string text = input.GetComponent<TMP_InputField>().text;
        regex.Replace(text,"");
        Debug.Log(text);
        input.GetComponent<TMP_InputField>().text = text; */
        //count = input.GetComponent<TextMeshProUGUI>().text;   
    }
}
