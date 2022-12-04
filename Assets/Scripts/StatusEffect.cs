using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "StatusEffect", fileName = "StatusEffect")]
[System.Serializable]
public class StatusEffect : ScriptableObject
{
    [SerializeField] EnemyStat effectType;
    public EnemyStat EffectType => effectType;

    [SerializeField, Tooltip("In Seconds")] int duration;
    public int Duration
    {
        get
        {
            if (!stackable) return duration;
            else
            {
                return stacks.Count > 0 ? stacks.Max() : 0;
            }
        }
        private set { duration = value; }
    }

    [SerializeField] float value;
    public float Value
    {
        get
        {
            if (!stackable) return value;
            else
            {
                return value * stacks.Count;
            }
        }
    }

    [SerializeField] bool stackable = false;
    public bool Stackable => stackable;

    List<int> stacks = new List<int>();

    public int GetIntValue()
    {
        return Mathf.RoundToInt(Value);
    }

    public void AddStack()
    {
        stacks.Add(duration);
    }

    public void CountDownOneSecond()
    {
        if (stackable)
        {
            for (int i = 0; i < stacks.Count; i++)
            {
                stacks[i]--;
                if (stacks[i] <= 0)
                {
                    stacks.RemoveAt(i);
                }
            }
        }
        else
        {
            Duration--;
        }
    }

    public void SetDuration(int duration)
    {
        this.duration = duration;
    }


}
