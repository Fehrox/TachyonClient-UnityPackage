using TachyonClientIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TachyonIO {
    
     public class TachyonUnityClientConnection : MonoBehaviour, IClient {

        public event RecievedEvent OnRecieved;

        public event ConnectionEvent OnDisconnected;
        public event ConnectionEvent OnConnected;
        public event FailedConnectionEvent OnFailedToConnect;

        private bool 
            _disconnectTriggered,
            _connectedTriggered;
        private ConnectionFailedException _connectionFailedException;
        
        Client _client;

        Queue<byte[]> _recievedQueue = new Queue<byte[]>(); 

        IEnumerator Start() {
            while (true) {

                if (_disconnectTriggered) {
                    OnDisconnected?.Invoke();
                    _disconnectTriggered = false;
                }

                if (_connectedTriggered) {
                    OnConnected?.Invoke();
                    _connectedTriggered = false;
                }

                if (_connectionFailedException != null) {
                    OnFailedToConnect?.Invoke(_connectionFailedException);
                    _connectionFailedException = null;
                }
                
                while (_recievedQueue.Count > 0) {
                    var nextReieved = _recievedQueue.Dequeue();
                    OnRecieved?.Invoke(nextReieved);
                }

                yield return null;
            }


        }

        public void Connect(string host, int port) {

            _client = new Client();

            _client.OnConnected += () => 
                _connectedTriggered = true;
            _client.OnDisconnected += () => 
                _disconnectTriggered = true;
            _client.OnFailedToConnect += (ex) =>
                _connectionFailedException = ex;
            _client.OnRecieved += (message) => 
                _recievedQueue.Enqueue(message);

            UnityEngine.Debug.Log("Connecting");
            _client.Connect(host, port);

        }

        public void Send(byte[] data) {
            _client.Send(data);
        }
        
    }
   
}