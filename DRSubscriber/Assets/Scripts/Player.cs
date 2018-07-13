using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private DRSubscriber sub;
	// Use this for initialization
	void Start () {
        sub = new DRSubscriber();
	}
	
	// Update is called once per frame
	void Update () {
        sub.update();
	}

    public void OnApplicationQuit()
    {
        if (sub != null) sub.destory();
    }

    public void OnDestroy()
    {
        if (sub != null) sub.destory();
    }

}
