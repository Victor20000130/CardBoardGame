using System;
using UnityEngine;
[CreateAssetMenu(fileName = "TZFZScriptableObject", menuName = "Scriptable Objects/TZFZScriptableObject")]
public class TZFZSO : ScriptableObject
{
    [SerializeField]
    private TZFZDamageRanking[] tZFZDamageRankings;

    public float DamageCalc(int vaule)
    {
        foreach (TZFZDamageRanking ranking in tZFZDamageRankings)
        {
            // Debug.Log(ranking)
            if (vaule >= ranking.MinInclusive && vaule < ranking.MaxExclusive)
            {
                return ranking.DamageMultiplierValue;
            }
        }
        return 1;
    }
}
[Serializable]
public class TZFZDamageRanking
{
    [SerializeField]
    private TZFZPuzzle tZFZPuzzle;

    [SerializeField]
    private int minInclusive;
    public int MinInclusive => minInclusive;
    [SerializeField]
    private int maxExclusive;
    public int MaxExclusive => maxExclusive;
    [SerializeField]
    private float damageMultiplierValue;
    public float DamageMultiplierValue => damageMultiplierValue;

}
