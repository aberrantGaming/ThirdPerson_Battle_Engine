using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Attributes
{
    public class BaseAttribute
    {
        [SerializeField] private int _baseValue;
        [SerializeField] private float _baseMultiplier;
        
        public BaseAttribute(int value, float multiplier = 0)
        {
            _baseValue = value;
            _baseMultiplier = multiplier;
        }
        
        public int BaseValue
        {
            get { return _baseValue; }
            set { _baseValue = value; }
        }

        public float BaseMultiplier
        {
            get { return _baseMultiplier; }
            set { _baseMultiplier = value; }
        }
    }
     [System.Serializable]
    public class RawBonus : BaseAttribute
    {
        [SerializeField]
        public RawBonus(int value = 0, float multiplier = 0) : base(value, multiplier)
        {
        }
    }

    public class FinalBonus : BaseAttribute
    {
        private Timer _timer;
        private Attribute _parent;

        public FinalBonus(int time, int value = 0, float multiplier = 0) : base(value, multiplier)
        {
            _timer = new Timer(time);
            _timer.Elapsed += OnTimerEnd;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public void StartTimer(Attribute parent)
        {
            _parent = parent;
            _timer.Start();
        }

        private void OnTimerEnd(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
                            e.SignalTime);

            _parent.RemoveFinalBonus(this);
        }
    }
}