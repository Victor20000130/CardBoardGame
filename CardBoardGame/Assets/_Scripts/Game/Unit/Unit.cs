using System;
using CardBoardGame.Assets._Scripts.Utility;
using DG.Tweening;
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
    [SerializeField]
    protected UnitObjectSetter unitObjSetter;
    protected void Start()
    {
        if (unitObjSetter == null)
        {
            unitObjSetter = gameObject.GetComponentInParent<UnitObjectSetter>();
        }
        if (unitObjSetter == null)
        {
            Debug.LogError($"{gameObject.name} UnitOBJSetter is Null");
        }
        _anim = GetComponent<Animator>();
    }
    public void Initialize()
    {

        // 차후 체력바 기능 넣고 싶으면 작업
        // hpBar = unitObjSetter.HpBar;
        OnInitialize();
    }

    protected abstract void OnInitialize();
    protected abstract void OnApplayEffect(GridType gridType);
    protected abstract void Heal();
    protected abstract void Buff();

    public abstract void ReflectUI();

    public virtual bool TakeDamage(float damage)
    {
        _anim.SetTrigger("Damaged");
        return false;
    }
    public virtual void Attack()
    {
        _anim.SetTrigger("Attack");
    }

}
