using System;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

public class Attributes : MonoBehaviour
{
        [Foldout("Basic Stats",true)]
        public CharacterAttribute healthPoints;
        public CharacterAttribute manaPoints;
        
        [Foldout("Basic Resists",true)]
        public CharacterAttribute armor;
        public CharacterAttribute magicResist;
        
        }
[Serializable]
public struct CharacterAttribute
{
        public string displayableName;
        [Multiline] public string description;
        public Image icon;
        public float value;
        public float maxValue;
}