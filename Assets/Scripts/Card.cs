/**
 * This is the class for every Card in the game.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static NotificationCenter;

public class Card : MonoBehaviour
{
    [SerializeField] CardData cardText;
    [SerializeField] Sprite cardTextImage;

    public Sprite CardImage{ get => cardTextImage; }
    public CardData CardText() => cardText;
}
