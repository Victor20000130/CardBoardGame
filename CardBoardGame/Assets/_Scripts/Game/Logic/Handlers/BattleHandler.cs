using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class BattleHandler : Handler
{
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator monsterAnim;
    [SerializeField] private PlayerSO playerSO;
    private MonsterSO curMonsterSO;
    public MonsterSO CurMonsterSO
    {
        get => curMonsterSO;
        set => curMonsterSO = value;
    }
    private void Awake()
    {

    }
    protected override void OnInitialize()
    {
        SODataLoad();
    }

    private void SODataLoad()
    {
        playerSO = Resources.Load<PlayerSO>("Data/PlayerSO");
    }

    public void GetGridType(GridType gridType)
    {

    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.BattleHandler;
    }
}
