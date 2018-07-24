using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : AgentBehaviour
{
    public float targetRadius;
    public float slowRadius;
    public bool facing;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        facing = false;
        //no target don't turn;
        if (target == null)
            return steering;
        float targetOrientation = target.GetComponent<Agent>().orientation;
        //rotation is the difference between the orientations
        float rotation = targetOrientation - agent.orientation;
        //map between 180 and -180 degrees
        rotation = MapToRange(rotation);
        float rotationSize = Mathf.Abs(rotation);
        //if we are facing target already
        if (rotationSize < targetRadius)
        {
            facing = true;
            return steering;
        }   
        float targetRotation;
        //if we are nowhere near facing the target 
        if (rotationSize > slowRadius)
            //turn at max speed
            targetRotation = agent.maxRotation;
        else
        {
            facing = true;
            //slow rotation speed based on how close we are to directly facing target        
            targetRotation = agent.maxRotation * rotationSize / slowRadius;

        }
        //turn left or turn right
        targetRotation *= rotation / rotationSize;

        //apply rotation
        steering.angular = targetRotation - agent.rotation;
        steering.angular /= timeToTarget;

        //cap turn speed
        float angularAccel = Mathf.Abs(steering.angular);
        if(angularAccel > agent.maxAngularAccel)
        {
            steering.angular /= angularAccel;
            steering.angular *= agent.maxAngularAccel;
        }
        return steering;
    }
}
