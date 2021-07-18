using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBarUI : MonoBehaviour
{
    [SerializeField] private GameObject effectPanel;
    [SerializeField] private GameObject bafGrid;
    [SerializeField] private GameObject debafGrid;

    private Dictionary<EffectType, GameObject> panels;
    void Start()
    {
        panels = new Dictionary<EffectType, GameObject>() {
            {EffectType.currentSpeed, null},
            {EffectType.currentStrength, null},
            {EffectType.maxStamina, null}
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEffect(AttributeType attributeType, AttributeValueType valueType, float increase, float time) {
        GameObject panel;
        GameObject parent;
        Sprite sprite;
        string description;
        EffectType effectType = TypeConvert(attributeType);

        if(effectType == EffectType.none) {
            return;
        }
        
        Dictionary<string, string> effectInformation = EffectDataBase.GetEffectInformation(effectType);

        sprite = Resources.Load<Sprite>(effectInformation["Image"]);
        description = effectInformation["Description"];

        if(increase < 0) {
            parent = debafGrid;
        } else {
            parent = bafGrid;
        }

        panel = Instantiate(effectPanel, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
        panel.GetComponent<EffectPanelUI>().CreatePanel(this, effectType, sprite, description, time);
    }

    public void EndEffect(EffectType panelEffectType) {
        GameObject panel = panels[panelEffectType];
        Destroy(panel);

        panels[panelEffectType] = null;
    }

    private EffectType TypeConvert(AttributeType attributeType) {
        EffectType effectType = EffectType.none;

        switch(attributeType) {
            case AttributeType.speed:
                effectType = EffectType.currentSpeed;
                break;
            case AttributeType.strength:
                effectType = EffectType.currentStrength;
                break;
            case AttributeType.stamina:
                effectType = EffectType.maxStamina;
                break;
        }

        return effectType;
    }
}
