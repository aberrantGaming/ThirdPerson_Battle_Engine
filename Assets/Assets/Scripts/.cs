using System.Collections.Generic;
using UnityEngine;
using Interactables;

public class EquipmentManager : MonoBehaviour
{

    #region Singleton

    public static EquipmentManager instance;

    void Awake()
    {
        instance = this;
    }

#endregion
    
    
}