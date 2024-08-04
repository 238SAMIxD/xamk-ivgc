using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour {
    public enum Direction {
        Up, Down, Left, Right
    }
    Vector2 start;
    Vector2 end;
    float minDistance = 10;

    public static event Action<Direction> Swipe = delegate { };

    void Update() {
        foreach(Touch touch in Input.touches) {
            if(touch.phase == TouchPhase.Began) {
                start = touch.position;
            } else if(touch.phase == TouchPhase.Ended) {
                end = touch.position;
                OnSwipe();
            }
        }
        if(Input.GetMouseButtonDown(0)) {
            start = Input.mousePosition;
        } else if(Input.GetMouseButtonUp(0)) {
            end = Input.mousePosition;
            OnSwipe();
        }
    }

    private void OnSwipe() {
        float distance = Vector2.Distance(start, end);
        if(distance < minDistance) {
            return;
        }
        if(isVertical()) {
            if(start.y < end.y) {
                Swipe(Direction.Up);
                return;
            }
            Swipe(Direction.Down);
            return;
        }
        if(start.x < end.x) {
            Swipe(Direction.Right);
            return;
        }
        Swipe(Direction.Left);
    }

    bool isVertical() {
        float vertical = Mathf.Abs(start.y - end.y);
        float horizontal = Mathf.Abs(start.x - end.x);
        return vertical > horizontal;
    }
}
