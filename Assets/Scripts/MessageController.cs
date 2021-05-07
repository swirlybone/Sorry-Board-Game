/**
 * This script updates the message box with various
 * messages, allowing the player to see the result
 * of their move.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
using static NotificationCenter;

public class MessageController : MonoBehaviour
{
    [SerializeField] GameObject messageMesh;

    private void OnEnable()
    {
        AddObserver("Message", NewMessage);
    }

    private void OnDisable()
    {
        RemoveObserver("Message", NewMessage);
    }

    void NewMessage(Notification notification) 
    {
        Instantiate(messageMesh, transform).GetComponent<TextMeshProUGUI>().text = (string)notification.Object;
    }
}


// Used for testing.
#if UNITY_EDITOR
[CustomEditor(typeof(MessageController))]
class CustomMessage : Editor
{
    string message;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        message = EditorGUILayout.TextArea(message);
        if (GUILayout.Button("Send Message")) SendMessage();
    }

    void SendMessage() { PostNotification("Message", message); }
}
#endif