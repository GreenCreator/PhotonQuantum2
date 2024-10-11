using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "ScriptableObjects/PlayerSetting", order = 1)]
public class PlayerSetting : ScriptableObject
{
    [Header("Main stats")] 
    public float speed = 1f;
    public float damagePerSecond = 10f;
    public float attackRadius = 2f;


    [Header("Step upgrade")] 
    public float speedStep = 0.5f;
    public float damageStep = 10f;
    public float radiusStep = 1f;

    [Header("Upgrade Probabilities")] 
    [Range(0, 1)] public float speedUpgradeProbability = 0.6f;
    [Range(0, 1)] public float damageUpgradeProbability = 0.3f;
    [Range(0, 1)] public float attackRadiusUpgradeProbability = 0.1f;

    public void NormalizeProbabilities()
    {
        float totalProbability = speedUpgradeProbability + damageUpgradeProbability + attackRadiusUpgradeProbability;
        speedUpgradeProbability /= totalProbability;
        damageUpgradeProbability /= totalProbability;
        attackRadiusUpgradeProbability /= totalProbability;
    }
}