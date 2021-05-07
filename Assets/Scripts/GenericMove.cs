/**
 * Most cards provide a generic move option.
 * This controls those cards. (Every one except Sorry)
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "CardText/GenericMove", order = 2)]
public class GenericMove : CardData
{
    public override CardData CardText() => this;
    public override CardData Choice1() => choiceOne;
    public override CardData Choice2() => choiceTwo;
}
