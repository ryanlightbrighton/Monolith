using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanish : MonoBehaviour
{
    public void ToggleVisibility()
    {
        if (gameObject.layer == 0) {
            gameObject.layer = 10;
        } else {
            gameObject.layer = 0;
        }
    }
}
