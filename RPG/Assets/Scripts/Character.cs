using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Character : MonoBehaviour
{
	
	[HideInInspector][AutoProperty]
	public MovementController movementController;
	[HideInInspector][AutoProperty]
	public SelectionController selectionController;
	
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void SelectTile()
	{
		selectionController.SwitchSelection();
	}
	
	public void MoveToTile()
	{
		if(selectionController.selected!=null)
		{
			movementController.MoveToTile(selectionController.selected);
			selectionController.DeSelectCurrent();
		}
	}
}
