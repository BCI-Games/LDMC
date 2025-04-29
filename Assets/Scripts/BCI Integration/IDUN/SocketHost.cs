using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System;

public class SocketHost: MonoBehaviour
{
    public event Action<byte[]> DataReceived;

    public string Host = "localhost";
    public int Port = 8005;
    public bool LogIncomingData;

    public bool IsConnected => _socket != null && _socket.Connected;
    public bool IsReading => _readThread != null && _readThread.IsAlive;

    private Socket _socket;
    private Thread _readThread;
    private readonly Queue<byte[]> _readQueue = new();


    void Start()
    {
        if (LogIncomingData)
        {
            DataReceived += (data) =>
            Debug.Log(Encoding.UTF8.GetString(data));
        }
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
            }
        }
    }

    void OnDestroy() => Close();


    [ContextMenu("Open Socket")]
    public void Open()
    => Open(SocketType.Stream, ProtocolType.Tcp);
    public async void Open(SocketType type, ProtocolType protocol)
    {
        if (IsConnected) _socket.Close();
        _socket = new(type, protocol);
        await _socket.ConnectAsync(Host, Port);

        if (IsReading) _readThread.Abort();
        _readThread = new Thread(RunReadData);
        _readThread.Start();
    }

    [ContextMenu("Close Socket")]
    public void Close()
    {
        if (IsReading) _readThread.Abort();
        _socket?.Close();
    }

    public void SendString(string message)
    {
        if (!IsConnected)
        {
            Debug.LogWarning("Socket is not connected");
            return;
        }
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
