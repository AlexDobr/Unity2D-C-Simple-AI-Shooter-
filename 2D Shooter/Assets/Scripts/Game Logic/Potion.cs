using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public int health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Agent agent;
        if(collision.gameObject.GetComponent<Agent>())
        {
            if (collision.gameObject.tag == "Bullet")
            {
                return;
            }

            agent = collision.gameObject.GetComponent<Agent>();
            agent.hp += health;

            Destroy(gameObject);
        }
    }
}
