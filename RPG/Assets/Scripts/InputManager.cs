using UnityEngine;
using MyBox;

public class InputManager : MonoBehaviour
{
    [HideInInspector] [AutoProperty] public Character character;
    [SearchableEnum] public KeyCode selectTile;

    [SearchableEnum] public KeyCode moveToTile;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(selectTile)) character.SelectTile();
        if (Input.GetKeyDown(moveToTile)) character.MoveToTile();
    }
}