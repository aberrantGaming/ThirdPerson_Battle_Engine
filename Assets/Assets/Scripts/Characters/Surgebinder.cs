using Skills;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "New Player Class", menuName = "Characters/Surgebinder")]
    public class Surgebinder : ScriptableObject
    {
        new public string name = "New Order";
        public Sprite icon = null;

        public int BodyBonus = 0;
        public int MindBonus = 0;
        public int SoulBonus = 0;
        public int HealthBonus = 0;
        public int EnergyBonus = 0;
        public int AcvBonus = 0;
        public int DcvBonus = 0;
        
        public Ability Passive1;
        public Ability Ability1;
        public Ability Ability2;
        public Ability Ability3;             
    }
}