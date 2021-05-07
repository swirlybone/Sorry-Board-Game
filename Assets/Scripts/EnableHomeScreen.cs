/**
 * Simply controls the pause menu.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableHomeScreen : MonoBehaviour
{
    [SerializeField] GameObject homeScreen;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) homeScreen.SetActive(true);
    }
}
