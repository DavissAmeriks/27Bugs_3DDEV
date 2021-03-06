﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Bug target;
    private Tower parent;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveToTarget();
	}
    public void Initialize(Tower parent)
    {
        this.target = parent.Target;
        this.parent = parent;
    }
    private void MoveToTarget()
    {
        if (target != null && target.IsActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * parent.ProjectileSpeed);
            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        else if (!target.IsActive)
        {
            GameManager.Instance.Pool.ReleaseObject(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bug")
        {
            if (target.gameObject == other.gameObject)
            {
                target.TakeDamage(parent.Damage);
               // Bug hitInfo = other.GetComponent<Bug>();
                GameManager.Instance.Pool.ReleaseObject(gameObject);
            }
            
        }
    }
}
