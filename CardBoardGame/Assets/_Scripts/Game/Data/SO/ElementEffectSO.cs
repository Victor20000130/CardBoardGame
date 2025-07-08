using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ElementEffectSO", menuName = "Scriptable Objects/ElementEffectSO")]
public class ElementEffectSO : ScriptableObject
{
    private const int LevelPerUsedCards = 10;
    private const float PercentBase = 100;

    [SerializeField]
    private ElementType elementType;
    public ElementType ElementType => elementType;
    [SerializeField]
    private List<ElementEffect> elementEffects;
    public List<ElementEffect> ElementEffects => elementEffects;

    public float CardEffectCalc(float value, int level)
    {
        level /= LevelPerUsedCards;
        Debug.Log($"Type: {ElementType}, Value: {value}, Level: {level}");
        foreach (ElementEffect elem in elementEffects)
        {
            switch (elem.Operator)
            {
                case Operator.None:
                    Debug.Log($"적용된 연산식: {elem.Operator} ");
                    break;
                case Operator.Plus:
                    Debug.Log($"적용된 연산식: {elem.Operator}, LevelPerValue: {elem.LevelPerValue}");
                    Debug.Log($"결과값: {value + elem.LevelPerValue[level]}");
                    return value += elem.LevelPerValue[level];
                case Operator.Minus:
                    Debug.Log($"적용된 연산식: {elem.Operator} ");
                    Debug.Log($"결과값: {value - elem.LevelPerValue[level]}");
                    return value -= elem.LevelPerValue[level];
                case Operator.Multiply:
                    Debug.Log($"적용된 연산식: {elem.Operator} ");
                    Debug.Log($"결과값: {value * elem.LevelPerValue[level]}");
                    return value *= elem.LevelPerValue[level];
                case Operator.Divide:
                    Debug.Log($"적용된 연산식: {elem.Operator} ");
                    Debug.Log($"결과값: {value / elem.LevelPerValue[level]}");
                    return value /= elem.LevelPerValue[level];
                case Operator.Percent:
                    Debug.Log($"적용된 연산식: {elem.Operator} ");
                    Debug.Log($"결과값: {value * (elem.LevelPerValue[level] / PercentBase)}");
                    return value *= elem.LevelPerValue[level] / PercentBase;
            }
        }
        return 0;
    }
}

[Serializable]
public class ElementEffect
{

    [SerializeField]
    private EffectType effectType;
    public EffectType EffectType => effectType;

    [SerializeField]
    private Operator _operator;
    public Operator Operator => _operator;

    [SerializeField]
    private List<float> levelPerValue;
    public List<float> LevelPerValue => levelPerValue;

    // public float EffectCalc(float value, int level)
    // {
    //     switch (_operator)
    //     {
    //         case Operator.None:
    //             Debug.Log($"적용된 연산식: {_operator} ");
    //             break;
    //         case Operator.Plus:
    //             Debug.Log($"적용된 연산식: {_operator} ");
    //             Debug.Log($"결과값: {value + levelPerValue[level]}");
    //             return value += levelPerValue[level];
    //         case Operator.Minus:
    //             Debug.Log($"적용된 연산식: {_operator} ");
    //             Debug.Log($"결과값: {value - levelPerValue[level]}");
    //             return value -= levelPerValue[level];
    //         case Operator.Multiply:
    //             Debug.Log($"적용된 연산식: {_operator} ");
    //             Debug.Log($"결과값: {value * levelPerValue[level]}");
    //             return value *= levelPerValue[level];
    //         case Operator.Divide:
    //             Debug.Log($"적용된 연산식: {_operator} ");
    //             Debug.Log($"결과값: {value / levelPerValue[level]}");
    //             return value /= levelPerValue[level];
    //         case Operator.Percent:
    //             Debug.Log($"적용된 연산식: {_operator} ");
    //             Debug.Log($"결과값: {value * (levelPerValue[level] / PercentBase)}");
    //             return value *= levelPerValue[level] / PercentBase;
    //     }
    //     return 0;
    // }

}