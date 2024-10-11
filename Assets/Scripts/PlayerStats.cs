using UniMob;
using UnityEngine;

public class PlayerStats : LifetimeMonoBehaviour
{
    // Реактивные свойства с помощью [Atom]
    [Atom] public float Speed { get; set; } = 5f;
    [Atom] public float Damage { get; set; } = 10f;
    [Atom] public float AttackRadius { get; set; } = 3f;

    // Методы для прокачки героя
    private void UpgradeSpeed(float value)
    {
        Speed += value;
    }

    private void UpgradeDamage(float value)
    {
        Damage += value;
    }

    private void UpgradeAttackRadius(float value)
    {
        AttackRadius += value;
    }
    
    public void RandomUpgrade()
    {
        float randomValue = Random.value; // Случайное число от 0 до 1

        if (randomValue <= 0.60f)
        {
            UpgradeSpeed(0.5f); // Прокачка скорости
            Debug.Log("Speed upgraded!");
        }
        else if (randomValue <= 0.90f)
        {
            UpgradeDamage(10f); // Прокачка урона
            Debug.Log("Damage upgraded!");
        }
        else
        {
            UpgradeAttackRadius(1f); // Прокачка радиуса атаки
            Debug.Log("Attack radius upgraded!");
        }
    }
}