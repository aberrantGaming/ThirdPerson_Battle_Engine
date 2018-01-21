using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skills;

/// <summary>
/// Surgebinding is a discipline of Investiture. Surgebinders benefit from default
///     passive abilities, but do not have active abilities of their own.
/// </summary>
public class Surgebinding : IInvestiture {

    public string Name { get { return "Surgebinder"; } }
    public string Desc { get { return "SurgeBinder_Description"; } }

    public int Body { get { return 3; } }
    public int Mind { get { return 3; } }
    public int Soul { get { return 3; } }

    public int Health { get { return 10; } }
    public int Energy { get { return 10; } }

    protected Ability[] availableAbilities;

    public void Init()
    {
        Debug.Log("Initialized Surgebinder.");
    }
}
