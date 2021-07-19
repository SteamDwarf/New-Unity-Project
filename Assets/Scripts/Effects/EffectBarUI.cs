using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBarUI : MonoBehaviour
{
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject effectPanel;
    [SerializeField] private GameObject bafGrid;
    [SerializeField] private GameObject debafGrid;

    private Player player;
    private Dictionary<EffectType, GameObject> panelsBafs;
    private Dictionary<EffectType, GameObject> panelsDebafs;
    private Color32 bafColor;
    private Color32 debafColor;
    void Start()
    {
        panelsBafs = new Dictionary<EffectType, GameObject>() {
            {EffectType.currentSpeed, null},
            {EffectType.currentStrength, null},
            {EffectType.maxStamina, null}
        };

        panelsDebafs = new Dictionary<EffectType, GameObject>() {
            {EffectType.currentSpeed, null},
            {EffectType.currentStrength, null},
            {EffectType.maxStamina, null}
        };

        bafColor = Color.white;
        debafColor = new Color32(152, 18, 18, 255);
    }

    public void GetPlayer(Player player) {
        this.player = player;
    }

    public void SetEffect(EffectClass effectClass, EffectType effectType, float time) {
        GameObject panel;
        GameObject parent;
        Sprite sprite;
        string description;
        Color32 color;
        Effect curEffect;
        Dictionary<EffectType, GameObject> curPanels;
        Dictionary<string, object> effectInformation;
        //EffectType effectType = TypeConvert(attributeType);

        if(effectClass == EffectClass.baf) {
            curPanels = panelsBafs;
        } else {
            curPanels = panelsDebafs;
        }

        effectInformation = EffectDataBase.GetEffectInformation(effectClass, effectType);
        curEffect = (Effect)effectInformation["Effect"];

        if(curPanels[effectType] == null) {

            curEffect.StartEffect();
            sprite = Resources.Load<Sprite>((string)effectInformation["Image"]);
            description = (string)effectInformation["Description"];
            //color = (Color32)effectInformation["Color"];

            if(effectClass == EffectClass.baf) {
                parent = bafGrid;
                color = bafColor;
            } else {
                parent = debafGrid;
                color = debafColor;
            }

            panel = Instantiate(effectPanel, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
            panel.GetComponent<EffectPanelUI>().CreatePanel(this, descriptionPanel, effectClass, effectType, sprite, color, description, time);
            curPanels[effectType] = panel;
        } else {
            curPanels[effectType].GetComponent<EffectPanelUI>().RefreshTime(time);
        }

        
    }

    public void EndEffect(EffectClass panelEffectClass, EffectType panelEffectType) {
        Dictionary<EffectType, GameObject> curPanels;

        if(panelEffectClass == EffectClass.baf) {
            curPanels = panelsBafs;
        }else {
            curPanels = panelsDebafs;
        }

        GameObject panel = curPanels[panelEffectType];
        Destroy(panel);

        curPanels[panelEffectType] = null;
        player.UnsetEffect(panelEffectClass, panelEffectType);
    }

/*     private EffectType TypeConvert(AttributeType attributeType) {
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
    } */
}
