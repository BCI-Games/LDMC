using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BCI2000RemoteNET;
using UnityEngine;
using System.Threading;
using UnityEngine.tvOS;

namespace BCI2000 
{
    using static BCI2000Remote;
    using static BCI2000Remote.SystemState;
    public class BCI2000Controller: MonoBehaviour
    {
        [SerializeField] private bool _autoConnect = true;
        [SerializeField] private bool _startWhenConnected = true;
        [SerializeField] private bool _stopWithScene = true;
        [SerializeField] private bool _autoDisconnect = true;
        

        [Header("Operator")]
        [SerializeField] private bool _startLocalOperator = true;
        [SerializeField] private string _operatorPath;
        [SerializeField] private string _operatorAddress = "127.0.0.1";
        [SerializeField] private int _operatorPort = 3999;


        [Header("Modules")]
        [SerializeField] private bool _startModulesWithConnection = true;
        [SerializeField] private BCI2000Module[] _startupModules
        = new BCI2000Module[] {
            new("SignalGenerator"),
            new("DummySignalProcessing"),
            new("DummyApplication")
        };


        [Header("Events")]
        [SerializeField] private BCI2000EventDefinition[] _events;

        [Header("Parameters")]
        [SerializeField] private BCI2000ParameterDefinition[] _parameters;
        [SerializeField] private string[] _parameterFiles;

        [Header("States")]
        [SerializeField] private BCI2000StateDefinition[] _states;


        private BCI2000Remote _remote;


        void Awake()
        {
            if (!_autoConnect) return;

            if (_startLocalOperator)
                StartAndConnectToLocalOperator(_operatorPath);
            else
                ConnectToOperator(_operatorPort, _operatorAddress);
        }

        void OnDestroy()
        {
            if (_stopWithScene)
                _remote?.Stop();

            if (_autoDisconnect)
                WrapBlockingMethod(() => _remote?.connection.Quit());
        }


        public void StartAndConnectToLocalOperator
        (
            string operatorPath, int port = 3999
        )
        {
            BCI2000Connection connection = new();
            _remote = new(connection);
            if (!File.Exists(_operatorPath))
            {
                Debug.LogWarning("Operator path invalid, aborting...");
                return;
            }
            connection.StartOperator(operatorPath, port: port);
            ConnectToOperator(port, existingConnection: connection);
        }

        public void ConnectToOperator
        (
            int port = 3999, string address = "127.0.0.1",
            BCI2000Connection existingConnection = null
        )
        {
            BCI2000Connection connection = existingConnection ?? new();
            connection.Connect(address, port);
            _remote = new(connection);

            AddEvents(_events);
            AddStates(_states);
            AddParameters(_parameters);

            if (_startModulesWithConnection)
            {
                StartModules(_startupModules);
                StartCoroutine(
                    RunAwaitSystemStates
                    (
                        new[] {Connected, Initialization},
                        CompleteInitialization
                    )
                );
            }
            else
            {
                CompleteInitialization();
            }
        }

        public void CloseRemoteOperator()
        => WrapBlockingMethod(() => _remote?.connection.Quit());


        protected void CompleteInitialization()
        {
            LoadParameterFiles(_parameterFiles);
            if (_startWhenConnected)
                _remote.Start();
        }


        public void StartModules(BCI2000Module[] modules)
        {
            Dictionary<string, IEnumerable<string>> moduleDictionary = new();
            foreach(var (name, arguments) in modules)
                moduleDictionary.Add(name, arguments);
            _remote.StartupModules(moduleDictionary);
        }

        public void AddEvents(BCI2000EventDefinition[] events)
        {
            foreach(var (name, bitWidth, initialValue) in events)
                _remote.AddEvent(name, bitWidth, initialValue);
        }

        public void AddStates(BCI2000StateDefinition[] states)
        {
            foreach(var (name, bitWidth, initialValue) in states)
                _remote.AddEvent(name, bitWidth, initialValue);
        }

        public void AddParameters(BCI2000ParameterDefinition[] parameters)
        {
            foreach
            (
                var (
                    section, name, defaultValue,
                    minimumValue, maximumValue
                ) in parameters
            )
            {
                _remote.AddParameter
                (
                    section, name, defaultValue,
                    minimumValue, maximumValue
                );
            }
        }

        public void LoadParameterFiles(string[] filePaths)
        {
            foreach(string path in filePaths)
            {
                if (File.Exists(path))
                    _remote.LoadParameters(path);
                else
                    Debug.LogWarning("Parameter file not found: "  +path);
            }
        }


        protected IEnumerator RunAwaitSystemStates
        (
            SystemState[] states, Action callback,
            float pollingPeriod = 0.1f
        )
        {
            while(_remote.WaitForSystemState(states, 0))
            {
                yield return new WaitForSeconds(pollingPeriod);
            }
            callback();
        }


        protected void WrapBlockingMethod(ThreadStart blockingMethod)
        => new Thread(blockingMethod).Start();
    }
}