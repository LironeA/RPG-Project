using UnityEngine;
using DG.Tweening;

public class MovementController : MonoBehaviour
{
    public void MoveToTile(Transform tile)
    {
        var position = tile.position;
        var targetPosition = new Vector3(position.x, 1, position.z);
        transform.DOJump(targetPosition, 0.5f, 1, 2);
    }
}