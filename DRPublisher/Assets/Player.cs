#pragma warning disable 0168
#pragma warning disable 0219

using FlatBuffers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    DRPublisher pub;
    float accx, accy, accz;
	// Use this for initialization
	void Start () {
        accx = 0;
        accy = 0;
        accz = 0;
        pub = new DRPublisher("pub1", "127.0.0.1", 10000);
        pub.useThread(60, publishMethod);
    }

    void Update()
    {
        //publishMethod();
    }

    private void publishMethod()
    {
        if (accx < 5000) accx += 2;
        else accx = 0;
        if (accy < 5000) accy += 1;
        else accy = 0;
        if (accz < 5000) accz += 5;
        else accz = 0;

        FlatBufferBuilder fbb = new FlatBufferBuilder(1);
        StringOffset fbb_name = fbb.CreateString("sample");
        Sample.StartSample(fbb);
        Sample.AddTopicName(fbb, fbb_name);
        Sample.AddAcc(fbb, Vec3.CreateVec3(fbb, accx, accy, accz));
        Sample.AddGyro(fbb, Vec3.CreateVec3(fbb, 3, 2, 1));
        Sample.AddMag(fbb, Vec3.CreateVec3(fbb, 4, 5, 6));
        Sample.AddGps(fbb, GPS.CreateGPS(fbb, 84.123456, 84.123456, 213, 30, 20, 10));
        Sample.AddEuler(fbb, Vec3.CreateVec3(fbb, 10, 15, 30));
        Sample.AddQuat(fbb, Vec4.CreateVec4(fbb, 1, 0.5f, 0.3f, 0.2f));
        var offset = Sample.EndSample(fbb);
        Sample.FinishSampleBuffer(fbb, offset);
        byte[] bMsg = fbb.SizedByteArray();
        Debug.Log(bMsg.Length);
        pub.publish(bMsg);
    }


    public void OnApplicationQuit()
    {
        if (pub != null) pub.destory();
        pub = null;
    }

    public void OnDestroy()
    {
        if (pub != null) pub.destory();
        pub = null;
    }


}