using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeAway : MonoBehaviour {

    public Text timeAwayText, earningText;
    TimeSpan difference;

	void Start () {
        difference = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().difference;
        timeAwayText.text = difference.Days + "d:" + difference.Hours + "h:" + difference.Minutes + "m:" + difference.Seconds + "s";
        earningText.text = "Sec * 2 = " + ((int)difference.TotalSeconds * 2);
    }
	
	void Update () {
		
	}
}
