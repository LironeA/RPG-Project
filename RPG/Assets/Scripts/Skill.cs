using System;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

[Serializable] [CreateAssetMenu(fileName = "Skill", menuName = "RPG/Skill", order = 1)]
public class Skill : ScriptableObject
{
    public string displayableName;
    [Multiline] public string description;
    public Image icon;

    public ResourceUsage resourceUsage = ResourceUsage.None;
    
    [ConditionalField(nameof(resourceUsage), false, ResourceUsage.Stamina)] public float staminaCost;
    [ConditionalField(nameof(resourceUsage), false, ResourceUsage.Mana)] public float manaCost;
    
    
    public SkillType skillType;

}

public enum ResourceUsage 
{
    None,
    Stamina,
    Mana
}
    
public enum SkillType 
{
    Target,
    NonTarget,
    Area
}
