/**
 * A piece holder to bind a piece to a space.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceHolder : MonoBehaviour
{
    public GamePiece piece;

    void HoldMe(GamePiece me) => piece = me;
    public void ILeft() => piece = null;
    public GamePiece PieceOnMe() => piece;

    // If pieces collide, return old piece to home space.
    public void GotLandedOn(GamePiece me)
    {
        if (piece != null) piece.ReturnHome();
        HoldMe(me);
    }

    public void SendHome(GamePiece me)
    {
        if (piece != null && piece.Player != me.Player) piece.ReturnHome();
    }
}
