/**
 * The Game Pieces are what the pawns the player controls.
 * Each player has three. This script keeps track of 
 * where they are and controls their animations, models, etc.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using static NotificationCenter;

public class GamePiece : MonoBehaviour
{
    #region Variables and References
    //Is the player in safe spaces or non safe spaces (can be reveresed or kicked back to home)
    public enum Safe { UNSAFE, SAFE, HOME, FINISHED }
    Safe safe;

    private BoardSpace currentSpace;
    private StartSpace startSpace;

    Animator animator;
    private int playerNumber;
    float gameSpeed;

    public int Player { get => playerNumber; }
    public BoardSpace CurrentSpace { get => currentSpace; }
    #endregion

    // Allows selection of game piece. GameManager and CameraController
    // are notified of the currently selected GamePiece.
    private void OnMouseDown()
    {
        PostNotification("Selected", this);
    }

    // Used to move piece forwards.
    #region Coroutine Enumerators
    IEnumerator MoveForwards(BoardSpace move)
    {
        if (CurrentSpace != null) LeaveSpace();
        while (currentSpace != move)
        {
            yield return GoNext(CurrentSpace.FindNextSpace(this));
        }
        EnterSpace();
        PostNotification("NextTurn");
    }

    // Used to move piece backwards.
    IEnumerator MoveBackwards(BoardSpace move)
    {
        if (CurrentSpace != null) LeaveSpace();
        while (currentSpace != move)
        {
            yield return GoNext(CurrentSpace.FindPreviousSpace());
        }
        yield return new WaitForEndOfFrame();
        EnterSpace();
        PostNotification("NextTurn");
    }

    // Move to next space.
    IEnumerator GoNext(BoardSpace boardSpace)
    {
        animator.SetTrigger("Bounce");
        currentSpace = boardSpace;
        Transform node = boardSpace.GetNode().transform;
        transform.LookAt(node);
        while (transform.position != FixNode(node.position))
        {
            transform.position = Vector3.MoveTowards(this.transform.position, node.position, gameSpeed);
            yield return new WaitForEndOfFrame();
        }
        SoundManager.PlaySound("move");
        if (CurrentSpace.CompareTag("Home")) safe = Safe.FINISHED;
    }

    // Used if piece is on slide spaces.
    IEnumerator SlideForward()
    {
        PostNotification("Message", "You perform a slide and knock everyone else away!");
        if (CurrentSpace != null) LeaveSpace();
        for (int index = 0; index < 5; index++) { currentSpace.SlideSearch(index, this).GetNode().SendHome(this); }
        currentSpace = currentSpace.SlideSearch(4, this);
        Transform node = currentSpace.GetNode().transform;
        transform.LookAt(node);
        while (transform.position != FixNode(node.position))
        {
            transform.position = Vector3.MoveTowards(this.transform.position, node.position, gameSpeed * 3);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    // Moves piece back home if it shares space with another piece.
    IEnumerator GoHome()
    {
        if (CurrentSpace != null) LeaveSpace();
        animator.SetTrigger("GoHome");
        currentSpace = startSpace;
        Transform node = currentSpace.GetNode().transform;
        transform.LookAt(node);
        while (transform.position != FixNode(node.position))
        {
            if (Vector3.Distance(transform.position, FixNode(node.position)) <= 1f) animator.SetBool("Drop", true);
            transform.position = Vector3.MoveTowards(this.transform.position, node.position, gameSpeed * 3);
            yield return new WaitForEndOfFrame();
        }
        animator.SetBool("Drop", false);
        yield return null;
    }
    #endregion

    #region Helper Functions
    Vector3 FixNode(Vector3 node) { return new Vector3(node.x, transform.position.y, node.z); }
    public void Slide() { StartCoroutine(SlideForward()); }
    public void SlideBegin() { animator.SetTrigger("Slide"); }
    public GamePiece Initializer(int player, StartSpace startSpace, float speed)
    {
        safe = Safe.HOME;
        this.currentSpace = startSpace;
        this.playerNumber = player;
        this.startSpace = startSpace;
        gameSpeed = speed;
        transform.localScale = new Vector3(4, 1, 4);
        animator = GetComponent<Animator>();

        return this;
    }

    public BoardSpace FindMove(int moveTimes, bool forward)
    {
        BoardSpace move = CurrentSpace.Search(moveTimes, forward, this);

        if (!Finished())
        {
            if (currentSpace == startSpace && !forward)
            {
                PostNotification("Message", "Can't move this piece backwards, it's at Start!");
            }
            else
            {
                if (move.Occupied())
                {
                    if (move.HasFriend(this)) PostNotification("Message", "Can't move this piece!");
                    else
                    {
                        BeginMovement(move, forward);
                    }
                }
                else BeginMovement(move, forward);
            }

        }
        else PostNotification("Message", "Can't move this piece, it's at Home!");

        return move;
    }

    void BeginMovement(BoardSpace move, bool forward)
    {
        if (forward) StartCoroutine(MoveForwards(move));
        else
        {
            if (IsSafe()) PostNotification("Message", "Can't move backwards from safe area!");
            else StartCoroutine(MoveBackwards(move));
        }
    }

    void EnterSpace()
    {
        CurrentSpace.GetNode().GotLandedOn(this);
        if (CurrentSpace.CanSlide)
        {
            if (currentSpace.SlidePlayer == playerNumber)
            {
                try
                {
                    if (!currentSpace.SlideSearch(5, this).HasFriend(this)) SlideBegin();
                }
                catch
                {
                    SlideBegin();
                }
            }
        }
    }

    void LeaveSpace()
    {
        CurrentSpace.LeftNode(this);
    }

    public void ReturnHome() { StartCoroutine(GoHome()); CurrentSpace.GetNode().GotLandedOn(this); }
    public bool Finished() => safe == Safe.FINISHED;
    public bool IsSafe() => safe == Safe.SAFE;
    #endregion
}

// Used for testing.
#if UNITY_EDITOR
[CustomEditor(typeof(GamePiece))]
public class GamePieceTester : Editor
{
    int move;
    bool forward = true;
    public override void OnInspectorGUI()
    {
        move = EditorGUILayout.IntField("Move # Spaces: ", move);
        forward = EditorGUILayout.Toggle("Forward?", forward);

        GamePiece gamePiece = (GamePiece)target;
        base.OnInspectorGUI();

        //GUILayout.Label("Current space: " + gamePiece.CurrentSpace.name);
        if (GUILayout.Button("Move Piece")) gamePiece.FindMove(move, forward);
        if (GUILayout.Button("Go Home")) gamePiece.ReturnHome();
    }
}
#endif