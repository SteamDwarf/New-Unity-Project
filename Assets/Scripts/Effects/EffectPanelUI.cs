using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EffectPanelUI : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image effectImage;
    [SerializeField] private TextMeshProUGUI timerUI;
    
    private GameManager gameManager;
    private GameObject descriptionPanel;
    private EffectBarUI effectBarUI;
    private EffectClass panelEffectClass;
    private EffectType panelEffectType;
    private float effectTime;
    private string effectDescription;
    private bool mouseEnter;

    private void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update() {
        if(gameManager.isPaused) {
            return;
        }
        if(effectTime >= 0) {
            TimerUpdate();    
        } else {
            effectBarUI.EndEffect(panelEffectClass, panelEffectType);
        }
        
    }

    public void CreatePanel(EffectBarUI effectBarUIScript, GameObject descriptionPanel, EffectClass effectClass, EffectType effectType, Sprite sprite, Color32 color, string description, float time) {
        this.descriptionPanel = descriptionPanel;
        effectBarUI = effectBarUIScript;
        panelEffectClass = effectClass;
        panelEffectType = effectType;
        effectImage.sprite = sprite;
        effectImage.color = color;

        effectDescription = description;
        //panelDescription.text = description;
        //panelDescription.color = color;

        //timerUI.color = color;

        effectTime = time;
    }

    public void RefreshTime(float time) {
        effectTime = time;
    }
    
    private void TimerUpdate() {
        float time = effectTime * 60;
        float minutes = Mathf.Floor(time / 60);

        timerUI.text = $"{Mathf.Floor(minutes)} : {Mathf.Round(time - (minutes * 60))}";
        effectTime -= 0.01f * Time.deltaTime;
    }

    public void OnPointerEnter (PointerEventData eventData) {
        descriptionPanel.GetComponentInChildren<TextMeshProUGUI>().text = effectDescription;
        descriptionPanel.transform.position = this.gameObject.transform.position;
        descriptionPanel.SetActive(true);
        
        StopCoroutine(HideDescriptionCoroutine());
        StartCoroutine(HideDescriptionCoroutine());
    }
    IEnumerator HideDescriptionCoroutine() {
        yield return new WaitForSeconds(1f);
        descriptionPanel.SetActive(false);
    }
}
