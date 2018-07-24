
using UnityEngine;

public class ConditionAgentHealthLower : Condition
{
    public Agent target;
    public float targetPercent;

    public override bool Test()
    {
        if (target == null)
            return false;
        float percent = (target.hp / (float)target.maxHealth);
        if (percent <= targetPercent)
            return true;
        return false;
    }
}
