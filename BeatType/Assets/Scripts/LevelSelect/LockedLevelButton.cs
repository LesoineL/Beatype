using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedLevelButton : MonoBehaviour
{
    void OnMouseEnter()
    {
        gameObject.transform.localScale = new Vector3(.75f, .75f);
    }

    void OnMouseExit()
    {
        gameObject.transform.localScale = Vector3.one;
    }
}
