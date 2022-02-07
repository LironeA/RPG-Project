using UnityEngine;
using MyBox;
using DG.Tweening;

public class SelectionController : MonoBehaviour
{
    [Header("MovementOfTileSettings")]
    [SerializeField] [PositiveValueOnly] private float duration;
    [SerializeField] private float selectedTileHight;

    [HideInInspector] public Transform selected;
    private Transform _hitTransform;
    private Transform _cameraTransform;

    private void Awake()
    {
        if (Camera.main != null) _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out var hit, 100.0f,
                1 << LayerMask.NameToLayer("Tiles"))) _hitTransform = hit.transform;
    }


    public void SwitchSelection()
    {
        if (selected == _hitTransform)
        {
            DeSelect(_hitTransform);
        }
        else
        {
            DeSelect(selected);
            Select(_hitTransform);
        }
    }

    private void Select(Transform targetTransform)
    {
        targetTransform.DOMoveY(selectedTileHight, duration);
        selected = targetTransform;
    }

    private void DeSelect(Transform targetTransform)
    {
        targetTransform.DOMoveY(0, duration);
        selected = null;
    }

    public void DeSelectCurrent()
    {
        selected.DOMoveY(0, duration);
        selected = null;
    }
}