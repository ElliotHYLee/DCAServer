#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class DRPublisher {
    #region publisher thread
    private Thread thPub;
    public delegate void PublisherLoop();
    public PublisherLoop pubLoop;
    private double updateRate; 
    bool okToPublish;
    int serverCheckCount=0;
    #endregion

    private DRSocket client;
    private DRNodeManager topicManager;
    bool isConnected;
    int knockServerCounter = 0;
    string serverIP;
    int serverPort;
    
    #region constructors & helpers

    private void openTopicRegistrationDesk()
    {
        topicManager = new DRNodeManager("127.0.0.1", client.TopicPort);
    }

    private void constructorHelper(string name, string serverIP, int serverPort)
    {
        this.serverIP = serverIP;
        this.serverPort = serverPort;
        isConnected = false;
        okToPublish = false;
        updateRate = 0;
        client = new DRSocket(name);
        client.IsPublisher = true;
        client.connectToServer(serverIP, serverPort);
        isConnected = client.IsSocketReady;

        Thread connThread = new Thread(new ThreadStart(connMethod));
        connThread.Start();
    }

    private void connMethod()
    {
        while (!client.IsAttentionRequired)
        {
            client.sendMyInfo();
            Debug.Log("is DRMonitor there?");
            isConnected = client.IsSocketReady;
            Thread.Sleep(500);
            knockServerCounter++;
            if (knockServerCounter > 10)
            {
                client.connectToServer(serverIP, serverPort);
                knockServerCounter = 0;
            }
        }
        okToPublish = true;
        Debug.Log("port to pub: " + client.TopicPort);
        openTopicRegistrationDesk();
        Debug.Log("after regi desk");
    }

    public void useThread(double updateRate = 100, PublisherLoop pubLoop = null)
    {
        this.updateRate = updateRate;
        this.pubLoop = pubLoop;
        Thread.Sleep(500);
        thPub = new Thread(new ThreadStart(runPUb));
        thPub.Start();
    }

    public DRPublisher(string name, string serverIP, int serverPort)
    {
        constructorHelper(name, serverIP, serverPort);
    }

    #endregion

    private void runPUb()
    {
        while(true)
        {
            if (pubLoop!=null) pubLoop();
            else Debug.Log("Publication method is not defined.");
            Thread.Sleep((int)Math.Round(1000.0 / updateRate));
        }  
    }

    public void publish(byte[] msg)
    {
        if (okToPublish)
        {
            topicManager.broadcast(msg);
            Debug.Log("publishing");
        }
    }

    public void destory()
    {
        topicManager = null;
        client.closeSocket();
        isConnected = false;
        okToPublish = false;
        while(thPub!=null && thPub.IsAlive)
        {
            thPub.Abort();
            Debug.Log("trying to stop thread");
        }
    }



}
