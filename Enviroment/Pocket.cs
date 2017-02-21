using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pocket : MonoBehaviour {

    public int ID, planetID;
    public float amount;
    public float maxAmount;
    public float angle;

    public Image drainer;

	void Start () {
        transform.localEulerAngles = new Vector3(0, 0, -angle);

        if(PlayerPrefs.HasKey("Pocket_" + planetID)) {
            amount = PlayerPrefs.GetFloat("Pocket_" + planetID);
        } else {
            amount = maxAmount;
        }
	}

    void FixedUpdate() {
        PlayerPrefs.SetFloat("Pocket_" + planetID, amount);
        drainer.fillAmount = (amount / maxAmount);
    }
}
