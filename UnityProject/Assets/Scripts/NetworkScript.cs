using UnityEngine;
using System.Collections;

public class NetworkScript : MonoBehaviour {

    public string Ip = "127.0.0.1";
    public int Port = 6600;
    private int playerCount = 0;

    void OnGUI() {
        Application.runInBackground = true;
        if (Network.peerType == NetworkPeerType.Disconnected) {
            if (GUI.Button(new Rect(30, 30, 100, 25), "Start Client")) {
                Network.Connect(Ip, Port);
            }
            if (GUI.Button(new Rect(30, 55, 100, 25), "Start Server")) {
                Network.InitializeSecurity();
                Network.InitializeServer(4, Port, true);
            }
        }
        else {
            if (Network.peerType == NetworkPeerType.Client) {
                GUI.Label(new Rect(30, 30, 100, 25), "Client");
                GUI.Label(new Rect(30, 100, 100, 25), Network.player.ToString());
                if (GUI.Button(new Rect(30, 50, 110, 25), "Logout")) {
                    Network.Disconnect(250);
                }
            }
            if (Network.peerType == NetworkPeerType.Server) {
                GUI.Label(new Rect(30, 30, 100, 25), "Server");
                GUI.Label(new Rect(30, 55, 100, 25), "Connections: " + Network.connections.Length);

                if (GUI.Button(new Rect(30, 75, 100, 25), "Logout")) {
                    Network.Disconnect(250);
                }
                if (GUI.Button(new Rect(30, 100, 100, 25), "New Game")) {
                    Application.LoadLevel(0);
                    GameObject origin = GameObject.Find("Origin");
                    GenerateTerrainScript generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
                    generateTerrainScript.MyNetworkView.RPC("Generate", RPCMode.Server, Network.connections.Length);

                }
            }
        }
    }
}
