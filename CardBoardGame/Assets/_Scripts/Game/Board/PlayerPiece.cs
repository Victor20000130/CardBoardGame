using UnityEngine;

public class PlayerPiece : MonoBehaviour
{
    [SerializeField]
    private Transform[] gridPositions;




    private void Start()
    {
        if (gridPositions.Length != ManagerHandler.Instance.gameManager.GridLenght)
        {
            Debug.LogError("보드 이동 위치와 현재 그리드 불일치");
            return;
        }

    }
    public void MoveNextGrid(int diceValue)
    {

    }
}
