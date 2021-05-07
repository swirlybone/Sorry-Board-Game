/**
 * This is the script for BoardGeneration.
 * When the Play button is clicked on the Main Menu,
 * the GameBoard is dynamically generated
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    #region Serialized Properties and References
    [SerializeField] float gameSpeed = 1f;

    [SerializeField] GameObject boardSpace;
    [SerializeField] GameObject startSpace;
    [SerializeField] GameObject homeSpace;
    [SerializeField] GameObject gamePiece;
    [SerializeField] GameObject centerSpace;

    [SerializeField] Material redMat;
    [SerializeField] Material blueMat;
    [SerializeField] Material greenMat;
    [SerializeField] Material yellowMat;
    [SerializeField] Material boardMat;
    [SerializeField] Material deckMat;

    //Add all 4 materials

    [SerializeField] GameObject cardDeck;

    StartSpace redHome;
    StartSpace blueHome;
    StartSpace greenHome;
    StartSpace yellowHome;
    #endregion

    void Start() => CreateBoard();

    //Handles creation of board.
    public void CreateBoard()
    {
        #region Unsafe Spaces Generator
        GameObject board = GameObject.Find("GameBoard");
        GameObject boardSpaces = GameObject.Find("BoardSpaces");

        /*
        for (int i = 0; i < 15; i++)
        {
            CreaterHelper(boardSpace, new Vector3(-115, 0, -100 + (15 * i)), boardSpaces, redMat, i);
        }
        for (int i = 0; i < 15; i++)
        {
            CreaterHelper(boardSpace, new Vector3(-100 + (15 * i), 0, 110), boardSpaces, greenMat, i);
        }
        for (int i = 0; i < 15; i++)
        {
            CreaterHelper(boardSpace, new Vector3(110, 0, 95 - (15 * i)), boardSpaces, blueMat, i);
        }
        for (int i = 0; i < 15; i++)
        {
            CreaterHelper(boardSpace, new Vector3(95 - (15 * i), 0, -115), boardSpaces, yellowMat, i);
        }
        */
        #endregion

        BoardSpace[] spaces = boardSpaces.GetComponentsInChildren<BoardSpace>();

        BoardConnector(spaces, null); //Connect unsafe spaces

        #region Safe Spaces Generator
        GameObject safeSpacesRed = GameObject.Find("redSafe");
        GameObject safeSpacesBlue = GameObject.Find("blueSafe");
        GameObject safeSpacesGreen = GameObject.Find("greenSafe");
        GameObject safeSpacesYellow = GameObject.Find("yellowSafe");

        /*
        for (int j = 0; j < 6; j++)
        {
            if (j < 5)
            {
                CreaterHelper(boardSpace, new Vector3(-100 + (15 * j), 1 + j, -85), safeSpacesRed, redMat, j);
                CreaterHelper(boardSpace, new Vector3(-85, 1 + j, 95 - (15 * j)), safeSpacesGreen, greenMat, j);
                CreaterHelper(boardSpace, new Vector3(95 - (15 * j), 1 + j, 80), safeSpacesBlue, blueMat, j);
                CreaterHelper(boardSpace, new Vector3(80, 1 + j, -100 + (15 * j)), safeSpacesYellow, yellowMat, j);
            }
            else
            {
                CreaterHelper(homeSpace, new Vector3(-100 + (15 * j), 1 + j, -85), safeSpacesRed, redMat, j).tag = "Home";
                CreaterHelper(homeSpace, new Vector3(-85, 1 + j, 95 - (15 * j)), safeSpacesGreen, greenMat, j).tag = "Home";
                CreaterHelper(homeSpace, new Vector3(95 - (15 * j), 1 + j, 80), safeSpacesBlue, blueMat, j).tag = "Home";
                CreaterHelper(homeSpace, new Vector3(80, 1 + j, -100 + (15 * j)), safeSpacesYellow, yellowMat, j).tag = "Home";
            }

        }
        */
        #endregion

        #region Connect Safe Spaces

        BoardConnector(safeSpacesRed.GetComponentsInChildren<BoardSpace>(), spaces[1]);
        spaces[0].SetSlider(1);
        spaces[1].SetNext(spaces[2]);
        spaces[1].SetSpecialNext(safeSpacesRed.GetComponentsInChildren<BoardSpace>()[0]);
        spaces[1].SetPlayer(1);
        GameObject redStart = CreaterHelper(startSpace, new Vector3(-100, 0, -55), safeSpacesRed, redMat, 999);
        redStart.GetComponent<StartSpace>().SetNext(spaces[3]);
        AddPieces(redStart, board, redMat, 1);

        BoardConnector(safeSpacesGreen.GetComponentsInChildren<BoardSpace>(), spaces[16]);
        spaces[15].SetSlider(2);
        spaces[16].SetNext(spaces[17]);
        spaces[16].SetSpecialNext(safeSpacesGreen.GetComponentsInChildren<BoardSpace>()[0]);
        spaces[16].SetPlayer(2);
        GameObject greenStart = CreaterHelper(startSpace, new Vector3(-55, 0, 95), safeSpacesGreen, greenMat, 999);
        greenStart.GetComponent<StartSpace>().SetNext(spaces[18]);
        AddPieces(greenStart, board, greenMat, 2);

        BoardConnector(safeSpacesBlue.GetComponentsInChildren<BoardSpace>(), spaces[31]);
        spaces[30].SetSlider(3);
        spaces[31].SetNext(spaces[32]);
        spaces[31].SetSpecialNext(safeSpacesBlue.GetComponentsInChildren<BoardSpace>()[0]);
        spaces[31].SetPlayer(3);
        GameObject blueStart = CreaterHelper(startSpace, new Vector3(95, 0, 50), safeSpacesBlue, blueMat, 999);
        blueStart.GetComponent<StartSpace>().SetNext(spaces[33]);
        AddPieces(blueStart, board, blueMat, 3);

        BoardConnector(safeSpacesYellow.GetComponentsInChildren<BoardSpace>(), spaces[46]);
        spaces[45].SetSlider(4);
        spaces[46].SetNext(spaces[47]);
        spaces[46].SetSpecialNext(safeSpacesYellow.GetComponentsInChildren<BoardSpace>()[0]);
        spaces[46].SetPlayer(4);
        GameObject yellowStart = CreaterHelper(startSpace, new Vector3(50, 0, -100), safeSpacesYellow, yellowMat, 999);
        yellowStart.GetComponent<StartSpace>().SetNext(spaces[48]);
        AddPieces(yellowStart, board, yellowMat, 4);
        #endregion

        //Finish board connection
        spaces[0].SetPrevious(spaces[spaces.Length - 1]);
        spaces[spaces.Length - 1].SetNext(spaces[0]);

        //Set parent transforms under board
        boardSpaces.transform.SetParent(board.transform);
        safeSpacesYellow.transform.SetParent(board.transform);
        safeSpacesRed.transform.SetParent(board.transform);
        safeSpacesGreen.transform.SetParent(board.transform);
        safeSpacesBlue.transform.SetParent(board.transform);

        //Add center
        GameObject center = CreaterHelper(centerSpace, new Vector3(0, 0, 0), board, boardMat, 0);
        center.transform.localScale = new Vector3(20, 1, 20);
        center.transform.Rotate(new Vector3(0,-90,0)); ;

        //Set board scale
        board.transform.localScale = new Vector3(.25f, 1, .25f);

        foreach (GamePiece gamePiece in board.GetComponentsInChildren<GamePiece>()) { gamePiece.transform.parent = null; }

        Instantiate(cardDeck, new Vector3(0, .55f, 0), Quaternion.identity); //Add deck
    }

    //Creates boardspaces
    GameObject CreaterHelper(GameObject objectSpawn, Vector3 position, GameObject parent, Material material, int name)
    {
        GameObject spawn = Instantiate(objectSpawn, position, Quaternion.identity, parent.transform);
        spawn.GetComponentInChildren<MeshRenderer>().material = material;
        spawn.name = material.name + name + parent.name;
        return spawn;
    }

    //Connect spaces within an array in order
    void BoardConnector(BoardSpace[] spaces, BoardSpace previousSpace)
    {
        BoardSpace previous = previousSpace;
        foreach (BoardSpace space in spaces)
        {
            space.SetPrevious(previous);
            if (previous != null) previous.SetNext(space);
            previous = space;
        }
    }

    //Add gamepieces to home
    void AddPieces(GameObject start, GameObject parent, Material mat, int player)
    {
        PieceHolder[] nodes = start.GetComponentsInChildren<PieceHolder>();
        nodes[0].GotLandedOn(CreaterHelper(gamePiece, nodes[0].transform.position, parent, mat, 001).GetComponent<GamePiece>().Initializer(player, start.GetComponent<StartSpace>(), gameSpeed));
        nodes[1].GotLandedOn(CreaterHelper(gamePiece, nodes[1].transform.position, parent, mat, 002).GetComponent<GamePiece>().Initializer(player, start.GetComponent<StartSpace>(), gameSpeed));
        nodes[2].GotLandedOn(CreaterHelper(gamePiece, nodes[2].transform.position, parent, mat, 003).GetComponent<GamePiece>().Initializer(player, start.GetComponent<StartSpace>(), gameSpeed));
    }
}