using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public class Handler : MonoBehaviour, IInitializable
{
    protected HandlerType handlerType;
    public HandlerType HandlerType => handlerType;
    protected bool isInitialized = false;
    protected void Start()
    {
        if (handlerType == HandlerType.None)
        {
            Debug.LogError("Handler Type is None");
        }
    }
    public virtual void Initialize()
    {
        if (isInitialized)
        {
            Debug.Log("Already Initialized");
            return;
        }
        isInitialized = true;
        gameObject.SetActive(true);
    }
}