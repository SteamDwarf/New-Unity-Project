using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFollowingByCursor : MonoBehaviour
{
    private Vector2 mousePos;
    private void Update() {
        mousePos = Input.mousePosition;
        transform.position = mousePos;
    }
}
