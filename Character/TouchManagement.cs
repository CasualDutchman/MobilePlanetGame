using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchManagement : MonoBehaviour {

    [Header("- TouchArea")]
    public RectTransform clickToMove;

    [Header("- Builder Components")]
    public GameObject buildButtons;
    public GameObject moneyFuelText, moneyCleanerText;
    public Button buildbutton;

    [Header("- Upgrader Fuel Components")]
    public GameObject upgradeFuelButtons;

    [Header("- Upgrader Cleaner Components")]
    public GameObject upgradeCleanerButtons;

    [Header("- Spaceship Components")]
    public GameObject spaceshipInfo;

    [Header("- Menu Fuel Components")]
    public GameObject menuComponents;
    public GameObject menuButton;

    [Header("- TimeAway Components")]
    public GameObject timeAwayScreen;

    bool clicked;

    float percentage;

    Player player;
    GameManager manager;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();

        if(manager.difference.TotalSeconds >= 2) {
            player.currentSelected = Player.Selection.MENU;
            timeAwayScreen.SetActive(true);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (player.currentSelected != Player.Selection.NONE) {
                CloseMenus();
            } else {
                manager.Save();
                Application.Quit();
            }
        }

        if (manager.currentPlanet.isEmptySpot(transform.position) && !AtSpaceship(Mathf.Abs(manager.currentPlanet.transform.eulerAngles.z))) {
            buildbutton.interactable = true;
        } else {
            buildbutton.interactable = false;
        }

        if (Input.touchCount > 0) {

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector3.forward);

            if (hit.collider != null && !clicked) {
                if (player.currentSelected == Player.Selection.NONE) {
                    if (hit.collider.tag == "Ship") {
                        player.currentSelected = Player.Selection.SPACESHIP;
                        spaceshipInfo.SetActive(true);
                        buildbutton.gameObject.SetActive(false);
                        menuButton.SetActive(false);
                    } else if (hit.collider.tag == "Fuel" || hit.collider.tag == "Cleaner") {
                        player.currentSelected = Player.Selection.UPGRADER;
                        player.currentSelectedMachine = hit.transform.parent.GetComponent<Machine>();
                        buildbutton.gameObject.SetActive(false);
                        menuButton.SetActive(false);
                        if (hit.collider.tag == "Fuel") {
                            upgradeFuelButtons.SetActive(true);
                        } else {
                            upgradeCleanerButtons.SetActive(true);
                        }
                    }
                }
                clicked = true;
            }
        } else {
            player.ResetLook();
            clicked = false;
        }
    }

    public void SendMoneyToPlayer() {
        manager.money += ((int)manager.difference.TotalSeconds * 2);
    }

    public void ShowBuildScreen() {
        player.currentSelected = Player.Selection.BUILDER;
        moneyFuelText.GetComponent<Text>().text = "$ " + manager.fuelBuildingCost.ToString();
        moneyCleanerText.GetComponent<Text>().text = "$ " + manager.cleanerBuildingCost.ToString();
        buildButtons.SetActive(true);
    }

    public void ShowSettingsMenu() {
        player.currentSelected = Player.Selection.MENU;
        menuComponents.SetActive(true);
        buildbutton.gameObject.SetActive(false);
        menuButton.SetActive(false);
    }

    public void AddFuelMachine() {
        if(manager.money >= manager.fuelBuildingCost) {
            RaycastHit2D hit = Physics2D.Raycast(player.transform.position, -Vector2.up, 7, 1 << LayerMask.NameToLayer("Pocket"));
            if (hit.collider == null) {
                return;
            }

            manager.money -= manager.fuelBuildingCost;
            GameObject go = Instantiate(player.machineFuel.gameObject);
            go.transform.parent = manager.currentPlanet.transform.GetChild(0);
            go.GetComponent<Machine>().angle = manager.currentPlanet.transform.eulerAngles.z;
            go.GetComponent<Machine>().planetID = manager.currentPlanet.machines.Count;

            manager.currentPlanet.machines.Add(go.GetComponent<Machine>());
            manager.AddMachines(go.GetComponent<Machine>());
            CloseMenus();
        }
    }

    public void AddCleanerMachine() {
        if (manager.money >= manager.cleanerBuildingCost) {
            manager.money -= manager.cleanerBuildingCost;
            GameObject go = Instantiate(player.machineAir.gameObject);
            go.transform.parent = manager.currentPlanet.transform.GetChild(0);
            go.GetComponent<Machine>().angle = manager.currentPlanet.transform.eulerAngles.z;
            go.GetComponent<Machine>().planetID = manager.currentPlanet.machines.Count;

            PlayerPrefs.SetString("Machine_" + go.GetComponent<Machine>().planetID, HelperConverter.StringFromInt(new float[] { go.GetComponent<Machine>().planetID, go.GetComponent<Machine>().angle, go.GetComponent<Machine>().level }));

            manager.currentPlanet.machines.Add(go.GetComponent<Machine>());
            manager.AddMachines(go.GetComponent<Machine>());
            CloseMenus();
        }
    }

    //public void UpgradeMachine() {
    //      if (player.currentSelectedMachine != null && manager.money >= manager.GetUpgradeCost(player.currentSelectedMachine.currentType, player.currentSelectedMachine.level + 1)) {
    //       manager.money -= manager.GetUpgradeCost(player.currentSelectedMachine.currentType, player.currentSelectedMachine.level + 1);
    //      player.currentSelectedMachine.level += 1;
    //
    //          CloseMenus();
    //    }
    //}

    public void FuelPumpUpgrade() {
        if(manager.money >= manager.fuelUpgradePumpCost * ((MachineFuel)player.currentSelectedMachine).pumpLevel) {
            manager.money -= manager.fuelUpgradePumpCost * ((MachineFuel)player.currentSelectedMachine).pumpLevel;
            ((MachineFuel)player.currentSelectedMachine).pumpLevel++;
            ((MachineFuel)player.currentSelectedMachine).MakeSave();
        }
    }

    public void FuelRefineryUpgrade() {
        if (manager.money >= manager.fuelUpgradePumpCost * ((MachineFuel)player.currentSelectedMachine).refineryLevel) {
            manager.money -= manager.fuelUpgradePumpCost * ((MachineFuel)player.currentSelectedMachine).refineryLevel;
            ((MachineFuel)player.currentSelectedMachine).refineryLevel++;
            ((MachineFuel)player.currentSelectedMachine).MakeSave();
        }
    }

    public void FuelFilterUpgrade() {
        if (manager.money >= manager.fuelUpgradePumpCost * ((MachineFuel)player.currentSelectedMachine).airFilterLevel) {
            manager.money -= manager.fuelUpgradePumpCost * ((MachineFuel)player.currentSelectedMachine).airFilterLevel;
            ((MachineFuel)player.currentSelectedMachine).airFilterLevel++;
            ((MachineFuel)player.currentSelectedMachine).MakeSave();
        }
    }

    public void CleanerFilterUpgrade() {
        if (manager.money >= manager.cleanerUpgradeFilterCost * ((MachineCleaner)player.currentSelectedMachine).airFilterLevel) {
            manager.money -= manager.cleanerUpgradeFilterCost * ((MachineCleaner)player.currentSelectedMachine).airFilterLevel;
            ((MachineCleaner)player.currentSelectedMachine).airFilterLevel++;
            ((MachineCleaner)player.currentSelectedMachine).MakeSave();
        }
    }

    public void CleanerEffeciencyUpgrade() {
        if (manager.money >= manager.cleanerUpgradeEffeciencyCost * ((MachineCleaner)player.currentSelectedMachine).efficiencyLevel) {
            manager.money -= manager.cleanerUpgradeEffeciencyCost * ((MachineCleaner)player.currentSelectedMachine).efficiencyLevel;
            ((MachineCleaner)player.currentSelectedMachine).efficiencyLevel++;
            ((MachineCleaner)player.currentSelectedMachine).MakeSave();
        }
    }

    public void RemoveMachine() {
        if (player.currentSelectedMachine != null) {
            
            manager.currentPlanet.machines.Remove(player.currentSelectedMachine);

            for(int i = 0; i < manager.currentPlanet.machines.Count; i++) {
                manager.currentPlanet.machines[i].planetID = i;
            }

            manager.RemoveMachine(player.currentSelectedMachine);

            Destroy(player.currentSelectedMachine.gameObject);
            CloseMenus();
        }
    }

    public void CloseMenus() {
        player.currentSelected = Player.Selection.NONE;
        buildButtons.SetActive(false);
        upgradeFuelButtons.SetActive(false);
        upgradeCleanerButtons.SetActive(false);
        spaceshipInfo.SetActive(false);
        buildbutton.gameObject.SetActive(true);
        menuComponents.SetActive(false);
        menuButton.SetActive(true);
        timeAwayScreen.SetActive(false);
    }

    bool TouchingRectange(RectTransform rectTransform) {
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.GetTouch(0).position);
    }

    public void resetPlanet() {
        PlayerPrefs.DeleteAll();
        int j = manager.allMachines.Count;
        for (int i = 0; i < j; i++) {
            Destroy(manager.currentPlanet.transform.GetChild(0).GetChild(i).gameObject);
            PlayerPrefs.DeleteKey("Machine_" + 1);
        }
        j = manager.currentPlanet.planetPockets.Count;
        for (int i = 0; i < j; i++) {
            Destroy(manager.currentPlanet.transform.GetChild(1).GetChild(i).gameObject);
            PlayerPrefs.DeleteKey("Pocket_" + i);
        }

        manager.currentPlanet.GetComponent<Planet>().machines = new List<Machine>();
        manager.allMachines = new List<Machine>();

        manager.currentPlanet.GetComponent<Planet>().planetPockets = new List<Pocket>();

        manager.currentPlanet.PlanetPrefs();
        manager.money = 500;
        PlayerPrefs.Save();
    }

    bool AtSpaceship(float rot) {
        int dis = 20;
        return (rot > (360 - dis) && rot <= 360) || (rot >= 0 && rot < dis);
    }
}
