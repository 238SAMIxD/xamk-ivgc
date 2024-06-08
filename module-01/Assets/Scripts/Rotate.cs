using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
   public float speed = 10.0f;

    void Update() {
        this.gameObject.transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }
}
