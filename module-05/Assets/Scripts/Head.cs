using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : BodyPart {
    Vector2 movement;
    private BodyPart tail = null;
    private List<BodyPart> bodyParts = new List<BodyPart>();
    public AudioSource[] gulpSounds = new AudioSource[3];
    public AudioSource dieSound = null;

    const float TIME_TO_ADD = 0.1f;
    float timer = TIME_TO_ADD;
    public int partsToAdd = 2;

    void Start() {
        SwipeController.Swipe += OnSwipe;
    }

    override public void Update() {
        if(!GameController.instance.alive) {
            return;
        }

        base.Update();
        SetMovement(movement);
        UpdateDirection();
        UpdatePosition();

        if(partsToAdd > 0) {
            timer -= Time.deltaTime;
            if(timer <= 0) {
                AddBodyPart();
                partsToAdd--;
                timer = TIME_TO_ADD;
            }
        }
    }

    void AddBodyPart() {
        if(tail == null) {
            Vector3 newPosition = transform.position;
            newPosition.z += 0.01f;

            BodyPart newPart = Instantiate(GameController.instance.bodyPartPrefab, newPosition, Quaternion.identity);
            newPart.followingPart = this;
            tail = newPart;
            newPart.MakeTail();
            bodyParts.Add(newPart);
        } else {
            Vector3 newPosition = tail.transform.position;
            newPosition.z += 0.01f;

            BodyPart newPart = Instantiate(GameController.instance.bodyPartPrefab, newPosition, tail.transform.rotation);
            newPart.followingPart = tail;
            newPart.MakeTail();
            tail.MakeBodyPart();
            tail = newPart;
            bodyParts.Add(newPart);
        }
    }

    void OnSwipe(SwipeController.Direction direction) {
        switch(direction) {
            case SwipeController.Direction.Up:
                movement = GameController.instance.speed * Time.deltaTime * Vector2.up;
                break;
            case SwipeController.Direction.Down:
                movement = GameController.instance.speed * Time.deltaTime * Vector2.down;
                break;
            case SwipeController.Direction.Left:
                movement = GameController.instance.speed * Time.deltaTime * Vector2.left;
                break;
            case SwipeController.Direction.Right:
                movement = GameController.instance.speed * Time.deltaTime * Vector2.right;
                break;
        }
    }

    public void ResetSnake() {
        tail = null;
        foreach(BodyPart part in bodyParts) {
            Destroy(part.gameObject);
        }
        bodyParts.Clear();

        gameObject.transform.localEulerAngles = Vector3.zero;
        gameObject.transform.position = new Vector3(0, 0, -8);
        ResetMemory();
        OnSwipe(SwipeController.Direction.Up);
        partsToAdd = 2;
        timer = TIME_TO_ADD;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Egg egg = collision.GetComponent<Egg>();
        if(egg != null) {
            EatEgg(egg);
        } else {
            dieSound.Play();
            GameController.instance.GameOver();
        }
    }

    void EatEgg(Egg egg) {
        partsToAdd++;
        timer = 0;
        gulpSounds[Random.Range(0, gulpSounds.Length)].Play();
        GameController.instance.EggEaten(egg);
    }
}
