using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

    public void ConnectAsync(string host, int port)
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

    public IEnumerator Connect(string host, int port)
    {
        //try
        //{
            _socket.Connect((new IPEndPoint(IPAddress.Parse(host), port)));
            Debug.Log("连接服务器成功");
            //5.0 接收数据
            byte[] buffer = new byte[1024];
            int length = _socket.Receive(buffer);
            Debug.Log(string.Format("接收服务器{0},消息:{1}", _socket.RemoteEndPoint.ToString(), Encoding.UTF8.GetString(buffer, 0, length)));
            //6.0 像服务器发送消息
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForSeconds(2);
                string sendMessage = string.Format("客户端发送的消息,当前时间{0}", DateTime.Now.ToString());
                _socket.Send(Encoding.UTF8.GetBytes(sendMessage));
                Debug.Log(string.Format("像服务发送的消息:{0}", sendMessage));
            }
        //}
        //catch (Exception ex)
        //{
        //    _socket.Shutdown(SocketShutdown.Both);
        //    _socket.Close();
        //    Console.WriteLine(ex.Message);
        //}
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
