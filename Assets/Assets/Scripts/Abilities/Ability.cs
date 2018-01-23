using UnityEngine;
using System.Collections;

namespace Abilities
{
    public abstract class Ability : ScriptableObject
    {

        public string aName = "New Ability";
        public Sprite aSprite;
        //public AudioClip aSound;
        public float aBaseCoolDown = 1f;

        public abstract void Init(GameObject obj);
        public abstract void TriggerAbility();
    }
}