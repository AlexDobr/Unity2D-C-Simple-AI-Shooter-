using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHeal : StateEnemy
{
    public GameObject[] potions;
    public GameObject potionAux;
    public float minDistance;
    public float targetPercent;

    public override void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
        base.Awake();


        //is player close
        ConditionTargetClose ctc = new ConditionTargetClose();
        ctc.origin = this.gameObject;
        ctc.target = targetAux;
        ctc.minDistance = minDistance;

        //if player far away
        ConditionTargetFar ctf = new ConditionTargetFar();
        ctf.origin = this.gameObject;
        ctf.target = targetAux;
        ctf.maxDistance = minDistance;

        //is health higher than %
        ConditionAgentHealthHigher cahh = new ConditionAgentHealthHigher();
        cahh.target = agent;
        cahh.targetPercent = targetPercent;

        //is both above true
        ConditionAnd ca = new ConditionAnd();
        ca.conditionA = ctc;
        ca.conditionB = cahh;

        ConditionAnd ca2 = new ConditionAnd();
        ca2.conditionA = ctf;
        ca2.conditionB = cahh;

        //transition to wander state
        State st = GetComponent<StateWander>();
        Transition transition = new Transition();
        transition.condition = ca;
        transition.target = st;
        transitions.Add(transition);

        //transition to seek state
        State st2 = GetComponent<StateSeek>();
        Transition transition2 = new Transition();
        transition2.condition = ca2;
        transition2.target = st2;
        transitions.Add(transition2);

    }

    public override void Update()
    {
        base.Update();

        //Horribly inefficient
        potions = GameObject.FindGameObjectsWithTag("Potion");

        float count = Mathf.Infinity;

        potionAux = null;

        foreach (GameObject potion in potions)
        {
            if(Vector3.Distance(potion.transform.position, transform.position) < count)
            {
                count = Vector3.Distance(potion.transform.position, transform.position);
                potionAux = potion;
            }
        }

        agent.SetSteering(GetSteering());
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        if (potionAux == null)
            return steering;
        steering.linear = potionAux.transform.position - transform.position;
        steering.linear.Normalize();
        steering.linear *= agent.maxAccel;

        Face(steering);

        return steering;
    }
}
