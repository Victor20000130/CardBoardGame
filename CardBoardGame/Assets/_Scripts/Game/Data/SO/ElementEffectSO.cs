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

    /// <summary>
    /// Dictionary<ElementType, ElementEffect> dictionary를 생성하여 초기화
    /// </summary>
    /// <param name="effectDic"></param>
    public void Initialize(Dictionary<ElementType, ElementEffect> effectDic)
    {
        foreach (ElementEffect elementEffect in elementEffects)
        {
            effectDic.Add(elementEffect.ElementType, elementEffect);
        }
    }

}

[Serializable]
public class ElementEffect
{
    private const int PercentBase = 100;
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

    public float EffectCalc(float value, int levelPerValue)
    {
        Debug.Log($"현재 속성 {elementType}/적용된 연산식 {_operator}/value {value}/levelPerVal {levelPerValue}");
        float returnValue = value;
        switch (_operator)
        {
            case Operator.None:
                break;
            case Operator.Plus:
                return value += levelPerValue;
            case Operator.Minus:
                return value -= levelPerValue;
            case Operator.Multiply:
                return value *= levelPerValue;
            case Operator.Divide:
                return value /= levelPerValue;
            case Operator.Percent:

                return value *= levelPerValue / PercentBase;
        }
        Debug.LogError("연산식 적용 실패");
        return 0;
    }

    public int EffectCalc(int value, int levelPerValue)
    {
        Debug.Log($"현재 속성 {elementType}/적용된 연산식 {_operator}/value {value}/levelPerVal {levelPerValue}");
        float returnValue = value;
        switch (_operator)
        {
            case Operator.None:
                break;
            case Operator.Plus:
                return value += levelPerValue;
            case Operator.Minus:
                return value -= levelPerValue;
            case Operator.Multiply:
                return value *= levelPerValue;
            case Operator.Divide:
                return value /= levelPerValue;
            case Operator.Percent:

                return value *= levelPerValue / PercentBase;
        }
        Debug.LogError("연산식 적용 실패");
        return 0;
    }
}