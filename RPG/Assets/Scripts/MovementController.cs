using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void MoveToTile(Transform _tile)
	{
		Vector3 targetPosition = new Vector3(_tile.position.x,1,_tile.position.z);
		this.transform.DOJump(targetPosition,0.5f,1,2);
	}
}
