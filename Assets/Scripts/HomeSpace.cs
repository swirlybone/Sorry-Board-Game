/**
 * The HomeSpace is a special BoardSpace.
 * When a three pieces make it back to home,
 * a winner is declared.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NotificationCenter;
public class HomeSpace : StartSpace
{
    int piecesOnMe = 0;

    public void GotLandedOn(Notification notification)
    {
        if ((BoardSpace)notification.UserInfo["space"] == this)
        {
            piecesOnMe++;
            if (CompareTag("Home") && piecesOnMe == 3) PostNotification("Winner", this);
            SoundManager.PlaySound("victory");
        }
    }
}
