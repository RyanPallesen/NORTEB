using System.Collections;
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
            tetrisObj.transform.position = Input.mousePosition;

            if(Input.GetKeyDown(KeyCode.D))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, -90));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                tetrisObj.transform.Rotate(new Vector3(0, 0, 90));
            }
        }
	}
}
