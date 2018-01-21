using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Attributes;


/// <summary>
/// Investiture is like a framework for using "magic"
/// </summary>
public interface IInvestiture
{
    string DisciplineName { get; }
    string DisciplineDesc { get; }
    string OrderName { get; }

    RawBonus BonusBody { get; }
    RawBonus BonusMind { get; }
    RawBonus BonusSoul { get; }
    RawBonus BonusHealth { get; }
    RawBonus BonusEnergy { get; }

    void Init();
    void GatherEnergy();
}
