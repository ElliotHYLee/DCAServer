using FlatBuffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private DRSubscriber sub;
	// Use this for initialization
	void Start () {
        sub = new DRSubscriber("sub1", "127.0.0.1", 10000, "pub1");
        sub.useThread(60, subscribeHere);
	}
	
	// Update is called once per frame
	void Update () {
        //subscribeHere();
    }

    public void subscribeHere()
    {
        byte[] bMsg = sub.update();

        if (bMsg != null)
        {
            Debug.Log("subscribing 1");
            ByteBuffer bb = new ByteBuffer(bMsg);
            if (Sample.SampleBufferHasIdentifier(bb))
            {
                Debug.Log("subscribing 2");
                Sample data = Sample.GetRootAsSample(bb);
                var temp = data.Acc.Value;
                Vector3 acc = new Vector3(temp.X, temp.Y, temp.Z);
                Debug.Log("acc: " + acc.x + ", " + acc.y + ", " + acc.z);
            }
        }

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
