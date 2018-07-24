using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public List<Transition> transitions;

	// Use this for initialization
	public virtual void Awake ()
    {
        transitions = new List<Transition>();
        //set up any transitions here
	}

    public virtual void OnEnable()
    {
        //state initialisation
    }

    public virtual void OnDisable()
    {
        //state finalisation;
    }
	
	// Update is called once per frame
	public virtual void Update ()
    {
		//behaviour
	}

    public void LateUpdate()
    {
        foreach(Transition t in transitions)
        {
            if(t.condition.Test())
            {
                t.target.enabled = true;
                this.enabled = false;
                return;
            }
        }
    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }

}
