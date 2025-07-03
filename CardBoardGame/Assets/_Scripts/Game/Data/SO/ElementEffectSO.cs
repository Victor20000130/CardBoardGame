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

    private Dictionary<ElementType, ElementEffect> effectDic;

    public float GetEffectValue(ElementType elementType, float value, int elementLevel)
    {
        switch (elementType)
        {
            case ElementType.None:
                Debug.Log("적용된 효과 없음");
                break;
            case ElementType.Embers:

                break;
            case ElementType.Spray:

                break;
            case ElementType.Nuri:

                break;
            case ElementType.Fair_Wind:

                break;
        }
        Debug.Log($"{elementType} 효과 적용");

        return value;
    }
    public int GetEffectValue(ElementType elementType, int value, int elementLevel)
    {
        switch (elementType)
        {
            case ElementType.None:
                Debug.Log("적용된 효과 없음");
                break;
            case ElementType.Embers:

                break;
            case ElementType.Spray:

                break;
            case ElementType.Nuri:

                break;
            case ElementType.Fair_Wind:

                break;
        }
        Debug.Log($"{elementType} 효과 적용");
        return value;
    }

    public void Initialize()
    {
        effectDic = new Dictionary<ElementType, ElementEffect>();
        foreach (ElementEffect elementEffect in elementEffects)
        {
            effectDic.Add(elementEffect.ElementType, elementEffect);
        }
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