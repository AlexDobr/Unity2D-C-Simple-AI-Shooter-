using System.Collections;
using UnityEngine;

public class Player : Agent
{
    public GameObject bulletPrefab;
    private Vector3 diff;
    public float fireRate = 1.0f;
    private float counter = 0.0f;

    // Update is called once per frame
    public override void Update ()
    {
        counter += Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal");
        velocity.y = Input.GetAxis("Vertical");
        velocity *= maxSpeed;
        Vector3 translation = velocity * Time.deltaTime;
        transform.Translate(translation, Space.World);
        //transform.up = Input.mousePosition - transform.position;

        //calculate direction between player and mouse
        diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        orientation = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg) + 180;
        transform.rotation = Quaternion.Euler(0f, 0f, orientation-270);

        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (counter >= fireRate)
            {
                Fire();
                counter = 0.0f;
            }
        }

        if (hp < 1)
        {
            Death();
        }

    }

    private void Fire()
    {
        GameObject curBullet = Instantiate<GameObject>(bulletPrefab, transform.position, Quaternion.identity, null);
        Bullet bullet = curBullet.GetComponent<Bullet>();
        curBullet.SetActive(false);
        bullet.parent = gameObject;
        bullet.target = null;
        bullet.GetSteering().linear = OriToVec(orientation - 180);
        curBullet.SetActive(true);
    }
}
