using FlatBuffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DRSubscriber
{
    private bool isConnected, okToSubscribe;
    private DRSocket client, sub;
    private int targetPort;
    private int serverCheckCount;

    public bool IsConnected
    {
        get
        {
            return isConnected;
        }

        set
        {
            isConnected = value;
        }
    }

    public DRSubscriber(string name, string serverIP, int serverPort, string targetPubName)
    {
        okToSubscribe = false;
        isConnected = false;
        client = new DRSocket(name);
        client.connectToServer(serverIP, serverPort);
        isConnected = client.IsSocketReady;
        client.setMyInfo(false, "127.0.0.1", targetPubName);
        client.sendMyInfo();

        Thread connThread = new Thread(new ThreadStart(persistent));
        connThread.Start();
    }

    private void persistent()
    {
        while (!isConnected)
        {
            client.sendMyInfo();
            isConnected = client.IsSocketReady;
            Debug.Log("is DRMonitor Running?");
            Thread.Sleep((int)1000.0 / 30);
        }
    }

    public byte[] update()
    {
        if (client.IsAttentionRequired) // when server response msg NEWLY arrived
        {
            if (sub == null)
            {
                if (client.TargetPort < 1)
                {
                    Debug.Log("no publisher found");
                    client.sendMyInfo();
                    return null;
                }
                sub = new DRSocket(client.ClientName);
                Debug.Log("connectig to : " + client.TargetPort);
                sub.setMyInfo(false, client.MyIp, client.TargetNodeName);
                sub.connectToServer(client.TargetIP, client.TargetPort);
                sub.sendMyInfo();
                client.IsAttentionRequired = false;
            }

            if (sub.isConnected()) okToSubscribe = true;
            return null;
        }
        else if (okToSubscribe) // server reponse msg is already porcessed and connected to publisher
        {
            if (sub.IsNewlyReceived)
            {
                sub.IsNewlyReceived = false;
                byte[] bMsg = sub.getRecentData();
                return bMsg;
            }
            else return null;
        }
        else // when server response msg not arrived 
        {
            if (sub == null) return null;
            if (!sub.isConnected())
            {
                if (!sub.isConnected()) okToSubscribe = false;
                client.TargetPort = -1;
                client.IsAttentionRequired = true;
                sub = null;
            }
            return null;
        }
    }

    public void destory()
    {
        if (client != null) client.closeSocket();
        if (sub != null) sub.closeSocket();
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
