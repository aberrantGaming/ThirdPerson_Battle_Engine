using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEffect : MonoBehaviour
{
    #region Variables
    public float Duration;      // when it should expire
    public float StartTime;     // delay on the (first) effect tick
    public float RepeatTime;    // time between each effect tick

    [HideInInspector]
    public EntityInterface.Entity target;
    #endregion

    // Use this for initialization
    void Start ()
    {
        // Apply the effect repeated over time or direct
        if (RepeatTime > 0)
            InvokeRepeating("ApplyEffect", StartTime, RepeatTime);
        else
            Invoke("ApplyEffect", StartTime);

        // End the effect accordingly
        Invoke("EndEffect", Duration);
    }
	
    protected virtual void ApplyEffect()
    {

    }

    protected virtual void EndEffect()
    {
        CancelInvoke();
        Destroy(gameObject);
    }
}
