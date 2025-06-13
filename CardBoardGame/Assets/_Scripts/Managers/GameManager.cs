using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GridScriptableObject currentGridData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetCrruentGridData();
    }

    private void GetCrruentGridData()
    {
        currentGridData = ManagerHandler.Instance.dataManager.GridScriptableObjects[0];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
