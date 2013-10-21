using UnityEngine;
using System.Collections;

public class GenerateTerrainScript : MonoBehaviour {

    #region Variables
    [SerializeField]
    private int _terrainSize = 15;
    public int TerrainSize {
        get { return _terrainSize; }
        set { _terrainSize = value; }
    }

    [SerializeField]
    private int _nbPlayers = 2;
    public int NbPlayers {
        get { return _nbPlayers; }
        set { _nbPlayers = value; }
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

    ArrayList _boxPlace = new ArrayList();
    public ArrayList BoxPlace {
        get { return _boxPlace; }
        set { _boxPlace = value; }
    }
    #endregion

	// Use this for initialization
	void Start () {
        int height;
        int width;
        int xSC = 0;
        int ySC = 1;
        bool startPlace = false;
        int[] startCase = new int[] { };
        CreateLight(0,5,0);
        //Switch to determine special square
        switch(NbPlayers){
            case 2:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainSize-2,TerrainSize-2,TerrainSize-3,TerrainSize-2,TerrainSize-2,TerrainSize-3};
                PlacePlayer(1, 1, 1);
                PlacePlayer(TerrainSize - 2, TerrainSize - 2, 2);
                break;
            case 3:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainSize-2,TerrainSize-2,TerrainSize-3,TerrainSize-2,TerrainSize-2,TerrainSize-3,
                                       TerrainSize-2,0,TerrainSize-3,0,TerrainSize-2,1};
                PlacePlayer(1, 1, 1);
                PlacePlayer(TerrainSize - 2, TerrainSize - 2, 2);
                PlacePlayer(TerrainSize - 2, 0, 3);
                break;
            case 4:
                startCase = new int[]{1,1,1,2,2,1,
                                       TerrainSize-2,TerrainSize-2,TerrainSize-3,TerrainSize-2,TerrainSize-2,TerrainSize-3,
                                       TerrainSize-2,0,TerrainSize-3,0,TerrainSize-2,1,
                                       0,TerrainSize-2,0,TerrainSize-3,1,TerrainSize-2};
                PlacePlayer(1, 1, 1);
                PlacePlayer(TerrainSize - 2, TerrainSize - 2, 2);
                PlacePlayer(TerrainSize - 2, 0, 3);
                PlacePlayer(0, TerrainSize - 2, 4);
                break;
        }

        for (height = 0; height < TerrainSize; height++ ) {
            for (width = 0; width < TerrainSize; width++) {
                //do/while to determine if we are on a special square
                do {
                    if ((int)startCase.GetValue(xSC) == width && (int)startCase.GetValue(ySC) == height) {
                        startPlace = true;
                        break;
                    }
                    ySC += 2;
                    xSC += 2;
                }while(ySC < startCase.Length);
                xSC = 0;
                ySC = 1;
                if (height == 0 || width == 0 || height == TerrainSize - 1 || width == TerrainSize - 1 || height % 2 == 0 && width % 2 == 0) {
                    CreateWall(width, height);
                }
                //Use random to place or not box
                else if (!startPlace && Random.Range(-5,10)>=0) {
                    CreateBox(width, height);
                }
                startPlace = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateBox(int x, int y) {
        //Create Box
        string positionBox = x +","+ y;
        BoxPlace.Add(positionBox);
        var box = Instantiate(Box, new Vector3(x, 1, y), Quaternion.identity);
        box.name = "Box "+x+","+y;
        
    }

    void CreateWall(int x, int y) {
        //Create Wall
        var wall = Instantiate(Wall, new Vector3(x, 1, y), Quaternion.identity);
        wall.name = "Wall " + x + "," + y;
    }

    void CreateLight(int x, int y, int z) {
        //Create light
        GameObject lightGameObject = new GameObject("The Light");
        lightGameObject.AddComponent<Light>();
        lightGameObject.light.color = Color.white;
        lightGameObject.light.intensity = 4;
        lightGameObject.transform.position = new Vector3(TerrainSize / 2, 6, TerrainSize / 2);
    }

    void PlacePlayer(int x, int y, int number) {
        //Place Player
        var player = Instantiate(Player, new Vector3(x, 1, y), Quaternion.identity);
        player.name = "Player" + number;
    }
}
