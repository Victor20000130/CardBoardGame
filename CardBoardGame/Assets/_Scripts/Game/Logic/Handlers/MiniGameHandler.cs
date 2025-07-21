using CardBoardGame.Assets._Scripts.Utility;
using TMPro;
using UnityEngine;

public class MiniGameHandler : Handler
{
    [SerializeField]
    private TZFZGame tZFZGame;
    protected override void OnInitialize()
    {
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
                StartCoroutine(tZFZGame.TZFZCorou());
                break;
        }
    }
}
