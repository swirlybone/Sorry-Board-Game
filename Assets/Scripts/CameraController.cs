/**
 * This script controls the camera. There are two modes:
 * "Board Camera" and "Piece Camera". The Board Camera
 * provides a top down view of the board, clicking and
 * dragging the mouse pans the camera. The Piece Camera
 * provides a third-person perspective of the last selected
 * game piece. Clicking and dragging the mouse rotates the camera.
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NotificationCenter;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 defaultPosition;
    [SerializeField] private Quaternion defaultRotation;
    [SerializeField] private Vector3 pieceCameraDistance = new Vector3(0f, 3f, -10f);
    [SerializeField] private string cameraType = "BOARD";
    [SerializeField] private float cameraSpeed = 2f;
    private GamePiece currentPiece;

    private float dist;
    private Vector3 MouseStart, MouseMove;

    List<GamePiece> pieces = new List<GamePiece>();

    // Set the default position and rotation of the camera at runtime.
    // The "dist" is the default distance from the camera in piece mode.
    private void Start()
    {
        defaultPosition = gameObject.transform.position;
        defaultRotation = gameObject.transform.rotation;
        
        dist = transform.position.z;

        pieces.AddRange(FindObjectsOfType<GamePiece>());
    }

    private void OnEnable()
    {
        AddObserver("Selected", Selected);
        AddObserver("Camera", cameraChange);
        AddObserver("ResetCamera", resetCamera);
        AddObserver("SelectNext", ChooseNext);
        AddObserver("SelectLast", ChoosePrevious);
    }

    private void OnDisable()
    {
        RemoveObserver("Selected", Selected);
        RemoveObserver("Camera", cameraChange);
        RemoveObserver("ResetCamera", resetCamera);
        RemoveObserver("SelectNext", ChooseNext);
        RemoveObserver("SelectLast", ChoosePrevious);
    }

    // General camera movement controller, simple click and drag. Updated every frame.
    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
        }
        else if (Input.GetMouseButton(1))
        {
            // Pan Camera in board mode
            if (cameraType == "BOARD")
            {
                MouseMove = new Vector3(Input.mousePosition.x - MouseStart.x, Input.mousePosition.y - MouseStart.y, dist);
                MouseStart = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                gameObject.transform.position = new Vector3(gameObject.transform.position.x + MouseMove.x * Time.deltaTime, gameObject.transform.position.y + MouseMove.y * Time.deltaTime, dist);
            }
            // Rotate camera in piece mode
            else
            {
                transform.Rotate(new Vector3(0, +Input.GetAxis("Mouse X") * cameraSpeed, 0));
                float X = transform.rotation.eulerAngles.x;
                float Y = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(X, Y, 0);
            }
        }
    }

    // Update the selected piece
    public void Selected(Notification notification)
    {
        currentPiece = (GamePiece)notification.Object;
        if (cameraType == "PIECE")
        {
            updatePieceCamera();
        }
    }

    // Swap camera based on UI element selected.
    public void cameraChange(Notification notification)
    {
        //Debug.Log((string)notification.Object);
        if ((string)notification.Object == "BOARD") {
            cameraType = "BOARD";
            transform.parent = null;
            gameObject.transform.position = defaultPosition;
            gameObject.transform.rotation = defaultRotation;
        }
        else
        {
            updatePieceCamera();
        }
    }

    // Used to update piece camera when mode is changed or new piece is selected.
    public void updatePieceCamera()
    {
        if (currentPiece == null)
        {
            currentPiece = pieces[Random.Range(0, pieces.Count)];
            updatePieceCamera();
        }
        else
        {
            cameraType = "PIECE";
            transform.SetParent(currentPiece.transform);
            transform.position = currentPiece.transform.position + pieceCameraDistance;
            transform.rotation = currentPiece.transform.rotation;
        }
    }

    void ChooseNext(Notification notification) { Iterate(1); updatePieceCamera(); }
    void ChoosePrevious(Notification notification) { Iterate(-1); updatePieceCamera(); }

    public void Iterate(int index) 
    {
        try { currentPiece = pieces[index + pieces.IndexOf(currentPiece)]; } catch { }
    }

    // Resets camera to default position and rotation, called by clicking ResetCamera button on UI
    public void resetCamera(Notification notification)
    {
        if (cameraType == "BOARD")
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }
        else
        {
            transform.position = currentPiece.transform.position + pieceCameraDistance;
            transform.rotation = currentPiece.transform.rotation;
        }
    }
}
