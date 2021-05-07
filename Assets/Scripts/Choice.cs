/**
 * Essentially, if one of the Cards in the
 * game has a choice, this script will
 * activiate the panel in the UI that allows
 * the player to choose which option they want.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NotificationCenter;

public class Choice : MonoBehaviour
{
    [SerializeField] GameObject panel;

    private void OnEnable()
    {
        AddObserver("Choose", OpenPanel);
    }

    private void OnDisable()
    {
        RemoveObserver("Choose", OpenPanel);
    }

    void OpenPanel(Notification notification) 
    {
        panel.SetActive(true);
    }
}
