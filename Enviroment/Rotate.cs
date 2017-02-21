using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public float rotationSpeed = 1;

    void FixedUpdate () {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.fixedDeltaTime);
	}
}
