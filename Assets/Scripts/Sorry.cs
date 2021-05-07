/**
 * The Sorry! card controller. Allows
 * a player to return another player's piece
 * back home.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NotificationCenter;

[CreateAssetMenu(fileName = "Data", menuName = "CardText/Sorry", order = 1)] // Generate card asset.
public class Sorry : CardData
{
    public override CardData CardText() => this;

    public override CardData Choice1()
    {
        PostNotification("Sorry");
        return choiceOne;
    }

    public override CardData Choice2() => choiceTwo;
}
