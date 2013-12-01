using UnityEngine;
using System.Collections;
using System.IO;

public class GenerateTerrainScript : MonoBehaviour {

    #region Variables

    [SerializeField]
    private GameObject[] portals;
    public GameObject[] Portals {
        get { return portals; }
        set { portals = value; }
    }

    [SerializeField]
    private int _terrainX = 15;
    public int TerrainX {
        get { return _terrainX; }
        set { _terrainX = value; }
    }

    [SerializeField]
    private int _terrainZ = 15;
    public int TerrainZ {
        get { return _terrainZ; }
        set { _terrainZ = value; }
    }

    [SerializeField]
    private GameObject _wall;
    public GameObject Wall {
        get { return _wall; }
        set { _wall = value; }
    }

    [SerializeField]
    private GameObject _box;
    public GameObject Box {
        get { return _box; }
        set { _box = value; }
    }

    [SerializeField]
    private GameObject _portal;
    public GameObject Portal {
        get { return _portal; }
        set { _portal = value; }
    }

    [SerializeField]
    private GameObject _player;
    public GameObject Player {
        get { return _player; }
        set { _player = value; }
    }

    [SerializeField]
    private GameObject _com;
    public GameObject COM {
        get { return _com; }
        set { _com = value; }
    }

    ArrayList _boxPlace = new ArrayList();
    public ArrayList BoxPlace {
        get { return _boxPlace; }
        set { _boxPlace = value; }
    }

    ArrayList _bombPlace = new ArrayList();
    public ArrayList BombPlace {
        get { return _bombPlace; }
        set { _bombPlace = value; }
    }

    private bool player1Alive = false;
    public bool Player1Alive {
        get { return player1Alive; }
        set { player1Alive = value; }
    }

    private bool player2Alive = false;
    public bool Player2Alive {
        get { return player2Alive; }
        set { player2Alive = value; }
    }

    private bool player3Alive = false;
    public bool Player3Alive {
        get { return player3Alive; }
        set { player3Alive = value; }
    }

    private bool player4Alive = false;
    public bool Player4Alive {
        get { return player4Alive; }
        set { player4Alive = value; }
    }

    private bool com1Alive = false;
    public bool Com1Alive {
        get { return com1Alive; }
        set { com1Alive = value; }
    }

    private bool com2Alive = false;
    public bool Com2Alive {
        get { return com2Alive; }
        set { com2Alive = value; }
    }

    private bool com3Alive = false;
    public bool Com3Alive {
        get { return com3Alive; }
        set { com3Alive = value; }
    }

    private int height;
    private int width;

    private int xSC = 0;
    private int ySC = 1;

    private bool startPlace = false;
    private int[] startCase = new int[] { };

    private NetworkView _myNetworkView = null;
    public NetworkView MyNetworkView {
        get { return _myNetworkView; }
        set { _myNetworkView = value; }
    }
    #endregion

    // Use this for initialization
    void Start() {
        MyNetworkView = this.gameObject.GetComponent<NetworkView>();
        for (int i = 0; i < 8; i++) {
            Portals[i] = (GameObject)Instantiate(Portal, new Vector3(0, -2, 0), Quaternion.identity);
            Portals[i].SetActive(false);
        }
    }

    [RPC]
    private void Load(int nbPlayerHuman, int nbPlayerTot, string level) {
        switch (nbPlayerHuman) {
            case 1:
                MyNetworkView.RPC("PlayerState", RPCMode.All, 1);
                break;
            case 2:
                MyNetworkView.RPC("PlayerState", RPCMode.All, 2);
                break;
            case 3:
                MyNetworkView.RPC("PlayerState", RPCMode.All, 3);
                break;
            case 4:
                MyNetworkView.RPC("PlayerState", RPCMode.All, 4);
                break;
        }
        switch (nbPlayerTot - nbPlayerHuman) {
            case 1:
                MyNetworkView.RPC("ComState", RPCMode.All, 1);
                break;
            case 2:
                MyNetworkView.RPC("ComState", RPCMode.All, 2);
                break;
            case 3:
                MyNetworkView.RPC("ComState", RPCMode.All, 3);
                break;
        }
        MyNetworkView.RPC("Read", RPCMode.Server, nbPlayerHuman, nbPlayerTot - nbPlayerHuman, level);
    }

