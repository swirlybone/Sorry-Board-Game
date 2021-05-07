/**
 * This is the abstract class for card data.
 * Cards can either provide a generic movement option,
 * or give the player several options.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    [SerializeField] protected bool hasChoice;
    [SerializeField] protected CardData choiceOne;
    [SerializeField] protected CardData choiceTwo;
    [SerializeField] protected bool forward;
    [SerializeField] protected int cardValue;

    public bool Forward { get => forward; }
    public int CardValue { get => cardValue; }
    public bool HasChoice { get => hasChoice; }

    public abstract CardData CardText();
    public abstract CardData Choice1();
    public abstract CardData Choice2();
}