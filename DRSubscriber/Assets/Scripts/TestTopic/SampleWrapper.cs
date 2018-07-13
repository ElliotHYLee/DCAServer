using FlatBuffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleWrapper{
    FlatBufferBuilder fbb;
    Vector3 acc, gyro, mag, euler;
    Vector4 quat;
    double lat, lon;
    float alt, gsN, gsE, gsD;
    
    public SampleWrapper()
    {
        acc = new Vector3(0, 0, 0);
        gyro = new Vector3(0, 0, 0);
        mag = new Vector3(0, 0, 0);
        euler = new Vector3(0, 0, 0);
        quat = new Vector4(0, 0, 0, 0);
        lat = 0;
        lon = 0;
        alt = 0;
        gsN = 0;
        gsE = 0;
        gsD = 0;
        fbb = new FlatBufferBuilder(1);
    }

    public byte[] pack()
    {
        StringOffset fbb_name = fbb.CreateString("sample");
        Sample.StartSample(fbb);
        Sample.AddTopicName(fbb, fbb_name);
        Sample.AddAcc(fbb, Vec3.CreateVec3(fbb, acc.x, acc.y, acc.z));
        Sample.AddGyro(fbb, Vec3.CreateVec3(fbb, gyro.x, gyro.y, gyro.z));
        Sample.AddMag(fbb, Vec3.CreateVec3(fbb, mag.x, mag.y, mag.z));
        Sample.AddGps(fbb, GPS.CreateGPS(fbb, lat, lon, alt, gsN, gsE, gsD));
        Sample.AddEuler(fbb, Vec3.CreateVec3(fbb,euler.x, euler.y, euler.z));
        Sample.AddQuat(fbb, Vec4.CreateVec4(fbb, quat.w, quat.x, quat.y, quat.z));
        var offset = Sample.EndSample(fbb);
        Sample.FinishSampleBuffer(fbb, offset);
        byte[] bMsg = fbb.SizedByteArray();
        return bMsg;
    }

    #region getters and setters
    

    public double Lat
    {
        get
        {
            return lat;
        }

        set
        {
            lat = value;
        }
    }

    public double Lon
    {
        get
        {
            return lon;
        }

        set
        {
            lon = value;
        }
    }

    public float Alt
    {
        get
        {
            return alt;
        }

        set
        {
            alt = value;
        }
    }

    public float GsN
    {
        get
        {
            return gsN;
        }

        set
        {
            gsN = value;
        }
    }

    public float GsE
    {
        get
        {
            return gsE;
        }

        set
        {
            gsE = value;
        }
    }

    public float GsD
    {
        get
        {
            return gsD;
        }

        set
        {
            gsD = value;
        }
    }

    public Vector3 Acc
    {
        get
        {
            return acc;
        }

        set
        {
            acc = value;
        }
    }

    public Vector3 Gyro
    {
        get
        {
            return gyro;
        }

        set
        {
            gyro = value;
        }
    }

    public Vector3 Mag
    {
        get
        {
            return mag;
        }

        set
        {
            mag = value;
        }
    }

    public Vector3 Euler
    {
        get
        {
            return euler;
        }

        set
        {
            euler = value;
        }
    }

    public Vector4 Quat
    {
        get
        {
            return quat;
        }

        set
        {
            quat = value;
        }
    }

    #endregion
}
