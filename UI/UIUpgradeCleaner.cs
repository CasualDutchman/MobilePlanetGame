using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpgradeCleaner : MonoBehaviour {

    public Text upgrade1lvl, upgrade2lvl;
    public Text upgrade1, upgrade2;

    GameManager manager;
    Player player;

    void Start() {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        ToBoUpdated();
    }

    void FixedUpdate() {
        ToBoUpdated();
    }

    void ToBoUpdated() {
        upgrade1lvl.text = "Level: " + ((MachineCleaner)player.currentSelectedMachine).airFilterLevel;
        upgrade2lvl.text = "Level: " + ((MachineCleaner)player.currentSelectedMachine).efficiencyLevel;

        upgrade1.text = "$" + manager.fuelUpgradePumpCost * ((MachineCleaner)player.currentSelectedMachine).airFilterLevel;
        upgrade2.text = "$" + manager.fuelUpgradeRefineryCost * ((MachineCleaner)player.currentSelectedMachine).efficiencyLevel;
    }
}
