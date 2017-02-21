using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineFuel : Machine {

    public Pocket currentPocket;

    public int airFilterLevel = 1;
    public int pumpLevel = 1;
    public int refineryLevel = 1;

    float pipeLength;
    public float maxPipeLength;

    LineRenderer line;
    bool pipeExtended;

    Planet currentPlanet;

    protected override void OnStart() {
        line = transform.GetChild(0).GetComponent<LineRenderer>();
        currentPlanet = transform.parent.parent.GetComponent<Planet>();

        if (!PlayerPrefs.HasKey("Machine_" + planetID)) {
            RaycastHit2D hit = Physics2D.Raycast(transform.GetChild(0).position, -Vector2.up, 7, 1 << LayerMask.NameToLayer("Pocket"));
            if (hit.collider != null) {
                currentPocket = hit.collider.GetComponent<Pocket>();
                maxPipeLength = Vector2.Distance(transform.GetChild(0).position, hit.point) + 0.1f;
                maxPipeLength *= 1 / transform.GetChild(0).localScale.y;
            }
        }

        MakeSave();
    }

    public void MakeSave() {
        PlayerPrefs.SetString("Machine_" + planetID, HelperConverter.StringFromInt(new float[] { ID, planetID, angle, maxPipeLength, currentPocket.planetID, airFilterLevel, pumpLevel, refineryLevel }));
    }

    protected override void OnUpdate() {
        if (!pipeExtended) {
            pipeLength -= 4f * Time.fixedDeltaTime;
            line.SetPosition(1, new Vector3(0, pipeLength, 0.1f));
            if(Mathf.Abs(pipeLength) >= maxPipeLength) {
                pipeExtended = true;
            }
        }
        
        //===== TODO

        if (pipeExtended && currentPocket.amount > 0) {
            currentPocket.amount -= (1 / (float)pumpLevel) * Time.fixedDeltaTime;
            manager.money += (refineryLevel) * manager.moneyIncrement * Time.fixedDeltaTime;
            currentPlanet.polution += ((refineryLevel + pumpLevel) - airFilterLevel) * manager.polutionIncrement * Time.fixedDeltaTime;
        }
    }
}
