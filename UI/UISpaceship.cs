using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpaceship : MonoBehaviour {

    public Text money, polution;

    GameManager manager;

    void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

	void FixedUpdate () {
        money.text = "Money: " + manager.money.ToString("F0");
        polution.text = "Polution: " + manager.currentPlanet.PolutionPercent().ToString("F1") + "%";
	}
}
