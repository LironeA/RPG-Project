using UnityEngine;
using MyBox;

public class Character : MonoBehaviour
{
    [HideInInspector] [AutoProperty] public MovementController movementController;
    [HideInInspector] [AutoProperty] public SelectionController selectionController;

    public void SelectTile()
    {
        selectionController.SwitchSelection();
    }

    public void MoveToTile()
    {
        if (selectionController.selected == null) return;
        movementController.MoveToTile(selectionController.selected);
        selectionController.DeSelectCurrent();
    }
}