using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterSO _monsterData;

    public string Name => _monsterData._name;
    public float MaxHP => _monsterData._maxHP;
    public float Damage => _monsterData._damage;
    public int Turn => _monsterData._turn;

    public void TakeDamage(int damage)
    {
        _monsterData._maxHP -= damage;
        if (_monsterData._maxHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle monster death logic here
        Debug.Log($"{Name} has died.");
        Destroy(gameObject);
    }
}
