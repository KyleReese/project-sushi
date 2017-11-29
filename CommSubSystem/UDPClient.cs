﻿using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Messages;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using SharedObjects;

namespace CommSubSystem
{
    public class UDPClient
    {
        public IPEndPoint myIP;
        public UdpClient _udpClient;
        public IPEndPoint _serverIp;
        private static readonly object MyLock = new object();
        private static UDPClient _Instance;

        private UDPClient() { }

        public void SetupAndRun(int port)
        {
            myIP = new IPEndPoint(IPAddress.Any, port);
            _udpClient = new UdpClient(myIP);
            _udpClient.Client.ReceiveTimeout = 1000;
            _serverIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3333);
        }

        public static UDPClient UDPInstance
        {
            get 
            {
                lock(MyLock)
                {
                    if(_Instance == null)
                    {
                        _Instance = new UDPClient();
                    }
                }
                return _Instance;
            }
        }

        public PublicEndPoint GetPublicEndPoint()
        {
            return new PublicEndPoint() { IpEndPoint = myIP };
        }
        

        public IPEndPoint GetEndPoint()
        {
            return _serverIp;
        }

        public byte[] Receive(ref IPEndPoint remoteEp)
        {
            byte[] receiveBuffer = null;
            try
            {
                receiveBuffer = _udpClient.Receive(ref remoteEp);
            }
            catch { }
            return receiveBuffer;
        }
        
        public Error Send(byte[] envelope)
        {
            Error error = null;
            try
            {
                _udpClient.Send(envelope, envelope.Length, _serverIp);
            }
            catch (Exception err)
            {
                error = new Error()
                {
                    Text = $"Cannnot send a message to {_serverIp}: {err.Message}"
                };
            }
            return error;

        }


        //We'll want to change this to allow for multiple servers and multicasting
        public void SetServerIP(string Address, string port)
        {
            _serverIp = new IPEndPoint(IPAddress.Parse(Address), Convert.ToInt32(port));
        }

        public void SetServerIP(IPEndPoint serverIP)
        {
            _serverIp = serverIP;
        }

        public static Envelope Decode(byte[] message)
        {
            Envelope result = null;

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream(message);

            result = (Envelope)formatter.Deserialize(stream);

            return result;
        }
        
    }
}
