using TMPro;
using UnityEngine;

public class Monster : Unit
{
    private TextMeshProUGUI monsterDMGTMP;
    private TextMeshProUGUI monsterTurnTMP;

    protected override void OnInitialize()
    {
        monsterDMGTMP = unitObjSetter.MonsterDMG_TMP;
        monsterTurnTMP = unitObjSetter.MonsterTurn_TMP;
    }
    public void TakeDamage(int damage)
    {

    }

    private void Die()
    {

    }
}
