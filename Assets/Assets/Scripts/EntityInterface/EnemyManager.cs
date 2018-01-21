using UnityEngine;
using UnityEngine.AI;

namespace EntityInterface
{
    public class EnemyManager : EntityManager
    {
        #region Variables
        public float lookRadius = 10f;
        public int BodyBaseValue = 1, MindBaseValue = 1, SoulBaseValue = 1;

        Transform target;
        NavMeshAgent agent;
        #endregion

        void Update()
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    // Attack the target
                    FaceTarget();
                }
            }
        }

        public override void Init()
        {
            // Instantiate Attributes
            myStats = new EntityStats(BodyBaseValue, MindBaseValue, SoulBaseValue);

            agent = GetComponent<NavMeshAgent>();
            target = PlayerManager.instance.EntitySelf.transform;
        }

        protected override void EntityDeath()
        {
            base.EntityDeath();

            // Add ragdoll effect / death animation
            Destroy(gameObject);
        }

        void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized.normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }   

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}