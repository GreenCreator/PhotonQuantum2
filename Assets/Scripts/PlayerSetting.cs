using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "ScriptableObjects/PlayerSetting", order = 1)]
public class PlayerSetting : ScriptableObject
{
    public float speed;
    public float damagePerSecond;
    public float attackRadius;
}
