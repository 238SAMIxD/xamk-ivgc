using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour {
    Vector2 dPosition;

    public BodyPart followingPart = null;
    private SpriteRenderer spriteRenderer = null;

    const int PARTS_REMEMBERED = 10;
    public Vector3[] previousPositions = new Vector3[PARTS_REMEMBERED];
    public int setIndex = 0;
    public int getIndex = -(PARTS_REMEMBERED - 1);

    void Awake() {
        Application.targetFrameRate = 60;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    virtual public void Update() {
        if(!GameController.instance.alive) {
            return;
        }

        Vector3 followPosition;
        if(followingPart != null) {
            if(followingPart.getIndex > -1) {
                followPosition = followingPart.previousPositions[followingPart.getIndex];
            } else {
                followPosition = followingPart.transform.position;
            }
        } else {
            followPosition = gameObject.transform.position;
        }
        previousPositions[setIndex] = gameObject.transform.position;
        setIndex++;
        if(setIndex >= PARTS_REMEMBERED) {
            setIndex = 0;
        }
        getIndex++;
        if(getIndex >= PARTS_REMEMBERED) {
            getIndex = 0;
        }

        if(followingPart != null) {
            Vector3 newPosition;
            if(followingPart.getIndex > -1) {
                newPosition = followPosition;
            } else {
                newPosition = followingPart.transform.position;
            }

            newPosition.z += 0.01f;

            SetMovement(newPosition - gameObject.transform.position);
            UpdateDirection();
            UpdatePosition();
        }
    }

    public void ResetMemory() {
        setIndex = 0;
        getIndex = -(PARTS_REMEMBERED - 1);
    }

    public void SetMovement(Vector2 movement) {
        dPosition = movement;
    }

    public void UpdatePosition() {
        gameObject.transform.position += new Vector3(dPosition.x, dPosition.y, 0);
    }

    public void UpdateDirection() {
        if(dPosition.y > 0) {
            gameObject.transform.localEulerAngles = Vector3.zero;
            return;
        }
        if(dPosition.y < 0) {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 180);
            return;
        }
        if(dPosition.x < 0) {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
            return;
        }
        gameObject.transform.localEulerAngles = new Vector3(0, 0, 270);
    }

    public void MakeTail() {
        spriteRenderer.sprite = GameController.instance.tailSprite;
    }
    public void MakeBodyPart() {
        spriteRenderer.sprite = GameController.instance.bodySprite;
    }
}
