using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

public class Bug : MonoBehaviour
{

    /// <summary>
    /// The units movement speed
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// This stack contains the path that the Unit can walk on
    /// This path should be generated with the AStar algorithm
    /// </summary>
    private Stack<Node> path;
    private SpriteRenderer spriteRenderer;
    private Animator myAnimator;
    [SerializeField]
    private Stat health;
    public bool Alive
    {
        get
        {
            return health.CurrentValue > 0;
        }
    }

    /// <summary>
    /// The Unit's grid position
    /// </summary>
    public Point GridPosition { get; set; }

    /// <summary>
    /// Indicates if the Unit is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The unit's next destination
    /// </summary>
    private Vector3 destination;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        health.Initialize();
    }

    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Spawns the bug in our world
    /// </summary>
    public void Spawn(int health)
    {

        transform.position = LevelManager.Instance.BlackPortal.transform.position;
        this.health.MaxVal = health;
        this.health.CurrentValue = this.health.MaxVal;

        myAnimator = GetComponent<Animator>();
        //Starts to scale the bugs
        StartCoroutine(Scale(new Vector3(0.1f, 0.1f), new Vector3(1, 1), false));

        //Sets the bugs path
        SetPath(LevelManager.Instance.Path, false);
    }

    /// <summary>
    /// Scales a bug up or down
    /// </summary>
    /// <param name="from">start scale</param>
    /// <param name="to">end scale</param>
    /// <returns></returns>
    public IEnumerator Scale(Vector3 from, Vector3 to, bool remove)
    {
        //The scaling progress
        float progress = 0;

        //As long as the progress is les than 1, then we need to keep scaling
        while (progress <= 1)
        {
            //Scales thebug
            transform.localScale = Vector3.Lerp(from, to, progress);
            progress += Time.deltaTime;
            yield return null;
        }

        //Makes sure that is has the correct scale after scaling
        transform.localScale = to;

        IsActive = true;

        if (remove)
        {
            Release();
        }
    }

    /// <summary>
    /// Makes the unity move along the given path
    /// </summary>
    public void Move()
    {
        if (IsActive)
        {
            //Move the unit towards the next destination
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            //Checks if we arrived at the destination
            if (transform.position == destination)
            {
                //If we have a path and we have more nodes, then we need to keep moving
                if (path != null && path.Count > 0)
                {
                    Animate(GridPosition, path.Peek().GridPosition);
                    //Sets the new gridPosition
                    GridPosition = path.Peek().GridPosition;

                    //Sets a new destination
                    destination = path.Pop().WorldPosition;

                }
            }
        }

    }

    /// <summary>
    /// Gives the Unit a path to walk on
    /// </summary>
    /// <param name="newPath">The unit's new path</param>
    /// <param name="active">Indicates if the unit is active</param>
    public void SetPath(Stack<Node> newPath, bool active)
    {
        if (newPath != null) //If we have a path
        {
            //Sets the new path as the current path
            this.path = newPath;
            Animate(GridPosition, path.Peek().GridPosition);

            //Sets the new gridPosition
            GridPosition = path.Peek().GridPosition;

            //Sets a new destination
            destination = path.Pop().WorldPosition;
        }
    }
    private void Animate (Point currentPos, Point newPos)
    {
        if (currentPos.Y > newPos.Y)
        {
            //we are moving down
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", 1);
        }
        else if(currentPos.Y <newPos.Y)
        {
            // then we are moving up
            myAnimator.SetInteger("Horizontal", 0);
            myAnimator.SetInteger("Vertical", -1);
        }
        if (currentPos.Y == newPos.Y)
        {
            if (currentPos.X > newPos.X)
            {
                // move to left
                myAnimator.SetInteger("Horizontal", -1);
                myAnimator.SetInteger("Vertical", 0);
            }
            else if (currentPos.X < newPos.X)
            {
                // move to right
                myAnimator.SetInteger("Horizontal", 1);
                myAnimator.SetInteger("Vertical", 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RedPortal")
        {
            StartCoroutine(Scale(new Vector3(1, 1), new Vector3(0.1f, 0.1f), true));
            GameManager.Instance.Lives--;
        }
        if (other.tag == "Tile")
        {
            spriteRenderer.sortingOrder = other.GetComponent<TileScript>().GridPosition.Y;
        }
    }
    public void Release()
    {
        IsActive = false;
        GridPosition = LevelManager.Instance.BlackSpawn;
        GameManager.Instance.Pool.ReleaseObject(gameObject);
        GameManager.Instance.RemoveBug(this);
    }
    public void TakeDamage(int damage)
    {
        if (IsActive)
        {
            health.CurrentValue -= damage;
            if (health.CurrentValue <=0)
            {
                GameManager.Instance.Currency += 2;
                myAnimator.SetTrigger("Die");
                IsActive = false;
                GetComponent<SpriteRenderer>().sortingOrder--;
            }
        }
    }
}
