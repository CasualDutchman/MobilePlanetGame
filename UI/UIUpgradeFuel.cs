using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeFuel : MonoBehaviour {

    public Text upgrade1lvl, upgrade2lvl, upgrade3lvl;
    public Text upgrade1, upgrade2, upgrade3;

    GameManager manager;
    Player player;

    void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ToBoUpdated();
    }

    void FixedUpdate () {
        ToBoUpdated();
    }

    void ToBoUpdated() {
        upgrade1lvl.text = "Level: " + ((MachineFuel)player.currentSelectedMachine).pumpLevel;
        upgrade2lvl.text = "Level: " + ((MachineFuel)player.currentSelectedMachine).refineryLevel;
        upgrade3lvl.text = "Level: " + ((MachineFuel)player.currentSelectedMachine).airFilterLevel;

        upgrade1.text = "$" + manager.fuelUpgradePumpCost * ((MachineFuel)player.currentSelectedMachine).pumpLevel;
        upgrade2.text = "$" + manager.fuelUpgradeRefineryCost * ((MachineFuel)player.currentSelectedMachine).refineryLevel;
        upgrade3.text = "$" + manager.fuelUpgradeFilterCost * ((MachineFuel)player.currentSelectedMachine).airFilterLevel;
    }
}
