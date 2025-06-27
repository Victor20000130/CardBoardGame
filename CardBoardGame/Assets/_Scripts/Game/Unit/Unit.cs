using System;
using CardBoardGame.Assets._Scripts.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    protected Animator _anim;
    [Obsolete]
    protected Slider hpBar;
    protected TextMeshProUGUI _hpTMP;
    public Action<GridType> applyEffectAct;
    public TextMeshProUGUI HpTMP
    {
        get => _hpTMP;
        set => _hpTMP = value;
    }

    protected UnitObjectSetter unitObjSetter;

    public void Initialize()
    {
        unitObjSetter = gameObject.GetComponentInParent<UnitObjectSetter>();
        // 차후 체력바 기능 넣고 싶으면 작업
        // hpBar = unitObjSetter.HpBar;
        _anim = GetComponent<Animator>();

        OnInitialize();
    }

    protected abstract void OnInitialize();
    protected abstract void OnApplayEffect(GridType gridType);
    protected abstract void Heal();
    protected abstract void Buff();

}
