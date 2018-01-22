using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes;

public class Windrunner : Surgebinding
{
    public new string OrderName = "Windrunners";

    public override void Init()
    {   
        // Apply new bonuses for this job
        bodyBonus = new Attribute(1);
        mindBonus = new Attribute(0);
        soulBonus = new Attribute(0);
        healthBonus = new Attribute(10);
        energyBonus = new Attribute(5);

        Debug.Log("Initialized " + OrderName);
    }
}
