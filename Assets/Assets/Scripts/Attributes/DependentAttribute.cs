using UnityEngine;
using System;
using System.Collections.Generic;

namespace Attributes
{
    public class DependantAttribute : Attribute
    {
        protected List<Attribute> _otherAttributes = new List<Attribute>();

        public DependantAttribute(int startingValue) : base(startingValue)
        {
            _otherAttributes.Clear();
        }

        public void AddAttribute(Attribute attr)
        {
            _otherAttributes.Add(attr);
        }

        public void RemoveAttribute(Attribute attr)
        {
            if (_otherAttributes.IndexOf(attr) >= 0)
            {
                _otherAttributes.RemoveAt(_otherAttributes.IndexOf(attr));
            }
        }

        public override int CalculateValue()
        {
            //specific attribute code goes somewhere in here

            _finalValue = BaseValue;

            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }
    }

    [System.Serializable]
    public class AttackCombatValue : DependantAttribute
    {
        [SerializeField]
        public AttackCombatValue(int startingValue) : base(startingValue)
        {
        }

        public override int CalculateValue()
        {
            _finalValue = BaseValue;

            // ACV = [(Body + Mind + Soul) / 3]
            int ACV = (_otherAttributes[0].CalculateValue() + _otherAttributes[1].CalculateValue() + _otherAttributes[2].CalculateValue()) / 3;

            _finalValue += Convert.ToInt32(ACV);

            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }
    }

    [System.Serializable]
    public class DefenseCombatValue : DependantAttribute
    {
        [SerializeField]
        public DefenseCombatValue(int startingValue) : base(startingValue)
        {
        }

        public override int CalculateValue()
        {
            _finalValue = BaseValue;

            // DCV = [(Body + Mind + Soul) / 2]
            int DCV = (_otherAttributes[0].CalculateValue() + _otherAttributes[1].CalculateValue() + _otherAttributes[2].CalculateValue()) / 2;

            _finalValue += Convert.ToInt32(DCV);
            //Debug.Log("DCV Base Value: " + BaseValue + " + ( [" + _otherAttributes[0].CalculateValue() + " + " + _otherAttributes[1].CalculateValue() + " + " + _otherAttributes[2].CalculateValue() + "] / 2 ) = " + _finalValue);

            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }
    }

    [System.Serializable]
    public class HealthPoints : DependantAttribute
    {
        [SerializeField]
        public HealthPoints(int startingValue) : base(startingValue)
        {
        }

        public override int CalculateValue()
        {
            _finalValue = BaseValue;

            // HP = [(Body + Soul) * 5]
            int HP = (_otherAttributes[0].CalculateValue() + _otherAttributes[1].CalculateValue()) * 5;

            _finalValue += Convert.ToInt32(HP);

            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }

    }

    [System.Serializable]
    public class EnergyPoints : DependantAttribute
    {
        [SerializeField]
        public EnergyPoints(int startingValue) : base(startingValue)
        {
        }

        public override int CalculateValue()
        {
            _finalValue = BaseValue;

            // EP = [(Mind + Soul) * 5]
            int EP = (_otherAttributes[0].CalculateValue() + _otherAttributes[1].CalculateValue()) * 5;

            _finalValue += Convert.ToInt32(EP);

            ApplyRawBonuses();

            ApplyFinalBonuses();

            return _finalValue;
        }

    }


}