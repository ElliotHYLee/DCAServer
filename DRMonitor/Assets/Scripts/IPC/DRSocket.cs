#pragma warning disable 0168
#pragma warning disable 0219


using FlatBuffers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

/** DRSocket Class: 
 **/
public class DRSocket
{
    private static int tempSocketID = 0;
    private string clientName;
    private string myIp;
    private bool isPublisher;
    private int topicPort;
    private bool isAttentionRequired;
    private string targetNodeName;
    private int targetPort;

    protected Socket tcp;
    protected TcpClient client;
    protected bool isNewlyRecieved;
    protected string recievedData;
    protected bool isSocketReady;
    protected List<byte[]> dataStorage;
    protected byte[] readBuffer;
    private int dataStoragePtr, dataStoragePtrMostRecent, maxNumDataStore;

    #region Constructors
    /// <summary>
    /// Base constructor
    /// </summary>
    public DRSocket()
    {
        client = new TcpClient();
        readBuffer = new byte[8192];
        dataStoragePtrMostRecent = 0;
        maxNumDataStore = 10;
        dataStoragePtr = 0;
        dataStorage = new List<byte[]>();
        isSocketReady = false;
        targetNodeName = "";
    }

    /// <summary>
    /// Can be called when deep copy is necessary.
    /// </summary>
    /// <param name="drSocket">The object to deep copy</param>
    public DRSocket(DRSocket drSocket) : this()
    {
        dataStoragePtr = drSocket.DataStoragePtr;
        dataStorage = drSocket.DataStorage;
        tcp = drSocket.Tcp;
        clientName = drSocket.ClientName;
        isNewlyRecieved = drSocket.IsNewlyReceived;
        recievedData = drSocket.RecievedData;
        isSocketReady = drSocket.IsSocketReady;
    }

    /// <summary>
    /// This is called when client first initialized. 
    /// </summary>
    /// <param name="name">name of this socket for server</param>
    public DRSocket(String name) : this()
    {
        clientName = name;
    }

    /// <summary>
    /// This is called when server accpets the client.
    /// </summary>
    /// <param name="clientSocket">new socket returned by listener</param>
    public DRSocket(Socket clientSocket) : this("guest" + (++tempSocketID).ToString())
    {
        tcp = clientSocket;
        client.Client = tcp;
        isSocketReady = true;
        client.GetStream().BeginRead(readBuffer, 0, readBuffer.Length, onRead, null);
    }
    #endregion

    #region Getters & Setters

    public byte[] getRecentData()
    {
        byte[] temp = null;
        try
        {
            temp = dataStorage[dataStoragePtrMostRecent];
        }
        catch (Exception e)
        {
            Debug.Log("count: " + dataStorage.Count);
            Debug.Log("dataStoragePtr: " + dataStoragePtr);
            Debug.Log("dataStoragePtrMostRecent: " + dataStoragePtrMostRecent);
        }
        return temp;
    }

    public Socket Tcp
    {
        get { return tcp; }
        set { tcp = value; }
    }

    public bool IsNewlyReceived
    {
        get { return isNewlyRecieved; }
        set { isNewlyRecieved = value; }
    }

    public string RecievedData
    {
        get
        {
            isNewlyRecieved = false;
            return recievedData;
        }

        set
        {
            isNewlyRecieved = true;
            recievedData = value;
        }
    }

    public bool IsSocketReady
    {
        get { return isSocketReady; }
        set { isSocketReady = value; }
    }

    public string ClientName
    {
        get { return clientName; }
        set { clientName = value; }
    }

    public int DataStoragePtr
    {
        get
        {
            return dataStoragePtr;
        }

        set
        {
            dataStoragePtr = value;
        }
    }

    public List<byte[]> DataStorage
    {
        get
        {
            return dataStorage;
        }

        set
        {
            dataStorage = value;
        }
    }

    public int MaxNumDataStore
    {
        get
        {
            return maxNumDataStore;
        }

        set
        {
            maxNumDataStore = value;
        }
    }

    public bool IsAttentionRequired
    {
        get
        {
            return isAttentionRequired;
        }

        set
        {
            isAttentionRequired = value;
        }
    }

    public bool IsPublisher
    {
        get
        {
            return isPublisher;
        }

        set
        {
            isPublisher = value;
        }
    }

    public int TopicPort
    {
        get
        {
            return topicPort;
        }

        set
        {
            topicPort = value;
        }
    }

    public string MyIp
    {
        get
        {
            return myIp;
        }

        set
        {
            myIp = value;
        }
    }

    public string TargetNodeName
    {
        get
        {
            return targetNodeName;
        }

        set
        {
            targetNodeName = value;
        }
    }

    public int TargetPort
    {
        get
        {
            return targetPort;
        }

        set
        {
            targetPort = value;
        }
    }


    #endregion

    #region Connection establishment
    /// <summary>
    /// Client side object needs to call this to connect to server
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    public void connectToServer(string host, int port)
    {
        if (isSocketReady) return;
        try
        {
            client.BeginConnect(host, port, (ar) => endConnect(ar), null);
            tcp = client.Client;
            Thread.Sleep(1000);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            isSocketReady = false;
        }
    }

