using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementEffectSO", menuName = "Scriptable Objects/ElementEffectSO")]
public class ElementEffectSO : ScriptableObject
{
    [SerializeField]
    private List<ElementEffect> elementEffects;
    public List<ElementEffect> ElementEffects => elementEffects;

    public float GetEffectValue(int value)
    {


        return value;
    }

}

[Serializable]
public class ElementEffect
{
    [SerializeField]
    private ElementType elementType;
    public ElementType ElementType => elementType;

    [SerializeField]
    private EffectType effectType;
    public EffectType EffectType => effectType;

    [SerializeField]
    private Operator _operator;
    public Operator Operator => _operator;

    [SerializeField]
    private List<float> levelPerValue;
    public List<float> LevelPerValue => levelPerValue;

}