using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(MeshCollider))]
    public class Interactable : MonoBehaviour
    {
        bool hasInteracted;

        public virtual void Interact()
        {
            // This method is meant to be overwritten
            Debug.Log("Interacting with " + transform.name);
        }

        void Start()
        {
            hasInteracted = false;
        }

        void OnTriggerEnter(Collider col)
        {
            if (!hasInteracted && col.gameObject.tag == "Player")
            {
                Interact();
                hasInteracted = true;
            }
                
        }
    }
}
