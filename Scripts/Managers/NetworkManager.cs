using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Core;
using UnityEngine;
using Utils;

namespace Managers
{
    public class NetWorkManager : UnitySingleton<NetWorkManager>
    {
        // 47.100.169.193
        private static IPEndPoint _remote = new IPEndPoint(IPAddress.Parse("192.168.3.125"), 6700);
        
        private UdpClient _udpClient;

        public void Init()
        {
            try
            {
                _udpClient = new UdpClient(6701);
                _udpClient.Connect(_remote);
            }
            catch (Exception ex)
            {
                LogUtils.Log(ex.ToString());
            }
            
        }

        public void Send<T>(T msg) where T : NinjaMessage
        {
            byte[] dgram = MessageEncodingUtils.Encode(msg);
            // Debug.Log("send message");
            _udpClient.Send(dgram, dgram.Length);
        }
        
        private NinjaMessage Receive()
        {
            byte[] bytes = _udpClient.Receive(ref _remote);
            // Debug.Log("receive message");
            return MessageEncodingUtils.Decode(bytes);
        }
        
        /**
         * 接受指定类型的msg，其他直接丢弃
         */
        public T Receive<T>() where T : NinjaMessage
        {
            T msg = null;
            while (msg == null)
            {
                msg = Receive() as T;
            }
            return msg;
        }
    }
}