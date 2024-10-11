using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "Game/Player Setting")]
public class PlayerSetting : ScriptableObject
{
    public float speed;
    public float damagePerSecond;
    public float attackRadius;
}
