using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EffectPanelUI : MonoBehaviour
{
    [SerializeField] private Image effectImage;
    [SerializeField] private TextMeshProUGUI panelDescription;
    [SerializeField] private TextMeshProUGUI timerUI;
    
    private EffectBarUI effectBarUI;
    private EffectType panelEffectType;
    private float effectTime;

    private void Update() {
        if(effectTime >= 0) {
            TimerUpdate();    
        }
        
    }

    public void CreatePanel(EffectBarUI effectBarUIScript, EffectType effectType, Sprite sprite, string description, float time) {
        effectBarUI = effectBarUIScript;
        panelEffectType = effectType;
        effectImage.sprite = sprite;
        panelDescription.text = description;
        effectTime = time;
    }
    
    private void TimerUpdate() {
        if (effectTime > 0) {
            float time = effectTime * 60;
            float minutes = Mathf.Floor(time / 60);

           timerUI.text = $"{Mathf.Floor(minutes)} : {Mathf.Round(time - (minutes * 60))}";
           effectTime -= 0.01f * Time.deltaTime;
        } else {
            effectBarUI.EndEffect(panelEffectType);
        }
    }
}
