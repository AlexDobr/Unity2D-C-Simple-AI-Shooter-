using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemy : State
{
    public float fireRate = 1.0f;
    public GameObject bulletPrefab;
    public GameObject target;
    public float targetRadius;
    public float slowRadius;

    [HideInInspector]
    public Agent agent;
    public bool facing;
    public GameObject targetAux;
    public Vector3 direction;
    public float timeToTarget = 0.1f;
    private float counter = 0.0f;


    public override void Awake()
    {
        base.Awake();
        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
    }

    private void OnDestroy()
    {
        Destroy(target);
    }

    public override void Update()
    {
        counter += Time.deltaTime;

        if(counter >= fireRate)
        {
            if(targetAux != null && facing == true)
            {
                Fire();
                counter = 0.0f;
            }
        }
    }

    public virtual Steering Face(Steering steering)
    {
        if(targetAux != null)
        {
            Agent targetAgent = targetAux.GetComponent<Agent>();
            direction = (targetAux.transform.position + targetAgent.velocity) - transform.position;
            if(direction.magnitude > 0.0f)
            {
                direction.Normalize();
            }

            float targetOrientation = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 180;
            target.GetComponent<Agent>().orientation = targetOrientation - 270;
        }

        steering = Align(steering);
        return steering;
    }

    public virtual Steering Align(Steering steering)
    {
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
        if (angularAccel > agent.maxAngularAccel)
        {
            steering.angular /= angularAccel;
            steering.angular *= agent.maxAngularAccel;
        }
        return steering;
    }

    public float MapToRange(float rotation)
    {
        rotation %= 360.0f;
        if (Mathf.Abs(rotation) > 180.0f)
        {
            if (rotation < 0.0f)
                rotation += 360.0f;
            else
                rotation -= 360.0f;
        }
        return rotation;
    }

    private void Fire()
    {
        GameObject curBullet = Instantiate<GameObject>(bulletPrefab, transform.position, Quaternion.identity, null);
        Bullet bullet = curBullet.GetComponent<Bullet>();
        curBullet.SetActive(false);
        bullet.parent = gameObject;
        bullet.target = targetAux;
        bullet.GetSteering().linear = direction;
        curBullet.SetActive(true);
    }
}
