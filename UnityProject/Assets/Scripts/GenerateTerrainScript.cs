﻿using UnityEngine;
using System.Collections;

public class GenerateTerrainScript : MonoBehaviour {

    #region Variables
    [SerializeField]
    private int _terrainSize = 15;
    public int TerrainSize {
        get { return _terrainSize; }
        set { _terrainSize = value; }
    }

    private int _nbPlayersHuman = 2;
    public int NbPlayersHuman {
        get { return _nbPlayersHuman; }
        set { _nbPlayersHuman = value; }
    }

    private int _nbPlayersIA = 2;
    public int NbPlayersIA {
        get { return _nbPlayersIA; }
        set { _nbPlayersIA = value; }
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
    }

    [RPC]
    private void Generate(int nbPlayerHuman, int nbPlayerTot) {
        NbPlayersHuman = nbPlayerHuman;
        NbPlayersIA = nbPlayerTot - NbPlayersHuman;
        MyNetworkView.RPC("CreateLight", RPCMode.Server, 0, 5, 0);
        MyNetworkView.RPC("PlacePlayer", RPCMode.Server);
        MyNetworkView.RPC("PlaceCOM", RPCMode.Server, nbPlayerHuman, nbPlayerTot - nbPlayerHuman);
        //Switch to determine special square
        switch (nbPlayerTot) {
            case 1:
                startCase = new int[] { 1, 1, 1, 2, 2, 1 };
                MyNetworkView.RPC("PlayerState", RPCMode.All, 1);
                break;
            case 2:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainSize-2,TerrainSize-2,TerrainSize-3,TerrainSize-2,TerrainSize-2,TerrainSize-3};
                MyNetworkView.RPC("PlayerState", RPCMode.All, 2);
                break;
            case 3:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainSize-2,TerrainSize-2,TerrainSize-3,TerrainSize-2,TerrainSize-2,TerrainSize-3,
                                       TerrainSize-2,1,TerrainSize-3,1,TerrainSize-2,2};
                MyNetworkView.RPC("PlayerState", RPCMode.All, 3);
                break;
            case 4:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainSize-2,TerrainSize-2,TerrainSize-3,TerrainSize-2,TerrainSize-2,TerrainSize-3,
                                       TerrainSize-2,1,TerrainSize-3,1,TerrainSize-2,2,
                                       1,TerrainSize-2,1,TerrainSize-3,2,TerrainSize-2};
                MyNetworkView.RPC("PlayerState", RPCMode.All, 4);
                break;
        }
        for (height = 0; height < TerrainSize; height++) {
            for (width = 0; width < TerrainSize; width++) {
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
                if (height == 0 || width == 0 || height == TerrainSize - 1 || width == TerrainSize - 1 || height % 2 == 0 && width % 2 == 0) {
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
            lightGameObject.transform.position = new Vector3(TerrainSize / 2, 6, TerrainSize / 2);
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
                Network.Instantiate(Player, new Vector3(TerrainSize - 2, 1, TerrainSize - 2), Quaternion.identity, 0);
                break;
            case 3:
                Network.Instantiate(Player, new Vector3(TerrainSize - 2, 1, 1), Quaternion.identity, 0);
                break;
            case 4:
                Network.Instantiate(Player, new Vector3(1, 1, TerrainSize - 2), Quaternion.identity, 0);
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
                    Network.Instantiate(COM, new Vector3(TerrainSize - 2, 1, TerrainSize - 2), Quaternion.identity, 0);
                }
                else if (humans == 2) {
                    Network.Instantiate(COM, new Vector3(TerrainSize - 2, 1, 1), Quaternion.identity, 0);
                }
                else if (humans == 3) {
                    Network.Instantiate(COM, new Vector3(1, 1, TerrainSize - 2), Quaternion.identity, 0);
                }
                break;
            case 2:
                if (humans == 1) {
                    Network.Instantiate(COM, new Vector3(TerrainSize - 2, 1, TerrainSize - 2), Quaternion.identity, 0);
                    Network.Instantiate(COM, new Vector3(TerrainSize - 2, 1, 1), Quaternion.identity, 0);
                }
                else if (humans == 2) {
                    Network.Instantiate(COM, new Vector3(1, 1, TerrainSize - 2), Quaternion.identity, 0);
                    Network.Instantiate(COM, new Vector3(TerrainSize - 2, 1, 1), Quaternion.identity, 0);
                }
                break;
            case 3:
                Network.Instantiate(COM, new Vector3(1, 1, TerrainSize - 2), Quaternion.identity, 0);
                Network.Instantiate(COM, new Vector3(TerrainSize - 2, 1, 1), Quaternion.identity, 0);
                Network.Instantiate(COM, new Vector3(TerrainSize - 2, 1, TerrainSize - 2), Quaternion.identity,0);
                break;
        }
    }
}
