using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorTarget : MonoBehaviour
{
    private Vector2 mousePos;
    private Camera cam;

    private void Start() {
        cam = Camera.main;
    }
    private void Update() {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 truePos = new Vector3(mousePos.x, mousePos.y);
        transform.position = truePos;
    }
}
