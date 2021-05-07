// Controls critical game functions such as win conditions.
// Singleton ensures one instantiation.
// Notifications are frequently sent and received by this class.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using static NotificationCenter;

public class GameManager : MonoBehaviour
{
    #region Variables and Referneces
    [SerializeField] TextMeshProUGUI turnDisplay;

    public enum GamePhase { SELECTION_PHASE, DRAWPHASE, FINISHED, SORRY, CHOOSING_OPTION }
    public GamePhase phase;

    private int turn;

    private Card draw;
    private CardData CardText;

    private static GameManager _instance = null;
    public static GameManager Instance { get { return _instance; } }

    private DeckManager deck;
    #endregion

    #region Monobehaviours
    void Start()
    {
        //Game Manager Instance
        if (_instance == null)
        {
            phase = GamePhase.DRAWPHASE;

            turn = 1;
            _instance = this;

            deck = GetComponentInChildren<DeckManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        //Draw new card during draw phase
        if (DrawPhase() && !EventSystem.current.IsPointerOverGameObject()) DrawCard(); else PostNotification("Message", "Can't draw card!");
    }

    //Add observers
    private void OnEnable()
    {
        AddObserver("NextTurn", GoNextTurn);
        AddObserver("Selected", Selected);
        AddObserver("Winner", Winner);
        AddObserver("Sorry", SendingHome);
    }
    //Remove observers
    private void OnDisable()
    {
        RemoveObserver("NextTurn", GoNextTurn);
        RemoveObserver("Selected", Selected);
        RemoveObserver("Winner", Winner);
        RemoveObserver("Sorry", SendingHome);
    }
    #endregion

    #region Helper Functions
    // NOTIFICATION ACTIONS
    public void GoNextTurn(Notification notification)
    {
        if (turn < 4) turn++;
        else turn = 1;

        UpdateHud(turn);
        phase = GamePhase.DRAWPHASE;
        SoundManager.PlaySound("next");
    }

    public void UpdateHud(int toSet)
    {
        turnDisplay.SetText(toSet.ToString());
        if (toSet == 1)
        {
            turnDisplay.color = new Color32(255, 0, 0, 255);
        }
        else if (toSet == 2)
        {
            turnDisplay.color = new Color32(0, 255, 0, 255);
        }
        else if (toSet == 3)
        {
            turnDisplay.color = new Color32(0, 0, 255, 255);
        }
        else
        {
            turnDisplay.color = new Color32(255, 255, 0, 255);
        }
    }

    public void ChooseNext() => PostNotification("SelectNext");
    public void ChoosePrevious() => PostNotification("SelectLast");

    public void PassTurn() => GoNextTurn(new Notification());

    public void PieceCamera() => PostNotification("Camera", "PIECE");

    public void BoardCamera() => PostNotification("Camera", "BOARD");

    public void ResetCamera() => PostNotification("ResetCamera");

    public void Selected(Notification notification)
    {
        if (!Choosing())
        {
            if (Selecting())
            {
                GamePiece selected;
                selected = (GamePiece)notification.Object;

                if (turn == selected.Player)
                {
                    //This needs top be updated for new card logic
                    selected.FindMove(CardText.CardValue, CardText.Forward);
                }
                else PostNotification("Message", "Wrong player");
            }
            else if (Sorry())
            {
                GamePiece selected;
                selected = (GamePiece)notification.Object;
                if (turn != selected.Player) { selected.ReturnHome(); GoNextTurn(new Notification()); } else PostNotification("Message", "Wrong player");
            }
            else PostNotification("Message", "Reminder, you must draw a card to move this piece!");
        }
        else { PostNotification("Message", "You must choose an option!"); }
    }

    void Winner(Notification notification)
    {
        phase = GamePhase.FINISHED;
        PostNotification("Message", notification.Object.ToString() + " is the Winner!!!");
    }

    void SendingHome(Notification notification)
    {
        phase = GamePhase.SORRY;
    }

    public void DrawCard()
    {
        draw = deck.Draw();
        CardText = draw.CardText();
        if (CardText.HasChoice)
        {
            PostNotification("Choose", draw.CardImage);
            phase = GamePhase.CHOOSING_OPTION;
        }
        else
        {
            SelectionPhase();
        }
    }

    //Add function that can be called on button presses to choose 1 or 2 and set it to Draw

    bool Selecting() => phase == GamePhase.SELECTION_PHASE;

    bool DrawPhase() => phase == GamePhase.DRAWPHASE;

    bool Sorry() => phase == GamePhase.SORRY;

    bool Choosing() => phase == GamePhase.CHOOSING_OPTION;

    public void ChoseOptionOne() { CardText = CardText.CardText().Choice1(); }
    public void ChoseOptionTwo() { CardText = CardText.CardText().Choice2(); }

    public void SelectionPhase() { if(phase != GamePhase.SORRY) phase = GamePhase.SELECTION_PHASE; }
    #endregion
}

// Used for testing.
#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class ManagerTester : Editor
{
    int move;
    public override void OnInspectorGUI()
    {
        GameManager manager = (GameManager)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Next Turn")) manager.PassTurn();
    }
}
#endif