using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBarUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> hotBarCells;

    public List<GameObject> GetHotBarCells() {
        return hotBarCells;
    }
}
