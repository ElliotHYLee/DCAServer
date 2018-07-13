using FlatBuffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DRSubscriber {
    private bool isConnected, okToSubscribe;
    private DRSocket client, sub;
    private int targetPort;
    private int serverCheckCount;

    public DRSubscriber()
    {
        okToSubscribe = false;
        isConnected = false;
        client = new DRSocket("sub1");
        client.connectToServer("127.0.0.1", 10000);
        isConnected = client.IsSocketReady;
        client.setMyInfo(false, "127.0.0.1", "pub1");
        client.sendMyInfo();
        while (!client.IsAttentionRequired)
        {
            if (serverCheckCount > 60 * 10)
            {
                Debug.Log("is DRMonitor there?");
                throw new Exception();
            }
            Debug.Log(client.IsAttentionRequired);
            serverCheckCount++;
            Thread.Sleep(20);
        }
    }

    public void update()
    {
        if (client.IsAttentionRequired)
        {
            if (sub == null)
            {
                sub = new DRSocket("sub1");
                Debug.Log("connectig to : " + client.TargetPort);
                sub.setMyInfo(false, client.MyIp, client.TargetNodeName);
                sub.connectToServer(client.MyIp, client.TargetPort);
                sub.sendMyInfo();
            }
            client.IsAttentionRequired = false;
            if(sub.isConnected()) okToSubscribe = true;
        }

        if (!okToSubscribe) return;
        else Debug.Log("subscribing...");
        if (sub.IsNewlyReceived && okToSubscribe)
        {
            sub.IsNewlyReceived = false;
            byte[] bMsg = sub.getRecentData();
            Debug.Log("subscribing 1");
            ByteBuffer bb = new ByteBuffer(bMsg);
            if (Sample.SampleBufferHasIdentifier(bb))
            {
                Debug.Log("subscribing 2");
                Sample data = Sample.GetRootAsSample(bb);
                var temp = data.Acc.Value;
                Vector3 acc = new Vector3(temp.X, temp.Y, temp.Z);

                Debug.Log("acc: " +  acc.x + ", " + acc.y + ", " + acc.z);
            }

        }
        

     }


    public void destory()
    {
        client.closeSocket();
        sub.closeSocket();
        isConnected = false;
    }
}
