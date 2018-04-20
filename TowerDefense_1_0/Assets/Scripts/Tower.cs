using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    private string projectileType;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private int damage;
    public float ProjectileSpeed
    {
        get
        {
            return projectileSpeed;
        }
    }
    private SpriteRenderer mySpriteRenderer;
    private Bug target;
    public Bug Target
    {
        get
        {
            return target;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
    }

    private Queue<Bug> bugs = new Queue<Bug>();
    private bool canAttack = true;
    private float attackTimer;
    [SerializeField]
    private float attackCooldown;
	// Use this for initialization
	void Start ()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Attack();
      //  Debug.Log(target);
	}
    public void Select()
    {
        mySpriteRenderer.enabled = !mySpriteRenderer.enabled;
    }
    private void Attack()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                canAttack = true;
                attackTimer = 0;
            }
        }
        if (target == null && bugs.Count > 0)
        {
            target = bugs.Dequeue();
        }
        if (target != null && target.IsActive)
        {
            if (canAttack)
            {
                Shoot();

                canAttack = false;
            }
        }
        else if (bugs.Count > 0)
        {
            target = bugs.Dequeue();
        }
        if (target != null && !target.Alive || target != null && !target.IsActive)
        {
            target = null;
        }
    }
    private void Shoot()
    {
        Projectile projectile = GameManager.Instance.Pool.GetObject(projectileType).GetComponent<Projectile>();
        projectile.transform.position = transform.position;
        projectile.Initialize(this);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bug")
        {
            bugs.Enqueue(other.GetComponent<Bug>());
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Bug")
        {
            target = null;
        }
    }
}
