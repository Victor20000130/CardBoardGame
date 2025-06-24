using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardScriptableObject", menuName = "Scriptable Objects/CardScriptableObject")]
public class CardSO : ScriptableObject
{
    public List<Sprite> sprites;
    public List<CardData> cards;

    private void Awake()
    {
        for (int i = 0; i < 52; i++)
        {

        }

    }
}
