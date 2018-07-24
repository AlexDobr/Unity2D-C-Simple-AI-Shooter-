using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Face
{

    public float fireRate = 1.0f;
    public GameObject bulletPrefab;
    private float counter = 0.0f;

    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Update ()
    {
        counter += Time.deltaTime;

        base.Update();

        if (counter >= fireRate)
        {
            if(targetAux != null && facing == true)
            {
                Fire();
                counter = 0.0f;
            }
        }
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
