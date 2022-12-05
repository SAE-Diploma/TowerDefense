using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    public override void SetCheckPoints(List<Transform> checkPoints)
    {
        base.SetCheckPoints(checkPoints);
        for (int i = 0;i< this.checkpoints.Count;i++)
        {
            this.checkpoints[i] += Vector3.up * 10;
        }
    }
}
