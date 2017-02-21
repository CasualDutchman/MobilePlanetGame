using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machine : MonoBehaviour {

    public int ID, planetID;

    public float angle;

    public int level;

    public Type currentType = Type.FUEL;

    protected GameManager manager;

    void Start() {
        transform.localEulerAngles = new Vector3(0, 0, -angle);
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        OnStart();
    }

    void FixedUpdate() {
        OnUpdate();
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }

    public bool isOccupied() {
        return currentType != Type.NONE;
    }

    public enum Type {
        NONE, FUEL, AIR
    }
}
