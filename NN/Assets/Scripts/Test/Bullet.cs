using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject tankFather;

    float speed = 200f;
    bool canShoot = true;
    float timer;

    public void Shoot(float force, float dt)
    {
        transform.Translate(transform.forward * force * speed * dt);

        timer += dt;

        if (timer > 6.0f)
        {
            tankFather.GetComponent<Tank>().Punish();
            ResetBullet();
        }
    }

    public void ResetBullet()
    {
        transform.position = tankFather.transform.position;
        timer = 0.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        tankFather.GetComponent<Tank>().Reward();
        ResetBullet();
    }
}
