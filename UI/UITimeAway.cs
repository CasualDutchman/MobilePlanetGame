using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimeAway : MonoBehaviour {

    public Text timeAwayText, earningText;
    TimeSpan difference;

	void Start () {
        GameManager manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        difference = manager.difference;
        timeAwayText.text = difference.Days + "d:" + difference.Hours + "h:" + difference.Minutes + "m:" + difference.Seconds + "s";
        earningText.text = "Sec * " + manager.multiplier + " = " + ((int)difference.TotalSeconds * manager.multiplier);
    }
	
	void Update () {
		
	}
}
