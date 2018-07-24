using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public int maxHealth = 1;
    public int hp;
    public float maxSpeed;
    public float maxAccel;
    public float maxRotation;
    public float maxAngularAccel;
    public Vector3 velocity;
    protected Steering steering;
    public float orientation;
    public float rotation;

	// Use this for initialization
	void Start ()
    {
        velocity = Vector3.zero;
        steering = new Steering();
        hp = maxHealth;
	}

    public void SetSteering(Steering steering)
    {
        this.steering = steering;
    }

    public virtual void Update()
    {
        Vector3 displacement = velocity * Time.deltaTime;
        orientation += rotation * Time.deltaTime;

        if (orientation < 0.0f)
            orientation += 360.0f;
        if (orientation > 360.0f)
            orientation -= 360.0f;

        transform.Translate(displacement, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.forward, orientation);

        if(hp < 1 )
        {
            Death();
        }
    }

    public virtual void LateUpdate()
    {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        if(velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
        if(steering.linear.sqrMagnitude == 0.0f)
        {
            velocity = Vector3.zero;
        }
        if(rotation > maxRotation)
        {
            rotation = maxRotation;
        }
        if (steering.angular == 0.0f)
        {
            rotation = 0.0f;
        }
        steering = new Steering();
    }

    public virtual void Death()
    {
        //end game, remove enemy, etc
        Destroy(gameObject);
    }

    public virtual Vector3 OriToVec(float orientation)
    {
        Vector3 vector = Vector3.zero;
        vector.y = Mathf.Sin(orientation * Mathf.Deg2Rad) * 1.0f;
        vector.x = Mathf.Cos(orientation * Mathf.Deg2Rad) * 1.0f;
        return vector.normalized;
    }
}
