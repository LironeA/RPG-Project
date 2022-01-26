using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class InputManager : MonoBehaviour
{	
	[HideInInspector][AutoProperty]
	public Character character;
	[SearchableEnum]
	public KeyCode selectTile;
	[SearchableEnum]
	public KeyCode moveToTile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if(Input.GetKeyDown(selectTile))
	    {
	    	character.SelectTile();
	    }
	    if(Input.GetKeyDown(moveToTile))
	    {
	    	character.MoveToTile();
	    }
    }
}
