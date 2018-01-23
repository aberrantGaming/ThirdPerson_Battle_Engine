using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(menuName = "Ability/Selfcast Ability")]
    public class SelfcastAbility : Ability
    {
        public SelfcastAbilityTriggerable scTrigger;

        public override void Init(GameObject obj)
        {
            throw new System.NotImplementedException();
        }

        public override void TriggerAbility()
        {
            throw new System.NotImplementedException();
        }
    }
}
