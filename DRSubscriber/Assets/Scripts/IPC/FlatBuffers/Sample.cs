// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

using global::System;
using global::FlatBuffers;

public struct Vec4 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Vec4 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float W { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float X { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float Y { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }
  public float Z { get { return __p.bb.GetFloat(__p.bb_pos + 12); } }

  public static Offset<Vec4> CreateVec4(FlatBufferBuilder builder, float W, float X, float Y, float Z) {
    builder.Prep(4, 16);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    builder.PutFloat(W);
    return new Offset<Vec4>(builder.Offset);
  }
};

public struct Vec3 : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Vec3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float X { get { return __p.bb.GetFloat(__p.bb_pos + 0); } }
  public float Y { get { return __p.bb.GetFloat(__p.bb_pos + 4); } }
  public float Z { get { return __p.bb.GetFloat(__p.bb_pos + 8); } }

  public static Offset<Vec3> CreateVec3(FlatBufferBuilder builder, float X, float Y, float Z) {
    builder.Prep(4, 12);
    builder.PutFloat(Z);
    builder.PutFloat(Y);
    builder.PutFloat(X);
    return new Offset<Vec3>(builder.Offset);
  }
};

public struct GPS : IFlatbufferObject
{
  private Struct __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public GPS __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public double Lat { get { return __p.bb.GetDouble(__p.bb_pos + 0); } }
  public double Lon { get { return __p.bb.GetDouble(__p.bb_pos + 8); } }
  public float Alt { get { return __p.bb.GetFloat(__p.bb_pos + 16); } }
  public float GsN { get { return __p.bb.GetFloat(__p.bb_pos + 20); } }
  public float GsE { get { return __p.bb.GetFloat(__p.bb_pos + 24); } }
  public float GsD { get { return __p.bb.GetFloat(__p.bb_pos + 28); } }

  public static Offset<GPS> CreateGPS(FlatBufferBuilder builder, double Lat, double Lon, float Alt, float GsN, float GsE, float GsD) {
    builder.Prep(8, 32);
    builder.PutFloat(GsD);
    builder.PutFloat(GsE);
    builder.PutFloat(GsN);
    builder.PutFloat(Alt);
    builder.PutDouble(Lon);
    builder.PutDouble(Lat);
    return new Offset<GPS>(builder.Offset);
  }
};

public struct Sample : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static Sample GetRootAsSample(ByteBuffer _bb) { return GetRootAsSample(_bb, new Sample()); }
  public static Sample GetRootAsSample(ByteBuffer _bb, Sample obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool SampleBufferHasIdentifier(ByteBuffer _bb) { return Table.__has_identifier(_bb, "SMPL"); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public Sample __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public string TopicName { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
  public ArraySegment<byte>? GetTopicNameBytes() { return __p.__vector_as_arraysegment(4); }
  public Vec3? Gyro { get { int o = __p.__offset(6); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Vec3? Acc { get { int o = __p.__offset(8); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Vec3? Mag { get { int o = __p.__offset(10); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Vec3? Euler { get { int o = __p.__offset(12); return o != 0 ? (Vec3?)(new Vec3()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public Vec4? Quat { get { int o = __p.__offset(14); return o != 0 ? (Vec4?)(new Vec4()).__assign(o + __p.bb_pos, __p.bb) : null; } }
  public GPS? Gps { get { int o = __p.__offset(16); return o != 0 ? (GPS?)(new GPS()).__assign(o + __p.bb_pos, __p.bb) : null; } }

  public static void StartSample(FlatBufferBuilder builder) { builder.StartObject(7); }
  public static void AddTopicName(FlatBufferBuilder builder, StringOffset topicNameOffset) { builder.AddOffset(0, topicNameOffset.Value, 0); }
  public static void AddGyro(FlatBufferBuilder builder, Offset<Vec3> gyroOffset) { builder.AddStruct(1, gyroOffset.Value, 0); }
  public static void AddAcc(FlatBufferBuilder builder, Offset<Vec3> accOffset) { builder.AddStruct(2, accOffset.Value, 0); }
  public static void AddMag(FlatBufferBuilder builder, Offset<Vec3> magOffset) { builder.AddStruct(3, magOffset.Value, 0); }
  public static void AddEuler(FlatBufferBuilder builder, Offset<Vec3> eulerOffset) { builder.AddStruct(4, eulerOffset.Value, 0); }
  public static void AddQuat(FlatBufferBuilder builder, Offset<Vec4> quatOffset) { builder.AddStruct(5, quatOffset.Value, 0); }
  public static void AddGps(FlatBufferBuilder builder, Offset<GPS> gpsOffset) { builder.AddStruct(6, gpsOffset.Value, 0); }
  public static Offset<Sample> EndSample(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<Sample>(o);
  }
  public static void FinishSampleBuffer(FlatBufferBuilder builder, Offset<Sample> offset) { builder.Finish(offset.Value, "SMPL"); }
  public static void FinishSizePrefixedSampleBuffer(FlatBufferBuilder builder, Offset<Sample> offset) { builder.FinishSizePrefixed(offset.Value, "SMPL"); }
};

