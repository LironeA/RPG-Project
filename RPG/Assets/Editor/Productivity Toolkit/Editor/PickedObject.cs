//Anthony Ackermans

using UnityEditor;
using UnityEngine;

namespace ToolExtensions
{
    public class PickedObject
    {
        public GameObject TheGameObject;
        public Vector3 OriginalPosition;
        public Quaternion OriginalRotation;
        public Vector3 OriginalScale;
        public PrefabAssetType AssetType;

        public PickedObject(GameObject gameObject)
        {
            TheGameObject = gameObject;
            OriginalPosition = TheGameObject.GetComponent<Transform>().position;
            OriginalRotation = TheGameObject.GetComponent<Transform>().rotation;
            OriginalScale = TheGameObject.GetComponent<Transform>().localScale;
            AssetType = PrefabUtility.GetPrefabAssetType(gameObject);
        }
    }
}