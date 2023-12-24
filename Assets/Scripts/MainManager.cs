using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text HighScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string playerName;
    private HighScore current_highScore;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        playerName = GameManager.instance.PlayerName;

        SetHighScoreText();
        AddPoint(0);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{playerName} Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        if (m_Points > LoadHighScore().score)
        {
            SaveHighScore();
            SetHighScoreText();
        }
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    public class HighScore
    {
        public string name;
        public int score;
    }

    public void SaveHighScore()
    {
        HighScore highScore = new HighScore();
        highScore.name = playerName;
        highScore.score = m_Points;
        string json = JsonUtility.ToJson(highScore);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
    }

    public HighScore LoadHighScore()
    {
        if (!System.IO.File.Exists(Application.persistentDataPath + "/highscore.json"))
        {
            return new HighScore();
        }
        string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/highscore.json");
        HighScore highScore = JsonUtility.FromJson<HighScore>(json);
        return highScore;
    }

    public void SetHighScoreText()
    {
        current_highScore = LoadHighScore();
        HighScoreText.text = $"High Score : {current_highScore.name} : {current_highScore.score}";
    }
}
