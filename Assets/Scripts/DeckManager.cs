/**
 * This script manages the deck and discard pile
 * during the game. Both of these are a list of cards,
 * and the cards are cycled from the deck to the discard
 * pile after use. Once the deck is empty, the discard pile
 * is reshuffled back into the deck.
**/ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DeckManager : MonoBehaviour
{
    //Add a way for the deck to use the rectransform height of the deck to place cards
    [SerializeField] List<GameObject> cardsInOrder;
    private List<GameObject> discardPile = new List<GameObject>();
    private List<GameObject> deckList = new List<GameObject>();

    float deckHeight = 0;
    float discardHeight = 0;

    private void Start()
    {
        Create();
    }

    // Shuffles cards from discard list back into deck
    public void Shuffle()
    {
        StartCoroutine(ShuffleAll());
    }

    IEnumerator ShuffleAll() 
    {
        int randomIndex;
        for (int i = 0; i < deckList.Count; i++)
        {
            randomIndex = Random.Range(0, deckList.Count);
            SwapPosition(deckList[i], deckList[randomIndex]);
            yield return new WaitForEndOfFrame();
        }

        /*
        foreach (GameObject card in deckList)
        {
            yield return new WaitForFixedUpdate();
            card.GetComponent<Animator>().SetFloat("Blend", Random.Range(0, 6));
            card.GetComponent<Animator>().SetTrigger("Shuffle");
        }
        */
    }

    // Change position of two cards.
    void SwapPosition(GameObject card1, GameObject card2)
    {
        float y = card1.transform.position.y;
        float yNew = card2.transform.position.y;

        card1.transform.position = new Vector3(0, yNew, 0);
        card2.transform.position = new Vector3(0, y, 0);

        int indexCard1 = deckList.IndexOf(card1);
        int indexCard2 = deckList.IndexOf(card2);

        GameObject CardToSwap = deckList[indexCard1];
        deckList[indexCard1] = deckList[indexCard2];
        deckList[indexCard2] = CardToSwap;
    }

    // Remove card from deck, put it into play, move to discard pile.
    public Card Draw()
    {
        //Reshuffle deck when deck is empty
        SoundManager.PlaySound("draw");
        if (deckList.Count == 0)
        {
            deckHeight = 0;
            discardHeight = 0;
            foreach (GameObject card in discardPile) 
            {
                CardtoDeck(card);
            }
            Shuffle();
        }
        return CardtoDDiscard(deckList[deckList.Count-1]);
    }

    // Add card to deck.
    void CardtoDeck(GameObject card) 
    {
        deckHeight += .02f;
        discardPile.Remove(card);
        deckList.Add(card);
        card.transform.position = new Vector3(0, deckHeight, 0);
        card.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    // Add card to discard pile.
    Card CardtoDDiscard(GameObject card) 
    {
        NotificationCenter.PostNotification("Draw", card.GetComponent<Card>().CardImage);
        discardHeight += .02f;
        deckList.Remove(card);
        discardPile.Add(card);
        card.transform.position = new Vector3(7.25f, discardHeight, 0);
        card.transform.rotation = new Quaternion(180,0,0,0);
        return card.GetComponent<Card>();
    }

    // Creates deck and shuffles it.
    private void Create()
    {
        foreach (GameObject card in cardsInOrder)
        {
            if (cardsInOrder.IndexOf(card) == 1)
            {
                for (int cardIndex = 0; cardIndex < 5; cardIndex++)
                {
                    deckList.Add(Instantiate(card, new Vector3(0, deckHeight, 0), Quaternion.identity));
                    deckHeight += .02f;
                }
            }
            else
            {
                for (int cardIndex = 0; cardIndex < 4; cardIndex++)
                {
                    deckList.Add(Instantiate(card, new Vector3(0, deckHeight, 0), Quaternion.identity));
                    deckHeight += .02f;
                }
            }
        }

        Shuffle();
    }
}

// Used for testing.
#if UNITY_EDITOR
[CustomEditor(typeof(DeckManager))]
public class DeckEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DeckManager test = (DeckManager)target;
        if (GUILayout.Button("Shuffle")) test.Shuffle();
    }
}
#endif