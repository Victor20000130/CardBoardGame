using CardBoardGame.Assets._Scripts.Utility;
using TMPro;
using UnityEngine;

public class MiniGameHandler : Handler
{
    [SerializeField]
    private TZFZGame tZFZGame;

    [SerializeField]
    private TZFZSO[] tZFZ_SO;

    private TZFZSO currTZFZ_SO;
    protected override void OnInitialize()
    {
        Difficulty diff = ManagerHandler.Instance.dataManager.CurGameData.Difficulty;
        switch (diff)
        {
            case Difficulty.Easy:
                currTZFZ_SO = tZFZ_SO[0];
                break;
            case Difficulty.Normal:
                currTZFZ_SO = tZFZ_SO[1];
                break;
            case Difficulty.Hard:
                currTZFZ_SO = tZFZ_SO[2];
                break;
        }
    }

    protected override void SetHnadlerType()
    {
        handlerType = HandlerType.MiniGameHandler;
    }

    public void StartMiniGame(GridType gridType)
    {
        switch (gridType)
        {
            case GridType.MiniGame:
                tZFZGame.gameObject.SetActive(true);
                break;
        }
    }

    public void GetTZFZGameResult(int highestValue)
    {
        print($"HighestValue: {highestValue}");

        float damageMultiplierValue = currTZFZ_SO.DamageCalc(highestValue);
        print(damageMultiplierValue);
        ManagerHandler.Instance.gameManager.GetTZFZResult(damageMultiplierValue);
    }
}
