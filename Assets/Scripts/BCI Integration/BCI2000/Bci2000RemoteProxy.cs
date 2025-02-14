

using System;
using System.IO;
using UnityEngine;
using BCI2000;

namespace BCI2000
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

        protected BCI2000Remote Remote
        {
            get {
                if(_connection is not null && !_connection.Connected())
                {
                    Debug.LogWarning("Connection invalidated");
                    _remote = null;
                    _connection = null;
                }
                return _remote;
            }
        }
        protected BCI2000Remote _remote;
        protected BCI2000Connection _connection;


        void Awake()
        {
            if (_autoConnect)
                ConnectOnceAvailable(OperatorPort, OperatorAddress);
        }

        void OnDestroy()
        {
            if(_stopWithScene)
                Remote?.Stop();
        }


        public uint GetState(string name)
        => Remote?.GetState(name) ?? 0;
        public void SetState(string name, uint value)
        => Remote?.SetState(name, value);

        public uint GetEvent(string name)
        => Remote?.GetEvent(name) ?? 0;
        public void SetEvent(string name, uint value)
        => Remote?.SetEvent(name, value);

        public string GetParameter(string name)
        => Remote?.GetParameter(name) ?? "";
        public void SetParameter(string name, string value)
        => Remote?.SetParameter(name, value);


        public void Connect
        (
            int port = 3999, string address = "127.0.0.1"
        )
        {
            _connection = new();
            _connection.Timeout = 200;
            _connection.Connect(address, port);
            _remote = new(_connection);

            InitializeRemote();
            this.ExecuteWhen(
                () => Remote.GetSystemState() >= Initialization,
                OnConnected
            );
        }

        protected virtual void InitializeRemote() {}
        protected virtual void OnConnected()
        {
            Array.ForEach(_parameterFiles, LoadParameterFile);
            if (_startWhenConnected)
                Remote.Start();
        }


        public void LoadParameterFile(string path)
        {
            if (File.Exists(path))
                Remote.LoadParameters(path);
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