using System.Collections;
using UnityEngine;

public class Seek : AgentBehaviour
{
    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        //move towards target
        steering.linear = target.transform.position - transform.position;
        steering.linear.Normalize();
        steering.linear *= agent.maxAccel;

        return steering;
    }
}
