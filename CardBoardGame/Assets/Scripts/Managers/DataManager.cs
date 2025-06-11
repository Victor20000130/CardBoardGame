using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private GridScriptableObject[] gridScriptableObjects;

    public GridScriptableObject[] GridScriptableObjects
    {
        get => gridScriptableObjects;
    }

}
