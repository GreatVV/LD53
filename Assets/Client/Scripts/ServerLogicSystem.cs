using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using Leopotam.Ecs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LD52
{
    internal class ServerLogicSystem : IEcsRunSystem, INetworkRunnerCallbacks, IEcsInitSystem
    {
        private RuntimeData _runtimeData;
        private StaticData _staticData;
        private SceneData _sceneData;
        
        public void Init()
        {
            var runner = Object.Instantiate(_staticData.RunnerPrefab);
            runner.AddCallbacks(this);
            
            var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
            if (sceneManager == null) {
                Debug.Log($"NetworkRunner does not have any component implementing {nameof(INetworkSceneManager)} interface, adding {nameof(NetworkSceneManagerDefault)}.", runner);
                sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            }

            StartGameArgs gameArgs = new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                Scene = 0,
                SceneManager = sceneManager,
                Initialized = OnInitialized
            };
            runner.StartGame(gameArgs);
            _runtimeData.Runner = runner;
        }

        private void OnInitialized(NetworkRunner runner)
        {
            
        }

        public void Run()
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                runner.Spawn(_staticData.Player, inputAuthority:player, onBeforeSpawned: (r, obj) =>
                {
                    var character = obj.GetComponent<Character>();
                    if(runner.LocalPlayer == player)
                    {
                        _sceneData.CameraFollow.Target = obj.transform;
                        _runtimeData.PlayerCharacter = character;
                    }
                    
                    r.SetPlayerObject(player, obj);
                });
                
               
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (runner.IsServer)
            {
                runner.Despawn(runner.GetPlayerObject(player));
            }
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public async void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            Debug.Log("Start host migration");

            await runner.Shutdown(shutdownReason: ShutdownReason.HostMigration);

            _runtimeData.Runner = default;

            Object.Destroy(runner.gameObject);

            var newRunner = Object.Instantiate(_staticData.RunnerPrefab);

            // setup the new runner...

            // Start the new Runner using the "HostMigrationToken" and pass a callback ref in "HostMigrationResume".
            StartGameResult result = await newRunner.StartGame(new StartGameArgs()
            {
                // SessionName = SessionName,              // ignored, peer never disconnects from the Photon Cloud
                // GameMode = gameMode,                    // ignored, Game Mode comes with the HostMigrationToken
                HostMigrationToken = hostMigrationToken, // contains all necessary info to restart the Runner
                HostMigrationResume = HostMigrationResume, // this will be invoked to resume the simulation
                // other args
            });

            // Check StartGameResult as usual
            if (result.Ok == false)
            {
                Debug.LogError("Host migration fail:"+ result.ShutdownReason);
            }
            else
            {
                Debug.Log("Done");
            }
        }

        private void HostMigrationResume(NetworkRunner runner)
        {
            Log.Debug($"Resume Simulation on new Runner");
            
            foreach (var resumeNO in runner.GetResumeSnapshotNetworkObjects())
            {
                runner.Spawn(resumeNO, inputAuthority: resumeNO.InputAuthority, syncPhysics:false, onBeforeSpawned: (r, newNO) => 
                { 
                 newNO.CopyStateFrom(resumeNO); 
                 r.SetPlayerObject(resumeNO.InputAuthority, newNO);
                });
            }
            
            foreach (var resumeNO in runner.GetResumeSnapshotNetworkSceneObjects()) {

                var sceneObject = resumeNO.Item1; // Reference to local Scene Object
                var objectState = resumeNO.Item2; // Reference to Scene Object State from old Host

                // Copy data back to Scene Object
                sceneObject.CopyStateFromSceneObject(objectState);

                Log.Debug($"Resume SceneObject: {sceneObject.name}");
            }

            Log.Debug("Resume Simulation DONE");
        }


        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

       
    }
}