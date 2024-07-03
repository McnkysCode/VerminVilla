using UnityEngine;

[CreateAssetMenu(fileName = "enemyData", menuName = "Create Enemy Data", order = 0)]
public class EnemyData : ScriptableObject
{
    public int Health;
    public int Speed;
    public int Value;
    public int Damage;
}