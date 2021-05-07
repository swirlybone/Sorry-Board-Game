/**
 * Controls the majority of spaces on the board,
 * the "normal" spaces.
 * This script keeps track of how many pieces are
 * on a space, and helps to build the board during
 * board generation.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using static NotificationCenter;

public class NormalSpace : BoardSpace
{
    [SerializeField] PieceHolder node;

    GamePiece pieceOnMe; // What piece, if any, is on this space?

    private void Start()
    {
        List<Material> materials = new List<Material>(GetComponent<MeshRenderer>().materials);
        materials.Add(border);
        GetComponent<MeshRenderer>().materials = materials.ToArray();
    }


    // Used for board generation.

    public override PieceHolder GetNode() => node;

    public override void LeftNode(GamePiece piece)
    {
        node.ILeft();
    }

    public override void SetNext(BoardSpace space) => nextSpace = space;

    public override void SetPrevious(BoardSpace space) => lastSpace = space;

    public override void SetSpecialNext(BoardSpace space) => special = space;
}


// Used for testing.
#if UNITY_EDITOR
[CustomEditor(typeof(NormalSpace))]
public class SpaceTester : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NormalSpace boardSpace = (NormalSpace)target;

        try { GUILayout.Label("Next Space: " + boardSpace.NextSpace.name); } catch { GUILayout.Label("No Next Space"); }
        try { GUILayout.Label("Last Space: " + boardSpace.LastSpace.name); } catch { GUILayout.Label("No Last Space"); }
        try { GUILayout.Label("Special Space: " + boardSpace.SpecialNext.name); } catch { GUILayout.Label("No Special Space"); }
        try { GUILayout.Label("Piece On Me: " + boardSpace.GetNode().PieceOnMe().name); } catch { GUILayout.Label("Nothing on this space"); }
    }
}
#endif
