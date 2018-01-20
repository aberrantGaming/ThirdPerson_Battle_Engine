using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu (menuName = "Abilities/RaycastAbility")]
    public class RaycastAbility : Ability
    {
        //public int gunDamage = 1;
        //public float weaponRange = 50f;
        //public float hitForce = 100f;
        //public Color laserColor = Color.white;

        //private Skills.RaycastShootTriggerable rcShoot;

        public override void Initialize(GameObject obj)
        {
            //rcShoot = obj.GetComponent<RaycastShootTriggerable>();
            //rcShoot.Initialize();

            //rcShoot.gunDamage = gunDamage;
            //rcShoot.weaponRange = weaponRange;
            //rcShoot.hitForce = hitForce;
            //rcShoot.laserLine.material = new Material(Shader.Find("Unlit/Color"));
            //rcShoot.laserLine.material.color = laserColor;

        }

        public override void Fire()
        {
            //rcShoot.Trigger();
            Debug.Log("Ability Fired: " + name);
        }        
    }
}
