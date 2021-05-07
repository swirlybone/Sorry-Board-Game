/**
 * A simplt script to control the main menu.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static NotificationCenter;

public class MenuController : MonoBehaviour
{
    // Loads next scene when "Play" is clicked.
    public void PlayGame()
    {
        PostNotification("NextLevel");
        gameObject.SetActive(false);
    }

    // Exit game.
    public void QuitGame()
    {
        Application.Quit();
    }
}

// todo: give credit for bg image https://www.pexels.com/photo/close-up-photography-of-yellow-green-red-and-brown-plastic-cones-on-white-lined-surface-163064/