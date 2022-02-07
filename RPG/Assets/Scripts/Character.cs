using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Character : MonoBehaviour
{
    [HideInInspector] [AutoProperty] public MovementController movementController;
    [HideInInspector] [AutoProperty] public SelectionController selectionController;


    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

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