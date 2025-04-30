using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System;

[ExecuteAlways]
public class SocketHost: MonoBehaviour
{
    public event Action<byte[]> DataReceived;
    public event Action SocketOpened;
    public event Action SocketClosed;

    public string Host = "localhost";
    public int Port = 8005;

    public bool IsConnected => _socket != null && _socket.Connected;
    public bool IsReading => _readThread != null && _readThread.IsAlive;

    private Socket _socket;
    private Thread _readThread;
    private readonly Queue<byte[]> _readQueue = new();
    private bool _shouldBeConnected;


    void Update()
    {
        if (_readQueue.Count != 0)
        lock(_readQueue)
        {
            while (_readQueue.Count > 0)
            {
                byte[] data = _readQueue.Dequeue();
                DataReceived?.Invoke(data);
                OnDataReceived(data);
            }
        }

        if (_shouldBeConnected && !IsConnected)
            NotifyConnectionState(false);
    }

    void OnDestroy()
    {
        if (IsConnected) Close();
    }


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

        NotifyConnectionState(true);
    }

    [ContextMenu("Close Socket")]
    public void Close()
    {
        if (!IsConnected)
        {
            Debug.LogWarning("Socket isn't open, ignoring...");
            return;
        }
        if (IsReading) _readThread.Abort();
        _socket?.Close();
        NotifyConnectionState(false);
    }

    public void Send(byte[] buffer)
    {
        if (!IsConnected)
        {
            Debug.LogWarning("Socket is not connected");
            return;
        }
        _socket.Send(buffer);
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


    private void NotifyConnectionState(bool connected)
    {
        _shouldBeConnected = connected;
        (connected? SocketOpened: SocketClosed)?.Invoke();
    }

    protected virtual void OnDataReceived(byte[] data) {}
}
