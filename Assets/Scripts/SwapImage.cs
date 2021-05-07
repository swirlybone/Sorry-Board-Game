/**
 * Used to change image on card holder on HUD.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NotificationCenter;

public class SwapImage : MonoBehaviour
{
    [SerializeField] Image sprite;

    private void OnEnable()
    {
        AddObserver("Draw", NewImage);
    }

    private void OnDisable()
    {
        RemoveObserver("Draw", NewImage);
    }

    void NewImage(Notification notification) 
    {
        sprite.sprite = (Sprite)notification.Object;
    }
}
