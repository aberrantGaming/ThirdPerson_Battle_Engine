using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes;
using Abilities;

public enum KnightsRadiantOrder
{
    Windrunner,
    Skybreaker
}

[CreateAssetMenu(menuName = "Surgebinder")]
public class Surgebinder : ScriptableObject {
    
    #region Variables
    public KnightsRadiantOrder CharacterOrder;

    public int BonusBodyValue = 2;
    public int BonusMindValue = 2;
    public int BonusSoulValue = 2;

    public Ability[] CharacterAbilities;
    
    public RawBonus BonusBody { get { return new RawBonus(BonusBodyValue); } }
    public RawBonus BonusMind { get { return new RawBonus(BonusMindValue); } }
    public RawBonus BonusSoul { get { return new RawBonus(BonusSoulValue); } }
    #endregion

    public void Init()
    {
        Debug.Log("Initializing Surgebinder: " + CharacterOrder);
    }

    public void GatherEnergy()
    {
        Debug.Log("Gathering Energy");

        // Locate nearby energy sources

        // Absorb the energy as a mana resource
    }
}
