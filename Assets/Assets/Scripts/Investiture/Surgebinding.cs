using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes;
using Skills;

/// <summary>
/// Surgebinding is a discipline of Investiture. Surgebinders benefit from default
///     passive abilities, but do not have active abilities of their own.
/// </summary>
public class Surgebinding : IInvestiture
{
    #region Variables

    #region Interface Members
    public string DisciplineName { get { return "Surgebinder"; } }
    public string DisciplineDesc { get { return "SurgeBinder_Description"; } }
    public string OrderName { get; set; }

    public RawBonus BonusBody { get { return new RawBonus(bodyBonus.CalculateValue()); } }
    public RawBonus BonusMind { get { return new RawBonus(mindBonus.CalculateValue()); } }
    public RawBonus BonusSoul { get { return new RawBonus(soulBonus.CalculateValue()); } }
    public RawBonus BonusHealth { get { return new RawBonus(soulBonus.CalculateValue()); } }
    public RawBonus BonusEnergy { get { return new RawBonus(soulBonus.CalculateValue()); } }
    #endregion

    #region Base Attribute Bonuses
    protected Attribute bodyBonus = new Attribute(3);
    protected Attribute mindBonus = new Attribute(3);
    protected Attribute soulBonus = new Attribute(3);
    protected Attribute healthBonus = new Attribute(10);
    protected Attribute energyBonus = new Attribute(10);
    #endregion

    protected Ability[] availableAbilities;

    #endregion

    public virtual void Init()
    {
        
    }

    public void GatherEnergy()
    {
        Debug.Log("Gathering Energy");

        // Locate nearby energy sources

        // Absorb the energy as a mana resource
    }

    protected void SetOrder(string _orderName)
    {
        OrderName = _orderName;
    }
}
