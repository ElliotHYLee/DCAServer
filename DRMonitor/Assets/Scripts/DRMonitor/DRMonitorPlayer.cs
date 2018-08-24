using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class DRMonitorPlayer : MonoBehaviour {

    public InputField ip;
    public InputField port;
    public GameObject panel_runningApps;
    public GameObject panel_coonectedApp;

    private DRMonitor drMon;

	void Start () {
        Application.runInBackground = true;
        ip.text = "127.0.0.1";
        port.text = "10000";
        drMon = new DRMonitor(ip.text, int.Parse(port.text), 10);
	}
	
	// Update is called once per frame
	void Update () {
        drMon.updateGUI(panel_runningApps, panel_coonectedApp, createNew);
        Thread.Sleep((int)1000.0 / 15);
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
