using UnityEngine;
using System.Collections;
using System.IO;

public class NetworkScript : MonoBehaviour {

    public string Ip = "127.0.0.1";
    public string Port = "6600";
    private bool serv = false;
    private bool client = false;
    private bool option = false;
    private bool load = false;

    private float hSliderValue = 0.5F;
    public float HSliderValue {
        get { return hSliderValue; }
        set { hSliderValue = value; }
    }

    private int nbPlayer = 1;
    public int NbPlayer {
        get { return nbPlayer; }
        set { nbPlayer = value; }
    }

    private int temp = 1;
    private string playerField = "1";

    void OnStart() {
        audio.Play();
    }

    void OnGUI() {
        Application.runInBackground = true;
        audio.volume = hSliderValue;
        if (Network.peerType == NetworkPeerType.Disconnected && client == false && serv == false && option == false && load == false) {
            GUI.Box(new Rect(0, 0, 200, 150), "Main Menu");
            if (GUI.Button(new Rect(40, 30, 110, 25), "Start Client")) {
                client = true;
            }
            if (GUI.Button(new Rect(40, 60, 110, 25), "Start Server")) {
                serv = true;
            }
            if (GUI.Button(new Rect(40, 90, 110, 25), "Options")) {
                option = true;
            }
            if (GUI.Button(new Rect(40, 120, 110, 25), "Exit")) {
                Application.Quit();
            }
        }
        else if (Network.peerType == NetworkPeerType.Disconnected && client == false && serv == true && option == false && load == false) {
            GUI.Box(new Rect(0, 0, 200, 120), "Server Menu");
            Port = GUI.TextArea(new Rect(30, 30, 110, 20), Port);
            if (GUI.Button(new Rect(30, 60, 110, 25), "Initialize Server")) {
                Network.InitializeSecurity();
                Network.InitializeServer(4, int.Parse(Port), true);
            }
            if (GUI.Button(new Rect(30, 90, 110, 25), "Back")) {
                serv = false;
            }
        }
        else if (Network.peerType == NetworkPeerType.Disconnected && client == true && serv == false && option == false && load == false) {
            GUI.Box(new Rect(0, 0, 200, 160), "Login Menu");
            Port = GUI.TextArea(new Rect(30, 30, 110, 20), Port);
            Ip = GUI.TextArea(new Rect(30, 60, 110, 20), Ip);
            if (GUI.Button(new Rect(30, 90, 110, 25), "Initialize Client")) {
                Network.Connect(Ip, int.Parse(Port));
            }
            if (GUI.Button(new Rect(30, 120, 110, 25), "Back")) {
                client = false;
            }
        }
        else if (Network.peerType == NetworkPeerType.Disconnected && client == false && serv == false && option == true && load == false) {
            GUI.Box(new Rect(0, 0, 200, 90), "Volume");
            GUI.Label(new Rect(25, 25, 100, 25), "Music");
            hSliderValue = GUI.HorizontalSlider(new Rect(30, 45, 100, 30), hSliderValue, 0.0F, 1.0F);
            audio.volume = hSliderValue;
            if (GUI.Button(new Rect(30, 60, 110, 25), "Back")) {
                option = false;
            }
        }
        else {
            serv = false;
            client = false;
            if (Network.peerType == NetworkPeerType.Client) {
                GUI.Box(new Rect(0, 0, 200, 100), "Client Menu");
                GUI.Label(new Rect(30, 30, 100, 25), "Client");
                GUI.Label(new Rect(30, 50, 100, 25), Network.player.ToString());
                if (GUI.Button(new Rect(30, 70, 110, 25), "Logout")) {
                    Network.Disconnect(250);
                    Application.LoadLevel(0);
                    audio.volume = hSliderValue;
                }
            }
            if (Network.peerType == NetworkPeerType.Server && load == false) {
                GUI.Box(new Rect(0, 0, 200, 220), "Server Menu");
                GUI.Label(new Rect(30, 30, 100, 25), "Server");
                GUI.Label(new Rect(30, 55, 100, 25), "Connections: " + Network.connections.Length);

                if (GUI.Button(new Rect(30, 75, 100, 25), "Logout")) {
                    Network.Disconnect(250);
                    Application.LoadLevel(0);
                    audio.volume = hSliderValue;
                }
                if (GUI.Button(new Rect(30, 110, 100, 25), "New Game")) {
                    GameObject origin = GameObject.Find("Origin");
                    NbPlayer = Mathf.Clamp(temp, 1, 4);
                    GenerateTerrainScript generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
                    generateTerrainScript.MyNetworkView.RPC("Generate", RPCMode.Server, Network.connections.Length, NbPlayer);

                }
                if (GUI.Button(new Rect(30, 140, 100, 25), "Load")) {
                    load = true;
                }
                GUI.Label(new Rect(30, 170, 130, 25), "Number of players");
                playerField = GUI.TextArea(new Rect(30, 190, 100, 20), playerField);
                if (int.TryParse(playerField, out temp)) {
                    NbPlayer = temp;
                }
            }
            else if (Network.peerType == NetworkPeerType.Server && load == true) {
                if (GUI.Button(new Rect(30, 45, 110, 25), "Back")) {
                    load = false;
                }
                var info = new DirectoryInfo(Application.dataPath);
                var fileInfo = info.GetFiles();
                ArrayList levelTab = new ArrayList();
                for (int file = 0; file < fileInfo.Length; file++) {
                    if (fileInfo[file].Extension.ToString().Equals(".txt")) {
                        levelTab.Add(Path.GetFileName(fileInfo[file].ToString()));
                    }
                }
                GUI.Box(new Rect(0, 0, 150, 50 + 50 * levelTab.Count), "Niveaux");

                for (int i = 0; i < levelTab.Count; i++) {
                    if (GUI.Button(new Rect(30, 80 + i * 30, 110, 25), levelTab[i].ToString())) {
                        GameObject origin = GameObject.Find("Origin");
                        NbPlayer = Mathf.Clamp(temp, 1, 4);
                        GenerateTerrainScript generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
                        generateTerrainScript.MyNetworkView.RPC("Load", RPCMode.Server, Network.connections.Length, NbPlayer, levelTab[i].ToString());
                    }
                }
            }
        }
    }
}