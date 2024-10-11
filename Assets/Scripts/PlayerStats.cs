using UniMob;
using UnityEngine;

public class PlayerStats : LifetimeMonoBehaviour
{
    // Реактивные свойства с помощью [Atom]
    [Atom] public float Speed { get; set; } = 5f;
    [Atom] public float Damage { get; set; } = 10f;
    [Atom] public float AttackRadius { get; set; } = 3f;

    [Header("Player Setting")] 
    [SerializeField] private PlayerSetting playerSetting;
    protected void Awake()
    {
        // Инициализация реактивных свойств из ScriptableObject
        Speed = playerSetting.speed;
        Damage = playerSetting.damagePerSecond;
        AttackRadius = playerSetting.attackRadius;
        
        playerSetting.NormalizeProbabilities();
    }
    
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
        float randomValue = Random.value;

        if (randomValue <= playerSetting.speedUpgradeProbability)
        {
            UpgradeSpeed(playerSetting.speedStep);
            Debug.Log("Speed upgraded!");
        }
        else if (randomValue <= playerSetting.speedUpgradeProbability + playerSetting.damageUpgradeProbability)
        {
            UpgradeDamage(playerSetting.damageStep);
            Debug.Log("Damage upgraded!");
        }
        else
        {
            UpgradeAttackRadius(playerSetting.radiusStep);
            Debug.Log("Attack radius upgraded!");
        }
    }
}