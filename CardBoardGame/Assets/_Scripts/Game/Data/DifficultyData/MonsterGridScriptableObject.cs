using UnityEngine;
[CreateAssetMenu(fileName = "DifficultyScriptableObject", menuName = "Scriptable Objects/DifficultyScriptableObject")]
public class MonsterGridScriptableObject : ScriptableObject
{
    [SerializeField] private MonsterScriptableObject[] monsterScriptableObjects;
    [SerializeField] private GridScriptableObject[] gridScriptableObjects;
}
