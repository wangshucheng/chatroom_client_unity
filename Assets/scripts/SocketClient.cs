using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class SocketClient
{

    Socket _socket;

    public SocketClient()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    }

    public bool Connected
    {
        get { return _socket != null && _socket.Connected; }
    }

    public void Connect(string host, int port)
    {
        try
        {
            _socket.BeginConnect(new IPEndPoint(IPAddress.Parse(host), port), asyncResult=> {
                Debug.Log(Connected);
                if (!_socket.Connected) return;
                _socket.EndConnect(asyncResult);
                Receive();
            }, null);
        }
        catch (Exception ex)
        {
            Debug.LogWarning(string.Format("异常信息：", ex.Message));
        }
    }

    private void Receive()
    {
        if (!_socket.Connected) return;
        byte[] receiveBuffer = new byte[1024];
        try
        {
            _socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, asyncResult =>
            {
                int length = _socket.EndReceive(asyncResult);
                Debug.Log(string.Format("收到服务器消息:{0}", Encoding.UTF8.GetString(receiveBuffer)));
                Receive();
            }, null);
        }
        catch (Exception ex)
        {
            Debug.LogWarning(string.Format("异常信息：", ex.Message));
        }
    }

    public void Send(string message)
    {
        if (!_socket.Connected) return;
        //编码
        byte[] sendBuffer = Encoding.UTF8.GetBytes(message);
        try
        {
            _socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, asyncResult =>
            {
                //完成发送消息
                int length = _socket.EndSend(asyncResult);
                Debug.Log(string.Format("客户端发送消息:{0}", message));
            }, null);
        }
        catch (Exception ex)
        {
            Debug.LogWarning(string.Format("异常信息：{0}", ex.Message));
        }

    }
}
