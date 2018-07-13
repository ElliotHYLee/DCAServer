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

    public class DRListener
    {
        public delegate void NewRequest(DRSocket drSocket);
        public NewRequest onRequest;

        private TcpListener tcp_listener;
        private bool isListening;

        public bool IsListening
        {
            get
            {
                return isListening;
            }

            set
            {
                isListening = value;
            }
        }

        public DRListener(string ip, int port)
        {
            tcp_listener = new TcpListener(IPAddress.Any, port);
            tcp_listener.Start();
            async_startListening();
            isListening = true;
        }

        private void async_startListening()
        {
            tcp_listener.BeginAcceptTcpClient(async_onNewRequest, tcp_listener);
        }

        private void async_onNewRequest(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            var drSocket = new DRSocket(listener.EndAcceptSocket(ar));
            if (onRequest == null) throw new NullReferenceException();
            async_startListening();
            onRequest(drSocket);
        }
    }
}