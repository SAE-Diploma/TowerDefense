using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Towers
{
    Ballista = 0,
    Minigun = 1,
    Sniper = 2
}

public enum TowerStat
{
    Attackspeed = 0,
    Damage = 1,
    Range = 2,
    ProjectileSpeed = 3
}

public enum EffectType
{
    None,
    Fire,
    Poison,
    Slowness,
}
