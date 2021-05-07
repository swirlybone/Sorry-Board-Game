/**
 * The start space is the space from which a piece starts.
 * A piece will return here if it collides with another piece.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using static NotificationCenter;

public class StartSpace : BoardSpace
{
    // For generation.

    [SerializeField] PieceHolder[] nodes;
    
    public override PieceHolder GetNode()
    {
        foreach (PieceHolder node in nodes)
        {
            if (node.PieceOnMe() == null)
            {
                return node;
            }
        }
        Debug.Log("For loop failed");
        return nodes[0];
    }

    public override void SetNext(BoardSpace space) => nextSpace = space;

    public override void SetPrevious(BoardSpace space) => lastSpace = space;

    public override void SetSpecialNext(BoardSpace space) => special = space;

    public override void LeftNode(GamePiece piece)
    {
        foreach (PieceHolder node in nodes) if (piece == node.PieceOnMe()) node.ILeft();
    }
}

// Used for testing.
#if UNITY_EDITOR
[CustomEditor(typeof(StartSpace))]
public class BoardSpaceTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StartSpace boardSpace = (StartSpace)target;

        try { GUILayout.Label("Next Space: " + boardSpace.NextSpace.name); } catch { GUILayout.Label("No Next Space"); }
        try { GUILayout.Label("Last Space: " + boardSpace.LastSpace.name); } catch { GUILayout.Label("No Last Space"); }
        try { GUILayout.Label("Special Space: " + boardSpace.SpecialNext.name); } catch { GUILayout.Label("No Special Space"); }
    }
}
#endif