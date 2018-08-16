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
    public PublisherLoop Loop;
    private double updateRate; 
    bool okToPublish;
    int serverCheckCount=0;
    #endregion

    private DRSocket client;
    private DRNodeManager topic1;
    bool isConnected;

    public double Update_rate
    {
        get
        {
            return updateRate;
        }

        set
        {
            updateRate = value;
        }
    }

    public bool OkToPublish
    {
        get
        {
            return okToPublish;
        }

        set
        {
            okToPublish = value;
        }
    }

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

    #region constructors & helpers

    private void openTopicRegistrationDesk()
    {
        topic1 = new DRNodeManager("127.0.0.1", client.TopicPort);
    }

    private void constructorHelper(string name, string serverIP, int serverPort)
    {
        isConnected = false;
        okToPublish = false;
        updateRate = 0;
        client = new DRSocket(name);
        client.IsPublisher = true;
        client.connectToServer(serverIP, serverPort);
        isConnected = client.IsSocketReady;
        client.sendMyInfo();
        while(!client.IsAttentionRequired)
        {
            if(serverCheckCount > 60*5)
            {
                Debug.Log("is DRMonitor there?");
                throw new Exception();
            }
            Debug.Log(client.IsAttentionRequired);
            serverCheckCount++;
            Thread.Sleep(20);
        }
        okToPublish = true;
        Debug.Log("port to pub: " + client.TopicPort);
        openTopicRegistrationDesk();
    }

    public DRPublisher(string name, string serverIP, int serverPort)
    {
        constructorHelper(name, serverIP, serverPort);
    }

    public DRPublisher(string name, string serverIP, int serverPort, double updateRate=100)
    {
        if (updateRate<=60) throw new Exception("For less than 60Hz, please use Unity's main thread");
        constructorHelper(name, serverIP, serverPort);
        this.updateRate = updateRate;
        Loop = null;
        if (isConnected)
        {
            thPub = new Thread(runPUb);
            thPub.Start();
        }
    }
    
    #endregion

    private void runPUb()
    {
        while(!okToPublish)
        {
            // check if port is received from server
            // okToPublish  = check();
            Thread.Sleep((int)Math.Round(1000.0 / updateRate));
        }

        while(okToPublish)
        {
            if(Loop!=null) Loop();
            else
            {
                Debug.Log("Assing the publisher main loop!!");
                destory();
            }
            Thread.Sleep((int)Math.Round(1000.0 / updateRate));
        }  
    }

    public void publish(byte[] msg)
    {
        if (okToPublish)
        {
            topic1.broadcast(msg);
            Debug.Log("publishing");
        }
    }

    public void destory()
    {
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
