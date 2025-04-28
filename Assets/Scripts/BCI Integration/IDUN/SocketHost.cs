using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System;
using System.Linq;

public class SocketHost: MonoBehaviour
{
    public string Host = "localhost";
    public int Port = 8005;

    public event Action<byte[]> DataReceived;

    private readonly Socket _socket = new(SocketType.Stream, ProtocolType.Tcp);
    private Thread _readThread;
    private readonly Queue<byte[]> _readQueue = new();


    void Start()
    {
        Open();
    }

    void Update()
    {
        if (_readQueue.Count != 0)
        lock(_readQueue)
        {
            while (_readQueue.Count > 0)
            {
                byte[] data = _readQueue.Dequeue();
                DataReceived?.Invoke(data);
                Debug.Log(Encoding.UTF8.GetString(data));
            }
        }
    }

    void OnDestroy()
    {
        Close();
    }


    public async void Open()
    {
        await _socket.ConnectAsync(Host, Port);
        _readThread = new Thread(RunReadData);
        _readThread.Start();
    }

    public void Close()
    {
        _readThread?.Abort();
        _socket?.Close();
    }

    public void SendString(string message)
    {
        if (!_socket.Connected) return;
        _socket.Send(Encoding.UTF8.GetBytes(message));
    }

    private void RunReadData()
    {
        while (_socket.Connected)
        {
            byte[] dataBuffer = new byte[256];
            int messageLength = _socket.Receive(dataBuffer);
            if (messageLength == 0) continue;

            lock (_readQueue)
            {
                _readQueue.Enqueue(dataBuffer[..messageLength]);
            }
        }
    }
}