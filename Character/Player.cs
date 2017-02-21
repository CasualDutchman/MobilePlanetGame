using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject machineFuel, machineAir;

    public Color OotColor;

    public Selection currentSelected = Selection.NONE;

    public Machine currentSelectedMachine;

    public Transform body, Leye, Reye;
    Vector3 bodyScale, LeyePos, ReyePos;

    void Start () {
        /*
        transform.FindChild("OotBody").GetComponent<SpriteRenderer>().color = OotColor;

        
        bodyScale = transform.FindChild("OotBody").localScale;
        LeyePos = transform.FindChild("LeftEye").localPosition;
        ReyePos = transform.FindChild("RightEye").localPosition;
        */
    }
	
	void FixedUpdate() {

    }

    public void ResetLook() {
        /*
        body.localScale = bodyScale;
        Leye.localPosition = LeyePos;
        Reye.localPosition = ReyePos;
        transform.localScale = new Vector3(1, 1, 1);
        */
    }

    public void Look(float f) {
        /*
        if (f > 0) {
            body.localScale = new Vector3(0.4f, 0.5f, 0.5f);
            Leye.localPosition = LeyePos + new Vector3(0.28f, 0, 0);
            Reye.localPosition = ReyePos + new Vector3(-0.1f, 0, 0.2f);
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            LookRigth();
            transform.localScale = new Vector3(-1, 1, 1);
        }
        */
    }

    public void LookRigth() {
        /*
        body.localScale = new Vector3(0.4f, 0.5f, 0.5f);
        Leye.localPosition = LeyePos + new Vector3(0.28f, 0, 0);
        Reye.localPosition = ReyePos + new Vector3(-0.1f, 0, 0.2f);
        transform.localScale = new Vector3(1, 1, 1);
        */
    }

    public bool canWalk() {
        return currentSelected == Selection.NONE;
    }

    public enum Selection {
        NONE, MENU, SPACESHIP, BUILDER, UPGRADER   
    }
}
