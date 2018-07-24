using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align {

    [HideInInspector]
    public GameObject targetAux;
    public Vector3 direction;

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

    public override Steering GetSteering()
    {
        if(targetAux != null)
        {
            Agent targetAgent = targetAux.GetComponent<Agent>();
            direction = (targetAux.transform.position + targetAgent.velocity) - transform.position;
            if(direction.magnitude > 0.0f)
            {
                direction.Normalize();

                float targetOrientation = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 180;
                target.GetComponent<Agent>().orientation = targetOrientation - 270;
            }
        }
        return base.GetSteering();
    }
}
