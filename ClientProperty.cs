using System;
using global::FlatBuffers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

namespace OpenDR
{

    public struct ClientProperty : IFlatbufferObject
    {
        private Table __p;
        public ByteBuffer ByteBuffer { get { return __p.bb; } }
        public static ClientProperty GetRootAsClientProperty(ByteBuffer _bb) { return GetRootAsClientProperty(_bb, new ClientProperty()); }
        public static ClientProperty GetRootAsClientProperty(ByteBuffer _bb, ClientProperty obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
        public static bool ClientPropertyBufferHasIdentifier(ByteBuffer _bb) { return Table.__has_identifier(_bb, "CLPR"); }
        public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
        public ClientProperty __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

        public string NodeName { get { int o = __p.__offset(4); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
        public ArraySegment<byte>? GetNodeNameBytes() { return __p.__vector_as_arraysegment(4); }
        public string MyIp { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
        public ArraySegment<byte>? GetMyIpBytes() { return __p.__vector_as_arraysegment(6); }
        public bool IsPublisher { get { int o = __p.__offset(8); return o != 0 ? 0 != __p.bb.Get(o + __p.bb_pos) : (bool)false; } }
        public int TopicPort { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
        public string TargetNodeName { get { int o = __p.__offset(12); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
        public ArraySegment<byte>? GetTargetNodeNameBytes() { return __p.__vector_as_arraysegment(12); }
        public int TargetPort { get { int o = __p.__offset(14); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

        public static Offset<ClientProperty> CreateClientProperty(FlatBufferBuilder builder,
            StringOffset nodeNameOffset = default(StringOffset),
            StringOffset myIpOffset = default(StringOffset),
            bool isPublisher = false,
            int topicPort = 0,
            StringOffset targetNodeNameOffset = default(StringOffset),
            int targetPort = 0)
        {
            builder.StartObject(6);
            ClientProperty.AddTargetPort(builder, targetPort);
            ClientProperty.AddTargetNodeName(builder, targetNodeNameOffset);
            ClientProperty.AddTopicPort(builder, topicPort);
            ClientProperty.AddMyIp(builder, myIpOffset);
            ClientProperty.AddNodeName(builder, nodeNameOffset);
            ClientProperty.AddIsPublisher(builder, isPublisher);
            return ClientProperty.EndClientProperty(builder);
        }

        public static void StartClientProperty(FlatBufferBuilder builder) { builder.StartObject(6); }
        public static void AddNodeName(FlatBufferBuilder builder, StringOffset nodeNameOffset) { builder.AddOffset(0, nodeNameOffset.Value, 0); }
        public static void AddMyIp(FlatBufferBuilder builder, StringOffset myIpOffset) { builder.AddOffset(1, myIpOffset.Value, 0); }
        public static void AddIsPublisher(FlatBufferBuilder builder, bool isPublisher) { builder.AddBool(2, isPublisher, false); }
        public static void AddTopicPort(FlatBufferBuilder builder, int topicPort) { builder.AddInt(3, topicPort, 0); }
        public static void AddTargetNodeName(FlatBufferBuilder builder, StringOffset targetNodeNameOffset) { builder.AddOffset(4, targetNodeNameOffset.Value, 0); }
        public static void AddTargetPort(FlatBufferBuilder builder, int targetPort) { builder.AddInt(5, targetPort, 0); }
        public static Offset<ClientProperty> EndClientProperty(FlatBufferBuilder builder)
        {
            int o = builder.EndObject();
            return new Offset<ClientProperty>(o);
        }
        public static void FinishClientPropertyBuffer(FlatBufferBuilder builder, Offset<ClientProperty> offset) { builder.Finish(offset.Value, "CLPR"); }
        public static void FinishSizePrefixedClientPropertyBuffer(FlatBufferBuilder builder, Offset<ClientProperty> offset) { builder.FinishSizePrefixed(offset.Value, "CLPR"); }
    };



}
