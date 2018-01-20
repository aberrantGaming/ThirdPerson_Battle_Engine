using UnityEngine;
using System;
using System.Collections.Generic;

namespace Attributes
{
    [System.Serializable]
    public class Attribute : BaseAttribute
    {
        private List<RawBonus> _rawBonuses = new List<RawBonus>();
        private List<FinalBonus> _finalBonuses = new List<FinalBonus>();

        protected int _finalValue;

        public Attribute(int startingValue) : base(startingValue) {   

            _rawBonuses.Clear();
            _finalBonuses.Clear();

            _finalValue = BaseValue;
        }

        public void AddRawBonus(RawBonus bonus)
        {
            _rawBonuses.Add(bonus);
        }

        public void AddFinalBonus(FinalBonus bonus)
        {
            _finalBonuses.Add(bonus);
        }

        public void RemoveRawBonus(RawBonus bonus)
        {
            if (_rawBonuses.IndexOf(bonus) >= 0)
            {
                _rawBonuses.RemoveAt(_rawBonuses.IndexOf(bonus));
            }
        }

        public void RemoveFinalBonus(FinalBonus bonus)
        {
            if (_finalBonuses.IndexOf(bonus) >= 0)
            {
                _finalBonuses.RemoveAt(_finalBonuses.IndexOf(bonus));
            }
        }

        protected void ApplyRawBonuses()
        {
            //Adding value from raw
            int rawBonusValue = 0;
            float rawBonusMultiplier = 0;

            foreach (RawBonus bonus in _rawBonuses)
            {
                rawBonusValue += bonus.BaseValue;
                rawBonusMultiplier += bonus.BaseMultiplier;
            }

            _finalValue += rawBonusValue;
            _finalValue *= Convert.ToInt32((1 + rawBonusMultiplier));
        }

        protected void ApplyFinalBonuses()
        {
            //Adding value from Final
            int finalBonusValue = 0;
            float finalBonusMultiplier = 0;

            foreach (FinalBonus bonus in _finalBonuses)
            {
                finalBonusValue += bonus.BaseValue;
                finalBonusMultiplier += bonus.BaseMultiplier;
            }

            _finalValue += finalBonusValue;
            _finalValue *= Convert.ToInt32((1 + finalBonusMultiplier));
        }

        public virtual int CalculateValue()
        {
            _finalValue = BaseValue;
            
            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }
        
        public int FinalValue()
        {
            return CalculateValue();
        }
    }
}