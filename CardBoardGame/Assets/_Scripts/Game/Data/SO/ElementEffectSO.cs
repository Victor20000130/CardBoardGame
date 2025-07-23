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

    private static readonly Dictionary<Operator, Func<float, float, float>> operatorFuncs = new()
    {
    { Operator.None,     (v, mod) => v },
    { Operator.Plus,     (v, mod) => v + mod },
    { Operator.Minus,    (v, mod) => v - mod },
    { Operator.Multiply, (v, mod) => v * mod },
    { Operator.Divide,   (v, mod) => v / mod },
    { Operator.Percent,  (v, mod) => v * (mod / 100f) },
    };

    // public float CardEffectCalc(float value, int level)
    // {
    //     level /= LevelPerUsedCards;
    //     Debug.Log($"Type: {ElementType}, Value: {value}, Level: {level}");
    //     foreach (ElementEffect elem in elementEffects)
    //     {
    //         switch (elem.Operator)
    //         {
    //             case Operator.None:
    //                 break;
    //             case Operator.Plus:

    //                 return value += elem.LevelPerValue[level];

    //             case Operator.Minus:

    //                 return value -= elem.LevelPerValue[level];

    //             case Operator.Multiply:

    //                 return value *= elem.LevelPerValue[level];

    //             case Operator.Divide:

    //                 return value /= elem.LevelPerValue[level];

    //             case Operator.Percent:

    //                 return value *= elem.LevelPerValue[level] / PercentBase;
    //         }
    //     }
    //     return 0;
    // }

    public float CardEffectCalc(EffectType effectType, float value, int level)
    {
        level /= LevelPerUsedCards;
        foreach (ElementEffect elem in elementEffects)
        {
            if (elem.EffectType == effectType)
            {
                Debug.Log($"적용된 효과: {effectType}");
                int safeLevel = Mathf.Clamp(level, 0, elem.LevelPerValue.Count - 1);
                return CalcByOperator(elem, value, safeLevel);
            }
        }
        return 0;
    }

    public float CalcByOperator(ElementEffect elem, float value, int safeLevel)
    {
        // switch (elem.Operator)
        // {
        //     case Operator.None:
        //         break;
        //     case Operator.Plus:

        //         return value += elem.LevelPerValue[level];

        //     case Operator.Minus:

        //         return value -= elem.LevelPerValue[level];

        //     case Operator.Multiply:

        //         return value *= elem.LevelPerValue[level];

        //     case Operator.Divide:

        //         return value /= elem.LevelPerValue[level];

        //     case Operator.Percent:

        //         return value *= elem.LevelPerValue[level] / PercentBase;
        // }
        // return 0;

        if (elem.LevelPerValue == null || elem.LevelPerValue.Count == 0)
        {
            return value;
        }

        float modifier = elem.LevelPerValue[safeLevel];

        if (operatorFuncs.TryGetValue(elem.Operator, out var opFunc))
        {
            return opFunc(value, modifier);
        }

        Debug.LogWarning($"정의되지 않은 연산자: {elem.Operator}");
        return value;
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
}