using System;
using System.Collections.Generic;
using CardBoardGame.Assets._Scripts.Utility;
using DG.Tweening;
using TMPro;
using UnityEditor.Animations;
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

    protected Dictionary<string, float> animClipDic = new Dictionary<string, float>();

    public void Initialize()
    {
        _anim = GetComponent<Animator>();
        // 차후 체력바 기능 넣고 싶으면 작업
        // hpBar = unitObjSetter.HpBar;
        OnInitialize();
        var ctrler = _anim.runtimeAnimatorController as AnimatorController;

        foreach (var layer in ctrler.layers)
        {
            foreach (var state in layer.stateMachine.states)
            {
                var clip = state.state.motion as AnimationClip;
                animClipDic.Add(state.state.name, clip.length);
            }
        }
    }
    public virtual float GetAnimationClipLength(string stateName)
    {
        return animClipDic[stateName];
    }
    protected abstract void OnInitialize();
    protected abstract void OnApplayEffect(GridType gridType);
    protected abstract void Heal();
    public abstract void Buff(bool isBuff);

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
