/**
 * The script controls the generic BoardSpaces.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NotificationCenter;
using UnityEngine.UIElements;

public abstract class BoardSpace : MonoBehaviour
{
    [SerializeField] protected Material border;

    protected BoardSpace lastSpace;
    protected BoardSpace nextSpace;
    protected BoardSpace special;
    private int player;
    private int slidePlayer;
    private bool canSlide = false;
    private bool safe = false;

    // Get next space.
    public BoardSpace NextSpace { get => nextSpace; }
    // Get last space.
    public BoardSpace LastSpace { get => lastSpace; }
    // Get the next special space.
    public BoardSpace SpecialNext { get => special; }
    // Get the current player.
    public int Player { get => player; }
    // Get boolean determining if piece can slide.
    public bool CanSlide { get => canSlide; }
    // Get number of spaces slid.
    public int SlidePlayer { get => slidePlayer; }

    // Method to find the next space after "This" space.
    public BoardSpace FindNextSpace(GamePiece piece)
    {
        if (Player == piece.Player)
        {
            return SpecialNext;
        }
        else return NextSpace;
    }

    // Finds previous space behind "This" space.
    public BoardSpace FindPreviousSpace() => LastSpace;

    // Searches for a board space.
    public BoardSpace Search(int moveTimes, bool forward, GamePiece piece)
    {
        if (moveTimes != 0)
        {
            moveTimes--;
            if (Player == piece.Player && forward)
            {
                return SpecialNext.Search(moveTimes, forward, piece);
            }
            else
            {
                try
                {
                    if (forward) return NextSpace.Search(moveTimes, forward, piece);
                    else return LastSpace.Search(moveTimes, forward, piece);
                }
                catch { return piece.CurrentSpace; }
            }
        }
        else return this;
    }

    // Searches for a sliding space.
    public BoardSpace SlideSearch(int moveTimes, GamePiece piece)
    {
        if (moveTimes != 0)
        {
            moveTimes--;
            try
            {
                return NextSpace.SlideSearch(moveTimes, piece);
            }
            catch { return piece.CurrentSpace; }
        }
        else return this;
    }

    // Gets and Sets
    public abstract PieceHolder GetNode();
    public abstract void LeftNode(GamePiece piece);
    public abstract void SetPrevious(BoardSpace space);
    public abstract void SetNext(BoardSpace space);
    public abstract void SetSpecialNext(BoardSpace space);
    public bool Occupied() => GetNode().PieceOnMe() != null;
    public bool HasFriend(GamePiece piece) => GetNode().PieceOnMe().Player == piece.Player;
    public bool IsFriendly(GamePiece piece) => Player == piece.Player;
    public bool IsSliderFor(GamePiece piece) => CanSlide && piece.Player == Player;
    public void SetSlider(int player) { this.slidePlayer = player; canSlide = true; }
    public void SetPlayer(int player) => this.player = player;
    public void SetSafe() => safe = true;
    public bool IsSafe() => safe;
}