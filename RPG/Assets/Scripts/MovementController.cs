using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    public void MoveToTile(Transform _tile)
    {
        var targetPosition = new Vector3(_tile.position.x, 1, _tile.position.z);
        transform.DOJump(targetPosition, 0.5f, 1, 2);
    }
}