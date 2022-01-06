using System;
using UnityEngine;

namespace HitPointSystem
{
    public class HitPointsComponent : MonoBehaviour
    {
        public HitPoints[] hitPointsArray;
        public bool isAlive = true;

        private void Start()
        {
            if (hitPointsArray.Length <= 0) return;
            foreach (var hp in hitPointsArray)
            {
                if (hp.amount > hp.maxAmount)
                    hp.amount = hp.maxAmount;
                    
                if (hp.amount < 0)
                    hp.amount = 0;
            }
        }

        private HitPoints.HitPointType HPSwitch(int index)
        {
            switch (index)
            {
                case 1:
                    return HitPoints.HitPointType.Health;
                case 2:
                    return HitPoints.HitPointType.Armor;
                case 3:
                    return HitPoints.HitPointType.Shield;
            }

            return HitPoints.HitPointType.None;
        }
        
        private HitPoints FindOutermostHitPoints()
        {
            for (var i = hitPointsArray.Length - 1; i >= 0; i--)
            {
                if (hitPointsArray[i].amount > 0)
                    return hitPointsArray[i];
            }

            return ScriptableObject.CreateInstance<HitPoints>();
        }
        
        public void AddHPTarget(HitPoints.HitPointType type, int amount)
        {
            foreach (var hp in hitPointsArray)
                if (hp.type == type)
                    ModifyHitPointValue(hp, amount, false);
        }

        public void ReduceHPTarget(HitPoints.HitPointType type, int amount)
        {
            foreach (var hp in hitPointsArray)
                if (hp.type == type)
                    ModifyHitPointValue(hp, amount, true);
        }

        public void AddHPGeneral()
        {
            
        }

        public void ReduceHPGeneral()
        {
            
        }

        public void ModifyHitPointValue(HitPoints hp, int amount, bool damage)
        {
            switch (damage)
            {
                case true:
                    hp.amount -= amount;
                    break;
                case false:
                    hp.amount += amount;
                    break;
            }

            if (hp.amount > hp.maxAmount)
                hp.amount = hp.maxAmount;

            if (hp.amount < 0)
                hp.amount = 0;
        }
    }
}
