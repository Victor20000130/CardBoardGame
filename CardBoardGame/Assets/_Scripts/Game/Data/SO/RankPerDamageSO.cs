using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

[CreateAssetMenu(fileName = "RankPerDamageSO", menuName = "Scriptable Objects/RankPerDamageSO")]
public class RankPerDamageSO : ScriptableObject
{
    [Serializable]
    public class RankDamagePair
    {
        public HandRankings handRanking;
        public float damage;
    }

    public List<RankDamagePair> rankDamageList = new List<RankDamagePair>();

    public float GetDamage(HandRankings rankings)
    {
        foreach (var pair in rankDamageList)
        {
            if (pair.handRanking == rankings)
            {
                return pair.damage;
            }
        }
        return 0;
    }

}
