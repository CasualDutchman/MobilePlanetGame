using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBeginning : MonoBehaviour {

    public bool withBeginning;
    public GameObject fader, ingameUI;
    Player player;

    bool phase1Complete, phase2Complete;
    float timer1, timer2;

    void Start() {
        if (!withBeginning) {
            transform.position = new Vector3(0, 6.7f, -10);
            ingameUI.GetComponent<RectTransform>().localPosition = new Vector3(0, 1060 - 190, 0);
           // player.currentSelected = Player.Selection.NONE;
            Destroy(fader.gameObject);
            Destroy(this);
        } else {
            fader.SetActive(true);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.currentSelected = Player.Selection.SPACESHIP;
        }
    }

    void FixedUpdate () {
        if (!phase1Complete && !phase2Complete) {
            timer1 += 1 * Time.fixedDeltaTime;
            if (timer1 >= 2) {
                phase1Complete = true;
            }
        }
        if (phase1Complete) {
            fader.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, fader.transform.GetChild(0).GetComponent<Image>().color.a - (0.6f * Time.fixedDeltaTime));
            fader.transform.GetChild(1).GetComponent<Text>().color = new Color(220 / 256f, 220/256f, 10/256f, fader.transform.GetChild(1).GetComponent<Text>().color.a - (0.6f * Time.fixedDeltaTime));
            if (fader.transform.GetChild(0).GetComponent<Image>().color.a * 256 <= 0) {
                phase2Complete = true;
            }
        }
        if (phase1Complete && phase2Complete) {
            timer2 += 2 * Time.fixedDeltaTime;

            fader.GetComponent<Image>().color = new Color(0, 0, 0, fader.GetComponent<Image>().color.a - (0.4f * Time.fixedDeltaTime));

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 6.7f, -10), 4.5f * Time.fixedDeltaTime);
            if(timer2 >= 3)
                ingameUI.GetComponent<RectTransform>().localPosition = Vector3.MoveTowards(ingameUI.GetComponent<RectTransform>().localPosition, new Vector3(0, 1060 - 190, 0), 350f * Time.fixedDeltaTime);

            if (transform.position == new Vector3(0, 6.7f, -10)) {
                player.currentSelected = Player.Selection.NONE;
                Destroy(fader.gameObject);
                Destroy(this);
            }
        }
    }
}
