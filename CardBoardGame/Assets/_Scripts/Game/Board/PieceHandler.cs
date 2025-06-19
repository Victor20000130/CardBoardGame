using UnityEngine;

public class PieceHandler : MonoBehaviour
{
    [SerializeField]
    private Transform[] piecePositions;
    public PlayerPiece playerPiece;

    public Transform MoveNextPos(int diceValue)
    {
        // TODO : 다음 포지션 계산하는 로직

        return piecePositions[diceValue];
    }

}
