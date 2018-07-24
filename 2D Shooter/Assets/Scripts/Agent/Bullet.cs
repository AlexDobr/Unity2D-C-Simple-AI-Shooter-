using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : AgentBehaviour
{
    private Steering steering;
    public GameObject parent;
    private float lifetime = 5f;
    private float counter = 0f;

    public override void Awake()
    {
        base.Awake();
        steering = new Steering();
    }

    public void OnEnable()
    {
        if(target != null)
        {
            if (steering.linear == new Vector3())
            {
                steering.linear = target.transform.position - transform.position;
            }
            
        }
        steering.linear.Normalize();
        steering.linear *= agent.maxAccel;
    }

    // Update is called once per frame
    public override void Update ()
    {
        counter += Time.deltaTime;

        base.Update();

        if(counter >= lifetime)
        {
            Destroy(gameObject);
        }
	}

    public override Steering GetSteering()
    {
        return steering;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Agent>() && other.gameObject != parent)
        {
            other.gameObject.GetComponent<Agent>().hp -= agent.hp;

            Destroy(gameObject);
        }
    }
}
