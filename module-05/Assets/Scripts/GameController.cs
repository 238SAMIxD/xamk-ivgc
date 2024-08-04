using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour {
    public static GameController instance = null;

    const float width = 4.25f;
    const float height = 9.25f;

    public float speed = 1;
    public bool alive = true;
    public bool waiting = true;
    int level = 0;
    int levelUpCount = 0;

    int score = 0;
    int highScore = 0;

    public Head snakeHead = null;
    public BodyPart bodyPartPrefab = null;
    public Sprite bodySprite = null;
    public Sprite tailSprite = null;
    public GameObject rockPrefab = null;
    public GameObject eggPrefab = null;
    public GameObject goldenEggPrefab = null;
    public GameObject spikePrefab = null;
    public TextMeshProUGUI scoreText = null;
    public TextMeshProUGUI highScoreText = null;
    public TextMeshProUGUI startText = null;
    public TextMeshProUGUI tapText = null;

    private Egg egg = null;
    private List<Spike> spikes = new List<Spike>();

    void Start() {
        instance = this;
        CreateWalls();
        alive = false;
    }

    void Update() {
        if(waiting) {
            foreach(Touch touch in Input.touches) {
                if(touch.phase == TouchPhase.Ended) {
                    StartGame();
                }
            }
            if(Input.GetMouseButtonDown(0)) {
                StartGame();
            }
        }
    }

    void StartGame() {
        score = 0;
        level = 0;
        snakeHead.ResetSnake();
        if(egg != null) {
            Destroy(egg.gameObject);
        }
        egg = null;
        foreach(Spike spike in spikes) {
            Debug.Log(spike);
            Destroy(spike.gameObject);
        }
        spikes.Clear();
        LevelUp();
        scoreText.text = $"Score: {score}";
        highScoreText.text = $"High Score: {highScore}";
        startText.gameObject.SetActive(false);
        tapText.gameObject.SetActive(false);

        alive = true;
        waiting = false;
    }

    void LevelUp() {
        level++;
        levelUpCount = level * 2 + 4;
        speed = Math.Min(1f + level / 4, 6);
        snakeHead.ResetSnake();
        foreach(Spike spike in spikes) {
            Debug.Log(spike);
            Destroy(spike.gameObject);
        }
        spikes.Clear();
        CreateEgg();
        for(int i = 0; i < level; i++) {
            CreateSpike();
        }
    }

    void CreateWalls() {
        Vector3 start = new Vector3(-width, -height, -0.01f);
        Vector3 end = new Vector3(-width, height, -0.01f);
        CreateWall(start, end);

        start = new Vector3(width, -height, -0.01f);
        end = new Vector3(width, height, -0.01f);
        CreateWall(start, end);

        start = new Vector3(-width, -height, -0.01f);
        end = new Vector3(width, -height, -0.01f);
        CreateWall(start, end);

        start = new Vector3(-width, height, -0.01f);
        end = new Vector3(+width, height, -0.01f);
        CreateWall(start, end);
    }

    void CreateWall(Vector3 start, Vector3 end) {
        float distance = Vector3.Distance(start, end);
        int rocksNumber = (int)(distance * 3.0f);
        Vector3 delta = (end - start) / rocksNumber;

        for(int i = 0; i < rocksNumber; i++) {
            Vector3 position = start + delta * i;
            float scale = Random.Range(1f, 2f);
            float rotation = Random.Range(0f, 360f);
            CreateRock(position, scale, rotation);
        }
    }

    void CreateRock(Vector3 position, float scale, float rotation) {
        GameObject rock = Instantiate(rockPrefab, position, Quaternion.Euler(0, 0, rotation));
        rock.transform.localScale = new Vector3(scale, scale, 1);
    }

    void CreateEgg(bool golden = false) {
        Vector3 position = new Vector3(Random.Range(-width + 1, width - 1), Random.Range(-height + 1, height - 1), -0.01f);
        egg = Instantiate(golden ? goldenEggPrefab : eggPrefab, position, Quaternion.identity).GetComponent<Egg>();
    }

    void CreateSpike() {
        float x = Random.Range(-width + 1, width - 1);
        float y = Random.Range(-height + 1, height - 1);
        x = Math.Abs(x) < 1 ? Math.Sign(x) : x;
        y = Math.Abs(y) < 1 ? Math.Sign(y)*2 : y;
        Vector3 position = new Vector3(x, y, -0.01f);
        spikes.Add(Instantiate(spikePrefab, position, Quaternion.identity).GetComponent<Spike>());
    }

    public void GameOver() {
        alive = false;
        waiting = true;
        startText.text = "Game Over";
        startText.gameObject.SetActive(true);
        tapText.gameObject.SetActive(true);
    }

    public void EggEaten(Egg egg) {
        levelUpCount--;
        score++;
        if(levelUpCount == 0) {
            score += 10;
            LevelUp();
        } else {
            CreateEgg(levelUpCount == 1);
        }
        highScore = Math.Max(score, highScore);
        scoreText.text = $"Score: {score}";
        highScoreText.text = $"High Score: {highScore}";
        Destroy(egg.gameObject);
    }
}
