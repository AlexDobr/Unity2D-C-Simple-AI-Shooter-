using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWander : StateEnemy
{
    public float maxDistance;
    public float targetPercent;

    public override void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
        base.Awake();


        //is player close
        ConditionTargetFar ctf = new ConditionTargetFar();
        ctf.origin = this.gameObject;
        ctf.target = targetAux;
        ctf.maxDistance = maxDistance;

        //is health higher than %
        ConditionAgentHealthHigher cahh = new ConditionAgentHealthHigher();
        cahh.target = agent;
        cahh.targetPercent = targetPercent;

        //is both above true
        ConditionAnd ca = new ConditionAnd();
        ca.conditionA = ctf;
        ca.conditionB = cahh;

        //transition to seek state
        State st = GetComponent<StateSeek>();
        Transition transition = new Transition();
        transition.condition = ca;
        transition.target = st;
        transitions.Add(transition);


        //if health lower than %
        ConditionAgentHealthLower cahl = new ConditionAgentHealthLower();
        cahl.target = agent;
        cahl.targetPercent = targetPercent;

        //transition to heal state
        State st2 = GetComponent<StateHeal>();
        Transition transition2 = new Transition();
        transition2.condition = cahl;
        transition2.target = st2;
        transitions.Add(transition2);
    }

    public override void Update()
    {
        base.Update();
        agent.SetSteering(GetSteering());
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();

        if (target == null)
            return steering;
        steering.linear = Vector3.zero;
        steering.linear.Normalize();
        steering.linear *= agent.maxAccel;

        Face(steering);

        return steering;
    }
}
