using UnityEngine;


public class PlayerPiece : MonoBehaviour
{
    private PieceHandler pieceHandler;

    private void Awake()
    {
        pieceHandler = GetComponentInParent<PieceHandler>();
        if (pieceHandler == null)
        {
            Debug.LogError("PieceHandler can't found");
            return;
        }
        pieceHandler.playerPiece = this;
    }

}

