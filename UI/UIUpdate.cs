using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdate : MonoBehaviour {

    public Text moneyText;

    GameManager manager;

    void Start () {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
	}
	
	void Update () {
        moneyText.text = "Money: " + manager.money.ToString("F0");
	}
}
