using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisHandler : MonoBehaviour
{


    public bool isPlacing = false;
    public TetrisPiece TetrisPiece;
    public GameObject tetrisObj;
    private static TetrisHandler _instance;
    bool isValidPlacement;

    public static TetrisHandler Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {

    }


    void Update()
    {
        if (isPlacing && TetrisPiece)
        {
            tetrisObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tetrisObj.transform.Translate(new Vector3(0, 0, 190));
            if (Input.GetKeyDown(KeyCode.D))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, -90));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, 90));
            }

            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < tetrisObj.transform.childCount; i++)
                {
                    //raycast backwards to see if there is a grid square behind

                    Transform workingTransform = tetrisObj.transform.GetChild(i);

                    if (Physics.Raycast(workingTransform.position, new Vector3(0, 0, 1), out RaycastHit hit))
                    {
                        if (hit.collider.transform.parent.GetComponent<GridPiece>())
                        {
                            TetrisPiece.ResourceType GridType = hit.collider.transform.parent.GetComponent<GridPiece>().resourceType;
                            TetrisPiece.ResourceType CubeType = TetrisPiece.squares[i].resourceType;

                            //if gridtype == resourcetype, place.
                            //if gridtype != resourcetype, but grid space contains something, destroy both.
                            //if gridtype != resourcetype and grid is empty, destroy cube
                            //if none of the cubes are the right type, do not allow placement.

                            if (GridType == CubeType || CubeType == TetrisPiece.ResourceType.Flexible)
                            {
                                isValidPlacement = true;
                            }
                        }
                    }
                }

                if (isValidPlacement)
                {



                    for (int i = 0; i < tetrisObj.transform.childCount; i++)
                    {
                        //raycast backwards to see if there is a grid square behind

                        Transform workingTransform = tetrisObj.transform.GetChild(i);

                        if (Physics.Raycast(workingTransform.position, new Vector3(0, 0, 1), out RaycastHit hit))
                        {
                            if (hit.collider.transform.parent.GetComponent<GridPiece>())
                            {
                                TetrisPiece.ResourceType GridType = hit.collider.transform.parent.GetComponent<GridPiece>().resourceType;
                                TetrisPiece.ResourceType CubeType = TetrisPiece.squares[i].resourceType;

                                //if gridtype == resourcetype, place.
                                if (GridType == CubeType || CubeType == TetrisPiece.ResourceType.Flexible)
                                {
                                    //place it
                                    workingTransform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 1f);

                                    Debug.Log("Correct Grid");
                                }
                                //if none of the cubes are the right type, do not allow placement.
                                else if (GridType != CubeType)
                                {
                                    Debug.Log("Placement not permitted");
                                }
                                //if gridtype != resourcetype, but grid space contains something, destroy both.
                                else if (GridType != CubeType && !hit.collider.transform.parent.GetComponent<GridPiece>())
                                {
                                    Destroy(gameObject);
                                    Destroy(hit.transform.gameObject);

                                }
                                //if gridtype != resourcetype and grid is empty, destroy cube
                                else if (GridType != CubeType && hit.collider.transform.parent.GetComponent<GridPiece>())
                                {
                                    Destroy(gameObject);
                                }
                                //don't overwrite correct cubes 
                                else if (GridType == CubeType && !hit.collider.transform.parent.GetComponent<GridPiece>())
                                {
                                    Destroy(workingTransform.transform.gameObject);

                                }
                            }
                        }
                    }

                    isPlacing = false;
                }

            }

        }
    }
}
