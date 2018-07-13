using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

public class DRNodeManager {
    private Dictionary<string, DRSocket> nodeDict;
    private DRListener registrationDesk;

    public Dictionary<string, DRSocket> NodeDict
    {
        get {return nodeDict;}
        set {nodeDict = value;}
    }

    public DRNodeManager(string ip, int port = 10000)
    {
        nodeDict = new Dictionary<string, DRSocket>();
        registrationDesk = new DRListener(ip, port);
        registrationDesk.onRequest = addNewNode;
        Debug.Log("topic opened at port: " + port);
    }

    private void addNewNode(DRSocket drSocket)
    {
        int count = 0;
        while (drSocket.ClientName.Length >= 5 && drSocket.ClientName.Substring(0, 5).Equals("guest"))
        {
            if (count > 10 * 10) throw new Exception();
            Thread.Sleep(100);
            count++;
        }
        if (nodeDict.ContainsKey(drSocket.ClientName)) nodeDict[drSocket.ClientName] = drSocket;
        else nodeDict.Add(drSocket.ClientName, drSocket);
        Debug.Log("new app attached");
    }

    public void broadcast(string msg)
    {
        byte[] bMsg = Encoding.UTF8.GetBytes(msg);
        broadcast(bMsg);
    }

    public void broadcast(byte[] msg)
    {
        foreach (KeyValuePair<string, DRSocket> nodePair in nodeDict)
        {
            if (nodePair.Value.isConnected()) nodePair.Value.send(msg);

        }
    }

}
