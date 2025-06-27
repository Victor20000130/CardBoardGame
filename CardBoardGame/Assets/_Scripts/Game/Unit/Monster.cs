using CardBoardGame.Assets._Scripts.Utility;
using TMPro;
using UnityEngine;

public class Monster : Unit
{
    private TextMeshProUGUI monsterDMGTMP;
    private TextMeshProUGUI monsterTurnTMP;
    private MonsterSO _monsterSO;
    public MonsterSO MonsterSO
    {
        get => _monsterSO;
        set => _monsterSO = value;
    }
    private float _damage;
    protected override void OnInitialize()
    {
        if (unitObjSetter == null)
        {
            unitObjSetter = this.gameObject.GetComponentInParent<UnitObjectSetter>();
        }

        _hpTMP = unitObjSetter.HpTMP;
        monsterDMGTMP = unitObjSetter.MonsterDMG_TMP;
        monsterTurnTMP = unitObjSetter.MonsterTurn_TMP;
        _hpTMP.text = _monsterSO._curHP.ToString();
        monsterDMGTMP.text = _monsterSO._damage.ToString();
        monsterTurnTMP.text = _monsterSO._turn.ToString();
        applyEffectAct += OnApplayEffect;
        _damage = _monsterSO._damage;
    }

    protected override void OnApplayEffect(GridType gridType)
    {
        switch (gridType)
        {
            case GridType.Day:
                AtDay();
                break;
            case GridType.Night:
                AtNight();
                break;
            case GridType.MonsterHeal:
                Heal();
                break;
            case GridType.Buff:
                Buff();
                break;
            default:
                break;
        }
    }

    private void AtNight()
    {
        print("밤 효과");
        _damage = MonsterSO._damage * 2;
        monsterDMGTMP.text = _damage.ToString();
    }
    private void AtDay()
    {
        print("낮 효과");
        _damage = MonsterSO._damage;
        monsterDMGTMP.text = _damage.ToString();
    }

    protected override void Heal()
    {
        print("몬스터 회복 효과");
        float hp = MonsterSO._curHP + (MonsterSO._maxHP / 10);
        if (hp > MonsterSO._maxHP)
        {
            MonsterSO._curHP = MonsterSO._maxHP;
        }
        else
        {
            MonsterSO._curHP = hp;
        }
        _hpTMP.text = MonsterSO._curHP.ToString();
    }

    protected override void Buff()
    {
        print("몬스터 버프 효과");
        MonsterSO._turn += 1;
    }
}
