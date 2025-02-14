

using System;
using System.IO;
using UnityEngine;
using BCI2000RemoteNET;

namespace Bci2000
{
    using static MethodExtensions;
    using static BCI2000Remote.SystemState;

    public class Bci2000RemoteProxy: MonoBehaviour
    {
        [SerializeField] private bool _autoConnect = true;
        [SerializeField] private bool _startWhenConnected = true;
        [SerializeField] private bool _stopWithScene = true;

        [Header("Operator")]
        public string OperatorAddress = "127.0.0.1";
        public int OperatorPort = 3999;

        [Header("Parameters")]
        [SerializeField] private string[] _parameterFiles;

        protected BCI2000Remote _remote;


        void Awake()
        {
            if (_autoConnect)
                ConnectOnceAvailable(OperatorPort, OperatorAddress);
        }

        void OnDestroy()
        {
            if(_stopWithScene)
                _remote?.Stop();
        }


        public uint GetState(string name)
        => _remote?.GetState(name) ?? 0;
        public void SetState(string name, uint value)
        => _remote?.SetState(name, value);

        public uint GetEvent(string name)
        => _remote?.GetEvent(name) ?? 0;
        public void SetEvent(string name, uint value)
        => _remote?.SetEvent(name, value);

        public string GetParameter(string name)
        => _remote?.GetParameter(name) ?? "";
        public void SetParameter(string name, string value)
        => _remote?.SetParameter(name, value);


        public void Connect
        (
            int port = 3999, string address = "127.0.0.1"
        )
        {
            BCI2000Connection connection = new();
            connection.Connect(address, port);
            _remote = new(connection);

            InitializeRemote();
            this.ExecuteWhen(
                () => _remote.GetSystemState() >= Connected,
                OnConnected
            );
        }

        protected virtual void InitializeRemote() {}
        protected virtual void OnConnected()
        {
            Array.ForEach(_parameterFiles, LoadParameterFile);
            if (_startWhenConnected)
                _remote.Start();
        }


        public void LoadParameterFile(string path)
        {
            if (File.Exists(path))
                _remote.LoadParameters(path);
            else
                Debug.LogWarning("Parameter file not found: " + path);
        }


        public void ConnectOnceAvailable
        (
            int port = 3999, string address = "127.0.0.1"
        )
        => this.ExecuteWhenHostAvailable
        (
            address, port, () => Connect(port, address)
        );


        public void CloseRemote()
        => ExecuteMethodInThread(() => _remote?.connection.Quit());
    }
}