using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public void OnMouseEnter()
    {
        transform.localScale += new Vector3(0.5F, 0, 0);

    }

    public void OnMouseExit()
    {

        transform.localScale += new Vector3(0, 0, 0);
    }

}
