using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "Scriptable Objects/CardScriptableObject")]
public class CardSO : ScriptableObject
{
    public List<Sprite> cards;
}
