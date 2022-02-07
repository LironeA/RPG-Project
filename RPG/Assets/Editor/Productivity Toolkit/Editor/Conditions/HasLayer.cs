// Anthony Ackermans

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ToolExtensions
{
    public class HasLayer : Object, ICondition
    {
        private bool expanded;
        private int _layerSearchField = 0;

        public List<GameObject> Select()
        {
            var gameObjectByLayer = new List<GameObject>();
            var allGameObjects = FindObjectsOfType<GameObject>().ToList<GameObject>();

            foreach (var go in allGameObjects)
                if (go.layer == _layerSearchField)
                    gameObjectByLayer.Add(go);

            return gameObjectByLayer;
        }

        public void ShowUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            EditorGUI.indentLevel++;
            _layerSearchField = EditorGUILayout.LayerField("Select Layer", _layerSearchField);
            EditorGUI.indentLevel--;
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        public object GetValue()
        {
            throw new System.NotImplementedException();
        }

        public void ResetValues()
        {
            throw new System.NotImplementedException();
        }
    }
}