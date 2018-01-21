using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes;

public class Windrunner : Surgebinding
{
    private string orderName = "Windrunners";

    private RawBonus bodyBonusValue = new RawBonus(1);
    private RawBonus mindBonusValue = new RawBonus(1);
    private RawBonus soulBonusValue = new RawBonus(1);
    private RawBonus energyBonusValue = new RawBonus(10);
    private RawBonus healthBonusValue = new RawBonus(10);

    public override void Init()
    {
        OrderName = orderName;

        bodyBonus.AddRawBonus(bodyBonusValue);
        mindBonus.AddRawBonus(mindBonusValue);
        soulBonus.AddRawBonus(soulBonusValue);
        healthBonus.AddRawBonus(healthBonusValue);
        energyBonus.AddRawBonus(energyBonusValue);
        
        Debug.Log("Initialized " + orderName);
    }
}
