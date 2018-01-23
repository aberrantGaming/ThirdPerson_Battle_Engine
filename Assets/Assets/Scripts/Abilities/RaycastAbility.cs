using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu (menuName = "Ability/RaycastAbility")]
    public class RaycastAbility : Ability
    {
        public int baseDamage = 1;
        public float weaponRange = 50f;
        public float hitForce = 100f;
        public Color laserColor = Color.white;

        private Abilities.RaycastShootTriggerable rcShoot;

        public override void Init(GameObject obj)
        {
            rcShoot = obj.GetComponent<RaycastShootTriggerable>();
            rcShoot.Initialize();

            rcShoot.gunDamage = baseDamage;
            rcShoot.weaponRange = weaponRange;
            rcShoot.hitForce = hitForce;
            rcShoot.laserLine.material = new Material(Shader.Find("Unlit/Color"))
            {
                color = laserColor
            };
        }

        public override void TriggerAbility()
        {
            rcShoot.Fire();
            Debug.Log("Ability Fired: " + name);
        }
    }
}
