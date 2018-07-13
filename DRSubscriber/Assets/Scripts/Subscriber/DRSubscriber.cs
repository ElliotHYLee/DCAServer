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

    public DRSubscriber(string name, string serverIP, int serverPort, string targetPubName)
    {
        okToSubscribe = false;
        isConnected = false;
        client = new DRSocket(name);
        client.connectToServer(serverIP, serverPort);
        isConnected = client.IsSocketReady;
        client.setMyInfo(false, "127.0.0.1", targetPubName);
        client.sendMyInfo();
        while (!client.IsAttentionRequired)
        {
            if (serverCheckCount > 60 * 5)
            {
                Debug.Log("is DRMonitor there?");
                QuitGame();
            }
            Debug.Log(client.IsAttentionRequired);
            serverCheckCount++;
            Thread.Sleep(100);
        }
    }

    public byte[] update()
    {
        if (client.IsAttentionRequired)
        {
            if (sub == null)
            {
                if (client.TargetPort==-1)
                {
                    Debug.Log("no publisher found");
                    QuitGame();
                    return null;
                }
                sub = new DRSocket(client.ClientName);
                Debug.Log("connectig to : " + client.TargetPort);
                sub.setMyInfo(false, client.MyIp, client.TargetNodeName);
                sub.connectToServer(client.MyIp, client.TargetPort);
                sub.sendMyInfo();
            }
            client.IsAttentionRequired = false;
            if(sub.isConnected()) okToSubscribe = true;
        }

        if (!okToSubscribe) return null;
        if (sub.IsNewlyReceived && okToSubscribe)
        {
            sub.IsNewlyReceived = false;
            byte[] bMsg = sub.getRecentData();
            return bMsg;
        }else return null;
     }


    public void destory()
    {
        if(client!=null)client.closeSocket();
        if(sub!=null)sub.closeSocket();
        isConnected = false;
    }

    public void QuitGame()
    {
        destory();
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                 Application.Quit();
        #endif
    }
}
