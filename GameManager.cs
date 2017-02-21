using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("- Player Info")]
    public float money;
    public float moneyIncrement = 10;

    [Header("- Planet Info")]
    public List<Machine> allMachines = new List<Machine>();
    public List<Pocket> allPockets = new List<Pocket>();
    public float polutionIncrement;

    [Header("- Cost")]
    public int fuelBuildingCost;
    public int fuelUpgradePumpCost, fuelUpgradeFilterCost, fuelUpgradeRefineryCost;
    public int cleanerBuildingCost;
    public int cleanerUpgradeFilterCost, cleanerUpgradeEffeciencyCost;

    public Planet currentPlanet;
    Player player;

    float timer;

    DateTime currentDate;
    public TimeSpan difference;

    void FixedUpdate() {
        //money += moneyPerSecond * Time.fixedDeltaTime;
        PlayerPrefs.SetFloat("Money", money);

        timer += 1 * Time.fixedDeltaTime;
        if (timer >= 1) {
            Save();
            timer = 0;
        }
    }

    public void AddMachines(Machine machine) {
        allMachines.Add(machine);
        PlayerPrefs.SetInt("MachineCount", allMachines.Count);
    }

    public void RemoveMachine(Machine machine) {
        int oldCount = allMachines.Count;
        allMachines.Remove(machine);

        for (int i = 0; i < oldCount - 1; i++) {
            if(allMachines[i].currentType == Machine.Type.FUEL) {
                PlayerPrefs.SetString("Machine_" + i, HelperConverter.StringFromInt(new float[] { allMachines[i].ID, i, allMachines[i].angle, ((MachineFuel)allMachines[i]).maxPipeLength, ((MachineFuel)allMachines[i]).currentPocket.planetID, ((MachineFuel)allMachines[i]).airFilterLevel, ((MachineFuel)allMachines[i]).pumpLevel, ((MachineFuel)allMachines[i]).refineryLevel }));
            } else {
                PlayerPrefs.SetString("Machine_" + i, HelperConverter.StringFromInt(new float[] { allMachines[i].ID, i, allMachines[i].angle, ((MachineCleaner)allMachines[i]).airFilterLevel, ((MachineCleaner)allMachines[i]).efficiencyLevel }));
            }
        }

        PlayerPrefs.DeleteKey("Machine_" + (oldCount - 1));
        PlayerPrefs.SetInt("MachineCount", allMachines.Count);
    }

    void Awake () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        DontDestroyOnLoad(gameObject);
        FindPlanet();
	}

    void Start() {
        Load();
    }

    public void Save() {
        PlayerPrefs.SetString("SystemTime", System.DateTime.Now.ToBinary().ToString());

        PlayerPrefs.Save();
    }

    public void Load() {
        if (PlayerPrefs.HasKey("Money")) {
            money = PlayerPrefs.GetFloat("Money");
        }
        if (PlayerPrefs.HasKey("MachineCount")) {
            for (int i = 0; i < PlayerPrefs.GetInt("MachineCount"); i++) {
                int type = HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[0];
                GameObject go = Instantiate(type == 0 ? player.machineFuel.gameObject : player.machineAir.gameObject);
                go.transform.parent = currentPlanet.transform.GetChild(0);
                go.GetComponent<Machine>().planetID = HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[1];
                go.GetComponent<Machine>().angle = HelperConverter.FloatFromString(PlayerPrefs.GetString("Machine_" + i))[2];
                if(type == 0) {
                    go.GetComponent<MachineFuel>().maxPipeLength = HelperConverter.FloatFromString(PlayerPrefs.GetString("Machine_" + i))[3];
                    go.GetComponent<MachineFuel>().currentPocket = currentPlanet.planetPockets[HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[4]];
                    go.GetComponent<MachineFuel>().airFilterLevel = HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[5];
                    go.GetComponent<MachineFuel>().pumpLevel = HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[6];
                    go.GetComponent<MachineFuel>().refineryLevel = HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[7];
                } else {
                    go.GetComponent<MachineCleaner>().airFilterLevel = HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[3];
                    go.GetComponent<MachineCleaner>().efficiencyLevel = HelperConverter.IntFromString(PlayerPrefs.GetString("Machine_" + i))[4];
                }
                allMachines.Add(go.GetComponent<Machine>());
                currentPlanet.machines.Add(go.GetComponent<Machine>());
            }
        }

        currentDate = DateTime.Now;

        if (PlayerPrefs.HasKey("SystemTime")) {

            long temp = Convert.ToInt64(PlayerPrefs.GetString("SystemTime"));

            DateTime oldDate = DateTime.FromBinary(temp);

            difference = currentDate.Subtract(oldDate);
            print("Seconds difference: " + Mathf.FloorToInt((float)difference.TotalSeconds));
        }

    }

    void FindPlanet() {
        currentPlanet = GameObject.Find("Planet").GetComponent<Planet>();
    }

    [ContextMenu("Delete PlayerPrefs")]
    public void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
    }
}
