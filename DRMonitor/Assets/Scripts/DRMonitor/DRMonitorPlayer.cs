using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DRMonitorPlayer : MonoBehaviour {

    public InputField ip;
    public InputField port;
    public GameObject panel_runningApps;
    public GameObject panel_coonectedApp;

    private DRMonitor drMon;

	void Start () {
        ip.text = "127.0.0.1";
        port.text = "10000";
        drMon = new DRMonitor(ip.text, int.Parse(port.text));
	}
	
	// Update is called once per frame
	void Update () {
        drMon.update(panel_runningApps, panel_coonectedApp, createNew);
    }

    void Awake()
    {
        #if UNITY_EDITOR
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 5;
        #endif
    }

    private GameObject createNew()
    {
        GameObject temp = Instantiate(panel_coonectedApp, panel_runningApps.transform);
        return temp;
    }
}
