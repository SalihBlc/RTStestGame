using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private UnitController attacker;
    private UnitController target;
    public float speed = 10f;

    public void Seek(UnitController _attacker, UnitController _target)
    {
        attacker = _attacker;
        target = _target;

        //speed = attacker.GetComponent<UnitStats>().bulletSpeed;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.transform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        RTSGameManager.UnitTakeDamage(attacker, target);
        GameObject.Destroy(this.gameObject);
    }
}
