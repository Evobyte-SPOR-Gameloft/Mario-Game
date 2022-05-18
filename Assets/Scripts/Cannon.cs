using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    // Start is called before the first frame update

    public float timeBeforeLaunchBullet = 3.0f;
    public float cannonTimer = 0;

    public GameObject Bullet;

    public Transform firePoint;
    void Start()
    {
        enabled = true;
        fire();
    }
    void OnBecameVisible()
    {
        enabled = false;
    }
    // Update is called once per frame
    void CheckCannonTimer()
    {
        if(cannonTimer<=timeBeforeLaunchBullet)
        {
            cannonTimer+=Time.deltaTime;
        }
        else
        {
            cannonTimer = 0;
            fire();
        }
    }
    void Update()
    {
        CheckCannonTimer();
    }

    void fire()
    {
        Instantiate(Bullet, firePoint.position, firePoint.rotation);
    }
}