    /// <summary>
    /// Async end connection and start reading as the data comes in.
    /// </summary>
    /// <param name="ar"></param>
    private void endConnect(IAsyncResult ar)
    {
        try
        {
            client.EndConnect(ar);
            client.GetStream().BeginRead(readBuffer, 0, readBuffer.Length, onRead, null);
            isSocketReady = true;
            Debug.Log("coonected");
        }
        catch (Exception e)
        {
            isSocketReady = false;
            Debug.Log("is server alive?");
        }
    }

    #endregion

    #region msg processing

    /// <summary>
    /// Routine for reading the data
    /// </summary>
    /// <param name="ar"></param>
    private void onRead(IAsyncResult ar)
    {

        int length = client.GetStream().EndRead(ar);
        if (length <= 0) // Connection closed
        {
            isSocketReady = false;
            return;
        }
        //readBuffer = new byte[8192];
        parseRecievedData(readBuffer);
        client.GetStream().BeginRead(readBuffer, 0, readBuffer.Length, onRead, null);
    }

    /// <summary>
    /// Parse received msg
    /// </summary>
    /// <param name="ba">the new received msg</param>
    protected void parseRecievedData(byte[] ba)
    {
        Debug.Log("sth came in");
        ByteBuffer bb = new ByteBuffer(ba);
        if (ClientProperty.ClientPropertyBufferHasIdentifier(bb))
        {
            ClientProperty data = ClientProperty.GetRootAsClientProperty(bb);
            clientName = data.NodeName;
            myIp = data.MyIp;
            isPublisher = data.IsPublisher;
            topicPort = data.TopicPort;
            targetNodeName = data.TargetNodeName;
            targetPort = data.TargetPort;
            isAttentionRequired = true;
            Debug.Log("name came in");
        }
        else updateDataStorage(ba);

    }

    /// <summary>
    /// Put the new msg to the data storage
    /// </summary>
    /// <param name="ba">byte array containing msg</param>
    protected void updateDataStorage(byte[] ba)
    {
        Debug.Log("im here111: " + dataStorage.Count);
        if (dataStorage.Count < maxNumDataStore)
        {
            dataStorage.Add(ba);
            dataStoragePtr = dataStorage.Count - 1;
        }
        else
        {
            if (dataStoragePtr + 1 >= maxNumDataStore) dataStoragePtr = 0;
            dataStorage[dataStoragePtr] = ba;
            dataStoragePtr++;
        }
        if (dataStoragePtr < 9) dataStoragePtrMostRecent = dataStoragePtr - 1;
        else dataStoragePtrMostRecent = maxNumDataStore - 1;
        isNewlyRecieved = true;
    }

    #endregion

    #region send
    /// <summary>
    /// Send this client information to the server
    /// </summary>
    public void sendMyInfo()
    {
        FlatBufferBuilder fbb = new FlatBufferBuilder(1);
        StringOffset fbb_name = fbb.CreateString(clientName);
        StringOffset fbb_myIp = fbb.CreateString("127.0.0.1");
        StringOffset fbb_targetNodeName = fbb.CreateString(targetNodeName);
        Debug.Log("packing clinet name: " + clientName);
        ClientProperty.StartClientProperty(fbb);
        ClientProperty.AddNodeName(fbb, fbb_name);
        ClientProperty.AddIsPublisher(fbb, isPublisher);
        ClientProperty.AddMyIp(fbb, fbb_myIp);
        ClientProperty.AddTopicPort(fbb, topicPort);
        ClientProperty.AddTargetNodeName(fbb, fbb_targetNodeName);
        ClientProperty.AddTargetPort(fbb, targetPort);
        var offset = ClientProperty.EndClientProperty(fbb);
        ClientProperty.FinishClientPropertyBuffer(fbb, offset);
        byte[] bMsg = fbb.SizedByteArray();
        send(bMsg);
    }

    public void setMyInfo(bool isPublisher, string myIp, string targetNodeName)
    {
        this.isPublisher = isPublisher;
        this.myIp = myIp;
        this.targetNodeName = targetNodeName;
    }

    /// <summary>
    /// send string data
    /// </summary>
    /// <param name="data">string msg</param>
    public void send(string data)
    {
        byte[] byData = string2byteArr(data);
        send(byData);
    }

    /// <summary>
    /// Lowest level send. 
    /// </summary>
    /// <param name="bData">byte[] msg</param>
    public void send(byte[] bData)
    {
        try
        {
            tcp.Send(bData);
        }
        catch (Exception e)
        {
            Debug.Log("write error: " + e.Message + " to clinet " + clientName);
        }
    }

    #endregion

    #region Helper functions
    /// <summary>
    /// Checks if the tcp socket is still connected or not.
    /// </summary>
    /// <returns>true if alive, false otherwise</returns>
    public bool isConnected()
    {
        try
        {
            if (tcp != null && tcp.Connected)
            {
                if (tcp.Poll(0, SelectMode.SelectRead)) return !(tcp.Receive(new byte[1], SocketFlags.Peek) == 0);
                else return true;
            }
            else return false;
        }
        catch
        {
            return false;
        }
    }

    protected byte[] string2byteArr(string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }

    protected string byteArr2string(byte[] ba)
    {
        return Encoding.UTF8.GetString(ba);
    }
    #endregion

    public void closeSocket()
    {
        if (isSocketReady)
        {
            isSocketReady = false;
            tcp.Close();
        }

    }
}