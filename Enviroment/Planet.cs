using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour {

    [Header("- Planet Colors")]
    public Color[] planetColors;
    [Header("- Planet Layers")]
    public GameObject planetOutside;
    public GameObject planetInner, planetCore;
    [Header("- Planet machines")]
    public List<Machine> machines = new List<Machine>();
    [Header("- Planet Attributes")]
    public float polution;
    public int maxPolution;
    public Pocket[] allPockets;
    public List<Pocket> planetPockets = new List<Pocket>();

    Player player;

    void Start () {
        GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>().currentPlanet = this;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        PlanetPrefs();
    }
	
    public void PlanetPrefs() {
        int randomNumber;

        if (PlayerPrefs.HasKey("Planet")) {
            int[] saved = HelperConverter.IntFromString(PlayerPrefs.GetString("Planet"));

            polution = PlayerPrefs.GetFloat("Polution");
            maxPolution = saved[0];

            planetColors[0] = HelperConverter.ColorFromInt(saved[1]);
            planetColors[1] = HelperConverter.ColorFromInt(saved[2]);
            planetColors[2] = HelperConverter.ColorFromInt(saved[3]);
            planetColors[3] = HelperConverter.ColorFromInt(saved[4]);
            planetColors[4] = HelperConverter.ColorFromInt(saved[5]);

            randomNumber = saved[6];
            for (int i = 0; i < randomNumber; i++) {
                GameObject go = Instantiate(allPockets[saved[7 + (i * 3)]].gameObject);
                go.transform.parent = transform.GetChild(1);
                go.transform.localPosition = new Vector3(0, 0, -0.01f);
                go.transform.localScale = (Vector3.one / 10) * 3;
                go.GetComponent<Pocket>().angle = saved[8 + (i * 3)];
                go.GetComponent<Pocket>().planetID = i;
                go.GetComponent<Pocket>().maxAmount = saved[9 + (i * 3)];
                planetPockets.Add(go.GetComponent<Pocket>());
            }
        } else {
            polution = Random.Range(5, 100);
            maxPolution = Random.Range(1000, 2000);

            randomNumber = Random.Range(4, 6);

            string save = maxPolution.ToString();
            save = HelperConverter.AddStringFromInt(save, HelperConverter.IntFromColor(planetColors[0]));
            save = HelperConverter.AddStringFromInt(save, HelperConverter.IntFromColor(planetColors[1]));
            save = HelperConverter.AddStringFromInt(save, HelperConverter.IntFromColor(planetColors[2]));
            save = HelperConverter.AddStringFromInt(save, HelperConverter.IntFromColor(planetColors[3]));
            save = HelperConverter.AddStringFromInt(save, HelperConverter.IntFromColor(planetColors[4]));
            save = HelperConverter.AddStringFromInt(save, randomNumber);

            for (int i = 0; i < randomNumber; i++) {
                GameObject go = Instantiate(allPockets[Random.Range(0, allPockets.Length)].gameObject);
                go.transform.parent = transform.GetChild(1);
                go.transform.localPosition = new Vector3(0, 0, -0.01f);
                go.transform.localScale = (Vector3.one / 10) * 3;
                go.GetComponent<Pocket>().angle = i * (360 / randomNumber) + (Random.Range(-15, 15));
                go.GetComponent<Pocket>().planetID = i;
                go.GetComponent<Pocket>().maxAmount = Random.Range(1000, 2000);
                go.GetComponent<Pocket>().amount = go.GetComponent<Pocket>().maxAmount;
                planetPockets.Add(go.GetComponent<Pocket>());
                save = HelperConverter.AddStringFromInt(save, go.GetComponent<Pocket>().ID);
                save = HelperConverter.AddStringFromInt(save, (int)go.GetComponent<Pocket>().angle);
                save = HelperConverter.AddStringFromInt(save, (int)go.GetComponent<Pocket>().maxAmount);
            }
            PlayerPrefs.SetString("Planet", save);
            PlayerPrefs.Save();
        }

        planetOutside.GetComponent<MeshRenderer>().material.color = planetColors[0];
        planetInner.GetComponent<MeshRenderer>().material.color = planetColors[3];
        planetCore.GetComponent<MeshRenderer>().material.color = planetColors[4];
    }

    void FixedUpdate() {
        PlayerPrefs.SetFloat("Polution", polution);
    }

    public Machine MachineInRange(Vector3 origin) {
        if (machines.Count > 0) {
            foreach (Machine machine in machines) {
                if(machine.angle > transform.eulerAngles.z - 11 && machine.angle < transform.eulerAngles.z + 11) {
                    return machine;
                }
            }
        }
        return null;
    }

    public float PolutionPercent() {
        return ((polution / maxPolution) * 100);
    }

    public bool isEmptySpot(Vector3 origin) {
        return MachineInRange(origin) == null;
    }

    public bool IsMachine(Vector3 origin) {
        return MachineInRange(origin) != null && !MachineInRange(origin).isOccupied();
    }

    public bool IsMachineOccupied(Vector3 origin) {
        return MachineInRange(origin) != null && MachineInRange(origin).isOccupied();
    }

    public void RotatePlanet(float speed) {
        transform.Rotate(transform.forward * speed *Time.deltaTime);
    }

    private float baseAngle = 0.0f;

    void OnMouseDown() {
        if (Input.touchCount > 0 && player.currentSelected == Player.Selection.NONE) {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            pos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y) - pos;
            baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
            baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;
        }
    }

    void OnMouseDrag() {
        if (Input.touchCount > 0 && player.currentSelected == Player.Selection.NONE) {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            pos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y) - pos;
            float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - baseAngle;
            transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);
        }
    }
}
