﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisHandler : MonoBehaviour {


    public bool isPlacing = false;
    public TetrisPiece TetrisPiece;
    public GameObject tetrisObj;
    private static TetrisHandler _instance;

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
    
    void Start () 
    {
		
	}
	
	
	void Update ()
    {
		if(isPlacing && TetrisPiece)
        {
            tetrisObj.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tetrisObj.transform.Translate(new Vector3(0, 0, 190));
            if(Input.GetKeyDown(KeyCode.D))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, -90));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, 90));
            }

            if(Input.GetMouseButton(0))
            {
                for(int i = 0; i < tetrisObj.transform.childCount; i++)
                {
                    //raycast backwards to see if there is a grid square behind

                    Transform workingTransform = tetrisObj.transform.GetChild(i);

                    if(Physics.Raycast(workingTransform.position,new Vector3(0,0,1), out RaycastHit hit))
                    {
                        if(hit.collider.transform.parent.GetComponent<GridPiece>())
                        {
                            workingTransform.position = hit.collider.transform.position;
                            isPlacing = false;

                        }
                    }

                }
            }

        }
	}
}
