using UnityEngine;

namespace HitPointSystem
{
    [CreateAssetMenu(fileName = "Hit Points", menuName = "Hit Points/Hit Point Properties")]
    public class HitPoints : ScriptableObject
    {
        public enum HitPointType
        {
            None,
            Health,
            Armor,
            Shield
        }
        
        public HitPointType type;
        public int amount;
        public int maxAmount;
        public bool hasWeakness;
        public Damage.DamageType[] weaknesses;
        public bool hasImmunity;
        public Damage.DamageType[] immunities;
    }
}
