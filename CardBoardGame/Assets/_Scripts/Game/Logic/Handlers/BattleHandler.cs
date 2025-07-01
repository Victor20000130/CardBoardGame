using System;
using System.Collections;
using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class BattleHandler : Handler
{
    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    [SerializeField] private PlayerSO originPlayerSO;
    [SerializeField] private MonsterSO originMonsterSO;
    private PlayerSO curPlayerSO;
    private MonsterSO curMonsterSO;
    public MonsterSO CurMonsterSO
    {
        get => curMonsterSO;
        set => curMonsterSO = value;
    }

    private void Start()
    {
        Debug.Log(curPlayerSO);
        player.PlayerSO = curPlayerSO;
        player.Initialize();

    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        monster = FindAnyObjectByType<Monster>();
        originPlayerSO = Resources.Load<PlayerSO>("Data/PlayerSO");
        curPlayerSO = ScriptableObject.CreateInstance<PlayerSO>();
        originPlayerSO.Copy(curPlayerSO);
    }

    public void ReceiveMonsterSO(MonsterSO monsterSO)
    {
        curMonsterSO = monsterSO;
        monster.MonsterSO = curMonsterSO;
        monster.Initialize();
    }

    public void SODataLoad()
    {

    }

    public void SendGridType(GridType gridType)
    {
        StartCoroutine(ApplyEffect(gridType));
    }

    private IEnumerator ApplyEffect(GridType gridType)
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
