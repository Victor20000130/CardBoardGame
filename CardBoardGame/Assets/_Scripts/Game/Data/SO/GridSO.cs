using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GridScriptableObject", menuName = "Scriptable Objects/GridScriptableObject")]
public class GridSO : ScriptableObject
{
    [SerializeField]
    private GridData[] gridData;
    public GridData[] GridDataArray => gridData;
    public int GridDataLength => gridData.Length;
}
