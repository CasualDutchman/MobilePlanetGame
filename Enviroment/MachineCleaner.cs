using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineCleaner : Machine {

    Planet currentPlanet;

    public int airFilterLevel = 1;
    public int efficiencyLevel = 1;

    protected override void OnStart() {
        currentPlanet = transform.parent.parent.GetComponent<Planet>();
        currentType = Machine.Type.AIR;
        MakeSave();   
    }

    public void MakeSave() {
        PlayerPrefs.SetString("Machine_" + planetID, HelperConverter.StringFromInt(new float[] { ID, planetID, angle, airFilterLevel, efficiencyLevel }));
    }

    protected override void OnUpdate() {
        currentPlanet.polution -= ((airFilterLevel) * (efficiencyLevel * 0.6f)) * manager.polutionIncrement * Time.fixedDeltaTime;
    }
}

