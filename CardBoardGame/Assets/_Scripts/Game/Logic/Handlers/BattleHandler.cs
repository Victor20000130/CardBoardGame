using System;
using System.Collections;
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
        player = FindAnyObjectByType<Player>();
        monster = FindAnyObjectByType<Monster>();

    }

    public void ReceiveMonsterSO(MonsterSO monsterSO)
    {
        curMonsterSO = ScriptableObject.CreateInstance<MonsterSO>();
        monsterSO.Copy(curMonsterSO);
        player.PlayerSO = curPlayerSO;
        monster.MonsterSO = curMonsterSO;
        player.Initialize();
        monster.Initialize();
    }

    private void SODataLoad()
    {
        originPlayerSO = Resources.Load<PlayerSO>("Data/PlayerSO");
        curPlayerSO = ScriptableObject.CreateInstance<PlayerSO>();
        originPlayerSO.Initialize(curPlayerSO);
        Debug.Log(curPlayerSO.Name);
    }

    public void SendGridType(GridType gridType, CardHandler cardHandler)
    {
        switch (gridType)
        {
            case GridType.Start:
                break;
            case GridType.Day:
                break;
            case GridType.Night:
                break;
            case GridType.PlayerHeal:
                break;
            case GridType.MonsterHeal:
                break;
            case GridType.Buff:
                break;
            case GridType.MiniGame:
                // MiniGameHandler.GetGridType(gridData.gridType);
                //TODO MiniGameHandler 제작 예정
                break;
            default:
                Debug.LogError($"잘못된 그리드 타입 {gridType}");
                break;
        }
        StartCoroutine(ApplyEffect(gridType, cardHandler));
    }

    private IEnumerator ApplyEffect(GridType gridType, CardHandler cardHandler)
    {
        print($"적용되는 효과 : {gridType}");
        player.applyEffectAct?.Invoke(gridType);
        monster.applyEffectAct?.Invoke(gridType);
        yield return new WaitForSeconds(3f);
    }
    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.BattleHandler;
    }
}
