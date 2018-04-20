using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// a property for the towerBtn
    /// </summary>
    public TowerBtn ClickedBtn { get; set; }

    /// <summary>
    /// A reference to the currency text
    /// </summary>
    private int currency;
    private int wave = 0;
    private int lives;
    private bool gameOver = false;
    private int health = 15;
    [SerializeField]
    private Text livesTxt;
    [SerializeField]
    private Text waveTxt;

    [SerializeField]
    private Text currencyTxt;
    [SerializeField]
    private GameObject waveBtn;
    [SerializeField]
    private GameObject gameOverMenu;
    private Tower selectedTower;

    private List<Bug> activeBugs = new List<Bug>();
     
    /// <summary>
    /// A property for the object pool
    /// </summary>
    public ObjectPool Pool { get; set; }
    public bool WaveActive
    {
        get
        {
            return activeBugs.Count > 0;
        }
    }

    /// <summary>
    /// Property for accessing the currency
    /// </summary>
    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value;
            this.currencyTxt.text = value.ToString() + "EUR";
        }
    }

    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            this.lives = value;
            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }
            livesTxt.text = lives.ToString();
        }
    }
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    // Use this for initialization
    void Start()
    {
        Lives = 10;
        Currency = 10;
    }

    // Update is called once per frame
    void Update()
    {

        HandleEscape();
    }

    /// <summary>
    /// Pick a tower then a buy button is pressed
    /// </summary>
    /// <param name="towerBtn">The clicked button</param>
    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price && !WaveActive)
        {
            //Stores the clicked button
            this.ClickedBtn = towerBtn;

            //Activates the hover icon
            Hover.Instance.Activate(towerBtn.Sprite);
        }


    }

    /// <summary>
    /// Buys a tower
    /// </summary>
    public void BuyTower()
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;

            Hover.Instance.Deactivate();
        }

    }
    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = tower;
        selectedTower.Select();

    }
    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
    }
    /// <summary>
    /// Handles escape presses
    /// </summary>
    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//if we press escape
        {
            //Deactivate the hover instance
            Hover.Instance.Deactivate();
        }
    }

    public void StartWave()
    {
        wave++;
        waveTxt.text = string.Format("Level: {0}", wave);
        StartCoroutine(SpawnWave());
        waveBtn.SetActive(false);
    }

    /// <summary>
    /// Spawns a wave of bugs
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();
        for (int i = 0; i < wave; i++)
        {
            int buugIndex = 1; //Random.Range(0, 2);

            string type = string.Empty;

            switch (buugIndex)
            {
                case 0:
                    type = "BlueBug";
                    break;
                case 1:
                    type = "RedBug";
                    break;
            }

            //Requests the bug form the pool
            Bug bug = Pool.GetObject(type).GetComponent<Bug>();

            bug.Spawn(health);
            if (wave % 3 ==0)
            {
                health += 5;
            }
            activeBugs.Add(bug);

            yield return new WaitForSeconds(2.5f);
        }

    }
    public void RemoveBug(Bug bug)
    {
        activeBugs.Remove(bug);

        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true); 
        }
    }
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
