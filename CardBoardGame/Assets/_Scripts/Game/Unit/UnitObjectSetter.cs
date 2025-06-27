using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitObjectSetter : MonoBehaviour
{
    [Obsolete]
    [SerializeField]
    private Slider hpBar;
    [Obsolete]
    public Slider HpBar => hpBar;

    [SerializeField]
    private TextMeshProUGUI _hpTMP;
    public TextMeshProUGUI HpTMP => _hpTMP;
    [SerializeField]
    private TextMeshProUGUI monsterDMG_TMP;
    public TextMeshProUGUI MonsterDMG_TMP => monsterDMG_TMP;
    [SerializeField]
    private TextMeshProUGUI monsterTurn_TMP;
    public TextMeshProUGUI MonsterTurn_TMP => monsterTurn_TMP;

    // public void SetDMG(int dmg)
    // {
    //     monsterDMG_TMP.text = dmg.ToString();
    // }

    // public void SetTurn(int turn)
    // {
    //     monsterTurn_TMP.text = turn.ToString();
    // }
}
