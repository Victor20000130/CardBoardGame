using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GridScriptableObject", menuName = "Scriptable Objects/GridScriptableObject")]
public class GridScriptableObject : ScriptableObject
{
    [SerializeField]
    private GridData[] gridData;
}
