using System;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class BattleHandler : Handler
{
    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    [SerializeField] private PlayerSO originPlayerSO;
    private PlayerSO curPlayerSO;
    private MonsterSO curMonsterSO;
    public MonsterSO CurMonsterSO
    {
        get => curMonsterSO;
        set => curMonsterSO = value;
    }

    protected override void OnInitialize()
    {
        SODataLoad();
    }

    public void ReceiveMonsterSO(MonsterSO monsterSO)
    {
        curMonsterSO = ScriptableObject.CreateInstance<MonsterSO>();
        monsterSO.Copy(curMonsterSO);
    }

    private void SODataLoad()
    {
        originPlayerSO = Resources.Load<PlayerSO>("Data/PlayerSO");
        curPlayerSO = ScriptableObject.CreateInstance<PlayerSO>();
        originPlayerSO.Initialize(curPlayerSO);
        Debug.Log(curPlayerSO.Name);
    }

    public void SetGridType(GridType gridType)
    {
        switch (gridType)
        {
            case GridType.Day:
                curPlayerSO.IsNight = false;
                break;
            case GridType.Night:
                curPlayerSO.IsNight = true;
                break;
            case GridType.PlayerHeal:
                curPlayerSO.IsHeal = true;
                break;
            case GridType.MonsterHeal:
                curMonsterSO.IsHeal = true;
                break;
            case GridType.Buff:
                curPlayerSO.IsBuff = true;
                break;
            case GridType.MiniGame:
                curPlayerSO.IsMiniGame = true;
                // MiniGameHandler.GetGridType(gridData.gridType);
                //TODO MiniGameHandler 제작 예정
                break;
            default:
                Debug.LogError($"잘못된 그리드 타입 {gridType}");
                break;
        }
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.BattleHandler;
    }
}
