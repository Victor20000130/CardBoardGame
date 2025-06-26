using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitObjectSetter : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;
    public Slider HpBar => hpBar;
    [SerializeField]
    private TextMeshProUGUI monsterDMG_TMP;
    public TextMeshProUGUI MonsterDMG_TMP => monsterDMG_TMP;
    [SerializeField]
    private TextMeshProUGUI monsterTurn_TMP;
    public TextMeshProUGUI MonsterTurn_TMP => monsterTurn_TMP;
}
