using UnityEngine;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    protected Animator _anim;
    protected Slider hpBar;
    protected UnitObjectSetter unitObjSetter;
    protected ScriptableObject unitSO;

    public ScriptableObject UnitSO
    {
        get => unitSO;
        set => unitSO = value;
    }
    protected void Awake()
    {
        unitObjSetter = gameObject.GetComponentInParent<UnitObjectSetter>();
        Initialize();
    }
    protected void Initialize()
    {
        hpBar = unitObjSetter.HpBar;
        _anim = GetComponent<Animator>();
        OnInitialize();
    }

    protected abstract void OnInitialize();
}
