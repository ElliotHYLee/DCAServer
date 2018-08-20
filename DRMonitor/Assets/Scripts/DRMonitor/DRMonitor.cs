using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DRMonitor
{
    public delegate GameObject AddNewAppPanel();

    private DRNodeManager nodeManager;
    private Dictionary<string, GameObject> connectedAppDict;
    int port;

    public DRMonitor(string ip, int port)
    {
        connectedAppDict = new Dictionary<string, GameObject>();
        this.port = port;
        nodeManager = new DRNodeManager(ip, port);
    }

    public void update(GameObject panel_runningApps, GameObject connectedApp, AddNewAppPanel func)
    {
        Dictionary<string, DRSocket> nodeDict = nodeManager.NodeDict;
        Debug.Log("here: " + nodeDict.Count);
        // deal with attention request
        foreach (KeyValuePair<string, DRSocket> nodePair in nodeDict)
        {
            DRSocket node = nodePair.Value;
            if (node.isConnected() && node.IsAttentionRequired)
            {
                // if node is publisher
                if (node.IsPublisher)
                {
                    node.TopicPort = ++port;
                    node.sendMyInfo();
                }
                // if node is subscriber, find publisher and notice the publisher ip & port.
                else
                {
                    string targetName = node.TargetNodeName;
                    Debug.Log("++++++" + targetName);
                    Debug.Log("++++++" + nodeDict.ContainsKey(targetName));
                    if (nodeDict.ContainsKey(targetName)) Debug.Log("++++++" + nodeDict[targetName].isConnected());
                    if (nodeDict.ContainsKey(targetName) && nodeDict[targetName].isConnected())
                    {
                        DRSocket temp = nodeDict[targetName];
                        node.TargetPort = temp.TopicPort;
                        node.TargetIP = temp.MyIp;
                        Debug.Log(temp.TopicPort);
                        var targetNode = nodeDict[targetName];
                        node.sendMyInfo();
                        Debug.Log(node.ClientName + " is requesting " + node.TargetNodeName + " : " + node.TargetIP + " : " + node.TargetPort);
                    }
                    else
                    {
                        node.TargetIP = "0.0.0.0";
                        node.TargetPort = -1;
                        node.sendMyInfo();
                    }
                }
                node.IsAttentionRequired = false;
            }

            // update GUI
            if (node.ClientName.Length>=5)
            {
                if (node.ClientName.Substring(0, 5).Equals("guest")) return;
            }

            string text = getAppInfo(node);
            if (connectedAppDict.ContainsKey(node.ClientName))
            {
                GameObject temp = connectedAppDict[node.ClientName];
                temp.GetComponentInChildren<Text>().text = text;
            }
            else //creat and add
            {
                GameObject temp = func();
                temp.GetComponentInChildren<Text>().text = text;
                connectedAppDict.Add(node.ClientName, temp);
            }
        }
    }

    private string getAppInfo(DRSocket node)
    {
        string text = "";
        if (node.isConnected())
        {
            Debug.Log(node.ClientName + " is alive");
            text = node.ClientName + " alive";
            if (node.IsPublisher) text += " at " + node.MyIp + ":" + node.TopicPort;
        }
        else text = node.ClientName + " lost";
        return text;
    }
}
