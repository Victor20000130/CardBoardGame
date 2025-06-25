using CardBoardGame.Assets._Scripts.Utility;
using UnityEngine;

public abstract class Handler : MonoBehaviour
{
    protected HandlerType handlerType;
    public HandlerType HandlerType => handlerType;
    protected bool isSetHandlerType = false;
    protected bool isInitialized = false;
    protected void Start()
    {

    }
    public void SetHandlerType()
    {
        if (isSetHandlerType)
        {
            return;
        }
        isSetHandlerType = true;
        SetHnadlerType();
    }
    public void Initialize()
    {
        if (isInitialized)
        {
            return;
        }
        isInitialized = true;
        OnInitialize();
    }
    protected abstract void OnInitialize();
    protected abstract void SetHnadlerType();
}