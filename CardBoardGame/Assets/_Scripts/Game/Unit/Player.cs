using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class Player : Unit
{

    private PlayerSO _playerSO;

    public PlayerSO PlayerSO
    {
        get => _playerSO;
        set => _playerSO = value;
    }
    private bool isDamageDouble;
    public bool IsDamageDouble
    {
        get => isDamageDouble;
        set => isDamageDouble = value;
    }
    protected override void OnInitialize()
    {
        _hpTMP = unitObjSetter.HpTMP;
        _hpTMP.text = _playerSO.CurHP.ToString();
        applyEffectAct += OnApplayEffect;
    }

    protected override void OnApplayEffect(GridType gridType)
    {
        switch (gridType)
        {
            case GridType.Start:
                break;
            case GridType.PlayerHeal:
                Heal();
                break;
            case GridType.Buff:
                Buff();
                break;
            default:
                print("효과 적용 안함");
                break;
        }
    }
    protected override void Heal()
    {
        print("플레이어 회복 효과");
        _playerSO.CurHP += _playerSO.CurHP + (_playerSO.MaxHP / 10);
        _hpTMP.text = _playerSO.CurHP.ToString();
    }

    protected override void Buff()
    {
        print("플레이어 버프 효과");
        isDamageDouble = true;

    }
}
