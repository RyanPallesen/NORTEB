﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisHandler : MonoBehaviour
{


    public bool isPlacing = false;
    public TetrisPiece TetrisPiece;
    public GameObject tetrisObj;
    private static TetrisHandler _instance;
    private bool isValidPlacement;
    public List<GameObject> airList;
    public List<GameObject> metalList;
    public List<GameObject> fuelList;
    public GridPiece airGrid;
    public GridPiece metalGrid;
    public GridPiece fuelGrid;

    public static TetrisHandler Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (isPlacing && TetrisPiece)
        {
            tetrisObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tetrisObj.transform.Translate(new Vector3(0, 0, 189));
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
                isValidPlacement = false;

                for (int i = 0; i < tetrisObj.transform.childCount; i++)
                {
                    TetrisPiece.ResourceType CubeType = TetrisPiece.squares[i].resourceType;

                    //raycast backwards to see if there is a grid square behind

                    Transform workingTransform = tetrisObj.transform.GetChild(i);

                    if (Physics.Raycast(workingTransform.position, new Vector3(0, 0, 1), out RaycastHit hit))
                    {

                        if (hit.collider.transform.parent.GetComponent<GridPiece>())
                        {
                            TetrisPiece.ResourceType GridType = hit.collider.transform.parent.GetComponent<GridPiece>().resourceType;

                            //if gridtype == resourcetype, place.
                            //if gridtype != resourcetype, but grid space contains something, destroy both.
                            //if gridtype != resourcetype and grid is empty, destroy cube
                            //if none of the cubes are the right type, do not allow placement.

                            if (GridType == CubeType || CubeType == TetrisPiece.ResourceType.Flexible)
                            {
                                isValidPlacement = true;
                            }
                        }
                        else if (hit.collider.transform.GetComponent<TetrisTag>())
                        {
                           if(hit.collider.transform.GetComponent<TetrisTag>().ResourceType == CubeType)
                            {
                                isValidPlacement = true;
                            }
                        }
                    }
                    else
                    {
                        isValidPlacement = false;
                        return;
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
                            TetrisPiece.ResourceType CubeType = TetrisPiece.squares[i].resourceType;                   

                            if (hit.collider.transform.parent.GetComponent<GridPiece>())
                            {
                                TetrisPiece.ResourceType GridType = hit.collider.transform.parent.GetComponent<GridPiece>().resourceType;

                                Debug.Log(CubeType);
                                Debug.DrawRay(workingTransform.position, new Vector3(0, 0, 1));
                                //if gridtype == resourcetype, place.
                                if (GridType == CubeType || CubeType == TetrisPiece.ResourceType.Flexible)
                                {
                                    if (!hit.collider.transform.parent.GetComponent<GridPiece>())
                                    {
                                        Destroy(workingTransform.transform.gameObject);
                                    }
                                    else
                                    {
                                        workingTransform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z - 1f);

                                        if(GridType == TetrisPiece.ResourceType.Air)
                                        {
                                            airList.Add(workingTransform.gameObject);
                                            Debug.Log("Air element added");
                                        }
                                        else if(GridType == TetrisPiece.ResourceType.Metal)
                                        {
                                            metalList.Add(workingTransform.gameObject);
                                            Debug.Log("Metal element added");
                                        }
                                        else if(GridType == TetrisPiece.ResourceType.Fuel)
                                        {
                                            fuelList.Add(workingTransform.gameObject);
                                            Debug.Log("Fuel element added");
                                        }
                                    }
                                    //place it
                                }
                                else
                                {
                                    Destroy(workingTransform.gameObject);
                                }
                            }
                            else
                            {
                                Debug.Log("Hit resource for destruction");
                                Debug.Log(hit.collider.transform);

                                //if gridtype != resourcetype, but grid space contains something, destroy both.
                                if (!hit.collider.transform.parent.GetComponent<GridPiece>())
                                {
                                    Destroy(workingTransform.gameObject);

                                    if (hit.collider.GetComponent<TetrisTag>().ResourceType != CubeType)
                                    {

                                        if (airList.Contains(hit.collider.transform.gameObject))
                                        {
                                            airList.Remove(hit.collider.transform.gameObject);
                                            Debug.Log("Air element added");
                                        }
                                        else if (metalList.Contains(hit.collider.transform.gameObject))
                                        {
                                            metalList.Remove(hit.collider.transform.gameObject);
                                            Debug.Log("Air element added");
                                        }
                                        else if (fuelList.Contains(hit.collider.transform.gameObject))
                                        {
                                            fuelList.Remove(hit.collider.transform.gameObject);
                                            Debug.Log("Air element added");
                                        }

                                        Destroy(hit.collider.transform.gameObject);

                                    }

                                }
                                //if gridtype != resourcetype and grid is empty, destroy cube
                                else if (hit.collider.transform.parent.GetComponent<GridPiece>())
                                {
                                    Destroy(workingTransform.gameObject);
                                }
                            };
                        }
                    }

                    isPlacing = false;
                }

            }

        }
    }
}
