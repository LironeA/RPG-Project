using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using DG.Tweening; 

public class SelectionController : MonoBehaviour
{
	[SerializeField][PositiveValueOnly]
	private float duration;
	[SerializeField]
	private float hightOfSelectionTile;
	
	[HideInInspector]
	public Transform selected;
	private Transform hitTransform;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		RaycastHit hit;
		if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100.0f,1 << LayerMask.NameToLayer("Tiles")))
		{
			hitTransform = hit.transform;
		}
	}
    
    
	public void SwitchSelection()
	{
		if(selected == hitTransform)
		{
			DeSelect(hitTransform);
		} else
		{
			DeSelect(selected);
			Select(hitTransform);
		}
	}
	
	void Select(Transform _transform)
	{
		_transform.DOMoveY(hightOfSelectionTile,duration);
		selected = _transform;
	}
	
	void DeSelect(Transform _transform)
	{
		_transform.DOMoveY(0,duration);
		selected = null;
	}
	
	public void DeSelectCurrent()
	{
		selected.DOMoveY(0,duration);
		selected = null;
	}
	
}
