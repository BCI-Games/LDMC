using UnityEngine;
using System.Text;
using System;

[ExecuteAlways]
public class UTF8SocketHost: SocketHost
{
    public event Action<string> MessageReceived;

    public bool LogIncomingData;
    

    public void SendString(string message)
    {
        Send(Encoding.UTF8.GetBytes(message));
    }

    protected override void OnDataReceived(byte[] data)
    {
        string message = Encoding.UTF8.GetString(data);
        if (LogIncomingData)
        {
            Debug.Log("Socket message received: " + message);
        }
        MessageReceived?.Invoke(message);
    }
}