    [RPC]
    private void Generate(int nbPlayerHuman, int nbPlayerTot) {
        MyNetworkView.RPC("CreateLight", RPCMode.Server, TerrainX / 2, 6, TerrainZ / 2);
        MyNetworkView.RPC("PlacePlayer", RPCMode.Server);
        MyNetworkView.RPC("PlaceCOM", RPCMode.Server, nbPlayerHuman, nbPlayerTot - nbPlayerHuman);
        //Switch to determine special square
        switch (nbPlayerHuman) {
            case 1:
                startCase = new int[] { 1, 1, 1, 2, 2, 1 };
                MyNetworkView.RPC("PlayerState", RPCMode.All, 1);
                break;
            case 2:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainX-2,TerrainZ-2,TerrainX-3,TerrainZ-2,TerrainX-2,TerrainZ-3};
                MyNetworkView.RPC("PlayerState", RPCMode.All, 2);
                break;
            case 3:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainX-2,TerrainZ-2,TerrainX-3,TerrainZ-2,TerrainX-2,TerrainZ-3,
                                       TerrainX-2,1,TerrainZ-3,1,TerrainX-2,2};
                MyNetworkView.RPC("PlayerState", RPCMode.All, 3);
                break;
            case 4:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainX-2,TerrainZ-2,TerrainX-3,TerrainZ-2,TerrainX-2,TerrainZ-3,
                                       TerrainX-2,1,TerrainX-3,1,TerrainX-2,2,
                                       1,TerrainZ-2,1,TerrainZ-3,2,TerrainZ-2};
                MyNetworkView.RPC("PlayerState", RPCMode.All, 4);
                break;
        }
        switch (nbPlayerTot - nbPlayerHuman) {
            case 1:
                MyNetworkView.RPC("ComState", RPCMode.All, 1);
                break;
            case 2:
                MyNetworkView.RPC("ComState", RPCMode.All, 2);
                break;
            case 3:
                MyNetworkView.RPC("ComState", RPCMode.All, 3);
                break;
        }
        switch (nbPlayerTot) {
            case 1:
                startCase = new int[] { 1, 1, 1, 2, 2, 1 };
                break;
            case 2:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainX-2,TerrainZ-2,TerrainX-3,TerrainZ-2,TerrainX-2,TerrainZ-3};
                break;
            case 3:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainX-2,TerrainZ-2,TerrainX-3,TerrainZ-2,TerrainX-2,TerrainZ-3,
                                       TerrainX-2,1,TerrainZ-3,1,TerrainX-2,2};
                break;
            case 4:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainX-2,TerrainZ-2,TerrainX-3,TerrainZ-2,TerrainX-2,TerrainZ-3,
                                       TerrainX-2,1,TerrainX-3,1,TerrainX-2,2,
                                       1,TerrainZ-2,1,TerrainZ-3,2,TerrainZ-2};
                break;
        }
        for (height = 0; height < TerrainZ; height++) {
            for (width = 0; width < TerrainX; width++) {
                //do/while to determine if we are on a special square
                do {
                    if ((int)startCase.GetValue(xSC) == width && (int)startCase.GetValue(ySC) == height) {
                        startPlace = true;
                        break;
                    }
                    ySC += 2;
                    xSC += 2;
                } while (ySC < startCase.Length);
                xSC = 0;
                ySC = 1;
                if (height == 0 || width == 0 || height == TerrainZ - 1 || width == TerrainX - 1 || height % 2 == 0 && width % 2 == 0) {
                    MyNetworkView.RPC("CreateWall", RPCMode.Server, width, height);
                }
                //Use random to place or not box
                else if (!startPlace && Random.Range(-10, 10) >= 0) {
                    MyNetworkView.RPC("CreateBox", RPCMode.Server, width, height);
                }
                startPlace = false;
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }

    [RPC]
    void PlayerState(int number) {
        if (number == 1) {
            Player1Alive = true;
        }
        else if (number == 2) {
            Player1Alive = true;
            Player2Alive = true;
        }
        else if (number == 3) {
            Player1Alive = true;
            Player2Alive = true;
            Player3Alive = true;
        }
        else {
            Player1Alive = true;
            Player2Alive = true;
            Player3Alive = true;
            Player4Alive = true;
        }
    }

    [RPC]
    void ComState(int number) {
        if (number == 1) {
            Com1Alive = true;
        }
        else if (number == 2) {
            Com1Alive = true;
            Com2Alive = true;
        }
        else {
            Com1Alive = true;
            Com2Alive = true;
            Com3Alive = true;
        }
    }

    [RPC]
    void CreateBox(int x, int y) {
        //Create Box
        if (!GameObject.Find("Box " + x + "," + y)) {
            string positionBox = x + "," + y;
            BoxPlace.Add(positionBox);
            GameObject box = (GameObject)Instantiate(Box, new Vector3(x, 1, y), Quaternion.identity);
            box.name = "Box " + x + "," + y;
            if (Network.isServer) {
                MyNetworkView.RPC("CreateBox", RPCMode.Others, x, y);
            }
        }
    }
    [RPC]
    void CreateWall(int x, int y) {
        //Create Wall
        if (!GameObject.Find("Wall " + x + "," + y)) {
            GameObject wall = (GameObject)Instantiate(Wall, new Vector3(x, 1, y), Quaternion.identity);
            wall.name = "Wall " + x + "," + y;
            if (Network.isServer) {
                MyNetworkView.RPC("CreateWall", RPCMode.Others, x, y);
            }
        }
    }

    [RPC]
    void CreateLight(int x, int y, int z) {
        //Create light
        if (!GameObject.Find("The Light")) {
            GameObject lightGameObject = new GameObject("The Light");
            lightGameObject.AddComponent<Light>();
            lightGameObject.light.color = Color.white;
            lightGameObject.light.intensity = 4;
            lightGameObject.transform.position = new Vector3(x, y, z);
            if (Network.isServer) {
                MyNetworkView.RPC("CreateLight", RPCMode.Others, x, y, z);
            }
        }
    }

    [RPC]
    void PlacePlayer() {
        //Place Player
        switch (int.Parse(Network.player.ToString())) {
            case 1:
                Network.Instantiate(Player, new Vector3(1, 1, 1), Quaternion.identity, 0);
                break;
            case 2:
                Network.Instantiate(Player, new Vector3(TerrainX - 2, 1, TerrainZ - 2), Quaternion.identity, 0);
                break;
            case 3:
                Network.Instantiate(Player, new Vector3(TerrainX - 2, 1, 1), Quaternion.identity, 0);
                break;
            case 4:
                Network.Instantiate(Player, new Vector3(1, 1, TerrainZ - 2), Quaternion.identity, 0);
                break;
        }
        if (Network.isServer) {
            MyNetworkView.RPC("PlacePlayer", RPCMode.Others);
        }
    }

    [RPC]
    void PlaceCOM(int humans, int coms) {
        switch (coms) {
            case 1:
                if (humans == 1) {
                   GameObject com = (GameObject)Network.Instantiate(COM, new Vector3(TerrainX - 2, 1, TerrainZ - 2), Quaternion.identity, 0);
                   com.name = "COM1";
                }
                else if (humans == 2) {
                    GameObject com = (GameObject)Network.Instantiate(COM, new Vector3(TerrainX - 2, 1, 1), Quaternion.identity, 0);
                    com.name = "COM1";
                }
                else if (humans == 3) {
                    GameObject com = (GameObject)Network.Instantiate(COM, new Vector3(1, 1, TerrainZ - 2), Quaternion.identity, 0);
                    com.name = "COM1";
                }
                break;
            case 2:
                if (humans == 1) {
                    GameObject com1 = (GameObject)Network.Instantiate(COM, new Vector3(TerrainX - 2, 1, TerrainZ - 2), Quaternion.identity, 0);
                    com1.name = "COM1";
                    GameObject com2 = (GameObject)Network.Instantiate(COM, new Vector3(TerrainX - 2, 1, 1), Quaternion.identity, 0);
                    com2.name = "COM2";
                }
                else if (humans == 2) {
                    GameObject com1 = (GameObject)Network.Instantiate(COM, new Vector3(1, 1, TerrainZ - 2), Quaternion.identity, 0);
                    com1.name = "COM1";
                    GameObject com2 = (GameObject)Network.Instantiate(COM, new Vector3(TerrainX - 2, 1, 1), Quaternion.identity, 0);
                    com2.name = "COM2";
                }
                break;
            case 3:
                GameObject com11 = (GameObject)Network.Instantiate(COM, new Vector3(1, 1, TerrainZ - 2), Quaternion.identity, 0);
                com11.name = "COM1";
                GameObject com21 = (GameObject)Network.Instantiate(COM, new Vector3(TerrainX - 2, 1, 1), Quaternion.identity, 0);
                com21.name = "COM2";
                GameObject com3 = (GameObject)Network.Instantiate(COM, new Vector3(TerrainX - 2, 1, TerrainZ - 2), Quaternion.identity, 0);
                com3.name = "COM3";
                break;
        }
    }

    [RPC]
    void PlacePlayersCustom(int[] playerPlace) {
        switch (int.Parse(Network.player.ToString())) {
            case 1:
                Network.Instantiate(Player, new Vector3(playerPlace[0], 1, playerPlace[1]), Quaternion.identity, 0);
                break;
            case 2:
                Network.Instantiate(Player, new Vector3(playerPlace[2], 1, playerPlace[3]), Quaternion.identity, 0);
                break;
            case 3:
                Network.Instantiate(Player, new Vector3(playerPlace[4], 1, playerPlace[5]), Quaternion.identity, 0);
                break;
            case 4:
                Network.Instantiate(Player, new Vector3(playerPlace[6], 1, playerPlace[7]), Quaternion.identity, 0);
                break;
        }
        if (Network.isServer) {
            MyNetworkView.RPC("PlacePlayersCustom", RPCMode.Others, playerPlace);
        }

    }

    [RPC]
    void PlaceCOMCustom(int[] playerPlace, int humans, int coms) {
        switch (coms) {
            case 1:
                if (humans == 1) {
                    Network.Instantiate(COM, new Vector3(playerPlace[2], 1, playerPlace[3]), Quaternion.identity, 0);
                }
                else if (humans == 2) {
                    Network.Instantiate(COM, new Vector3(playerPlace[4], 1, playerPlace[5]), Quaternion.identity, 0);
                }
                else if (humans == 3) {
                    Network.Instantiate(COM, new Vector3(playerPlace[6], 1, playerPlace[7]), Quaternion.identity, 0);
                }
                break;
            case 2:
                if (humans == 1) {
                    Network.Instantiate(COM, new Vector3(playerPlace[2], 1, playerPlace[3]), Quaternion.identity, 0);
                    Network.Instantiate(COM, new Vector3(playerPlace[4], 1, playerPlace[5]), Quaternion.identity, 0);
                }
                else if (humans == 2) {
                    Network.Instantiate(COM, new Vector3(playerPlace[4], 1, playerPlace[5]), Quaternion.identity, 0);
                    Network.Instantiate(COM, new Vector3(playerPlace[6], 1, playerPlace[7]), Quaternion.identity, 0);
                }
                break;
            case 3:
                Network.Instantiate(COM, new Vector3(playerPlace[2], 1, playerPlace[3]), Quaternion.identity, 0);
                Network.Instantiate(COM, new Vector3(playerPlace[4], 1, playerPlace[5]), Quaternion.identity, 0);
                Network.Instantiate(COM, new Vector3(playerPlace[6], 1, playerPlace[7]), Quaternion.identity, 0);
                break;
        }
    }

    [RPC]
    void Read(int humans, int coms, string level) {
        MyNetworkView.RPC("CreateLight", RPCMode.Server, TerrainX / 2, 6, TerrainZ / 2);

        var sr = new StreamReader(Application.dataPath + "/" + level);
        var fileContents = sr.ReadToEnd();
        sr.Close();
        int x = 0;
        int y = 0;
        int tmpx = 0;
        int[] playerPlace = { 0, 0, 0, 0, 0, 0, 0, 0 };
        bool getSize = false;
        var lines = fileContents.Split("\n"[0]);
        for (int line = 0; line < lines.Length; line++) {
            if (getSize) {
                for (int curs = 0; curs < lines[line].Length; curs++) {
                    if (lines[line][curs].ToString().Equals("W")) {
                        Debug.Log("Create Wall: " + x + "," + y);
                        MyNetworkView.RPC("CreateWall", RPCMode.All, x, y);
                    }
                    else if (lines[line][curs].ToString().Equals("C")) {
                        Debug.Log("Create Box: " + x + "," + y);
                        MyNetworkView.RPC("CreateBox", RPCMode.All, x, y);
                    }
                    else if (lines[line][curs].ToString().Equals("1")) {
                        Debug.Log("Create Player 1: " + x + "," + y);
                        playerPlace[0] = x;
                        playerPlace[1] = y;
                    }
                    else if (lines[line][curs].ToString().Equals("2")) {
                        Debug.Log("Create Player 2: " + x + "," + y);
                        playerPlace[2] = x;
                        playerPlace[3] = y;
                    }
                    else if (lines[line][curs].ToString().Equals("3")) {
                        Debug.Log("Create Player 3: " + x + "," + y);
                        playerPlace[4] = x;
                        playerPlace[5] = y;
                    }
                    else if (lines[line][curs].ToString().Equals("4")) {
                        Debug.Log("Create Player 4: " + x + "," + y);
                        playerPlace[6] = x;
                        playerPlace[7] = y;
                    }
                    x--;
                }
                x = tmpx;
                y--;
            }
            else {
                string str = "";
                for (var i = 0; i < lines[line].Length; i++) {
                    str = str + lines[line][i].ToString();
                }
                var size = str.Split(","[0]);
                x = int.Parse(size[0]);
                x--;
                y = int.Parse(size[1]);
                y--;
                tmpx = x;
                getSize = true;
            }
        }
        MyNetworkView.RPC("PlacePlayersCustom", RPCMode.Others, playerPlace);
        MyNetworkView.RPC("PlaceCOMCustom", RPCMode.Others, playerPlace, humans, coms);

    }
}
