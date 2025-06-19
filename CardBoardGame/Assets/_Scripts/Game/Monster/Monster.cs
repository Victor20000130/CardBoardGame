using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterSO _monsterData;

    public string Name => _monsterData._name;
    public int Health => _monsterData._health;
    public int Damage => _monsterData._damage;
    public int Turn => _monsterData._turn;

    public void TakeDamage(int damage)
    {
        _monsterData._health -= damage;
        if (_monsterData._health <= 0)
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
