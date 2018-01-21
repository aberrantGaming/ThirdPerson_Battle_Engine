using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Investiture is like a framework for using "magic"
/// </summary>
public interface IInvestiture
{
    string Name { get; }
    string Desc { get; }
    int Body { get; }
    int Mind { get; }
    int Soul { get; }
    int Health { get; }
    int Energy { get; }

    void Init();    
}
