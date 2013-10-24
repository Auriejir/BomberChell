using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    [SerializeField]
    private int _powerBomb = 2;
    public int PowerBomb {
        get { return _powerBomb; }
        set { _powerBomb = value; }
    }

    [SerializeField]
    private float myTimer = 5f;
    public float MyTimer {
        get { return myTimer; }
        set { myTimer = value; }
    }

    bool done = false;
    private NetworkView _myNetworkView = null;
    private ArrayList boxp;

	// Use this for initialization
	void Start () {
        _myNetworkView = this.gameObject.GetComponent<NetworkView>();
        gameObject.collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (myTimer > 0) {
            MyTimer -= Time.deltaTime;
        }
        if (MyTimer <= 0 && !done) {
            print("TimeOut");
            done = true;
            TimeOut();
        }

	}

    void TimeOut() {
        Vector3 pos = this.gameObject.transform.position;

        GameObject origin = GameObject.Find("Origin");
        GameObject player1 = GameObject.Find("Player1");
        GameObject player2 = GameObject.Find("Player2");

        characterScript p1 = player1.GetComponent<characterScript>();
        characterScript p2 = player2.GetComponent<characterScript>();
        GenerateTerrainScript generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();

        boxp = generateTerrainScript.BoxPlace;
        ArrayList PlayerPlace = new ArrayList();

        PlayerPlace.Add(p1.Position[0] + "," + p1.Position[1]);
        PlayerPlace.Add(p2.Position[0] + "," + p2.Position[1]);
        if (generateTerrainScript.NbPlayers == 3) {
            GameObject player3 = GameObject.Find("Player3");
            characterScript p3 = player3.GetComponent<characterScript>();
            PlayerPlace.Add(p3.Position[0] + "," + p3.Position[1]); 
        }
        else if (generateTerrainScript.NbPlayers == 4) {
            GameObject player3 = GameObject.Find("Player3");
            GameObject player4 = GameObject.Find("Player4");
            characterScript p3 = player3.GetComponent<characterScript>();
            characterScript p4 = player4.GetComponent<characterScript>();
            PlayerPlace.Add(p3.Position[0] + "," + p3.Position[1]);
            PlayerPlace.Add(p4.Position[0] + "," + p4.Position[1]); 
        }
                
        bool up = true, down = true, left = true, right = true;
        int tempy;
        int tempx;

        print("Bomb : " + (int)pos.x + "," + (int)pos.z);

        for (int i = 0; i <= PowerBomb; i++) {
            tempx = (int)pos.x + i;
            if (left) {
                print("Right : " + tempx + "," + (int)pos.z);
                if (boxp.Contains(tempx + "," + (int)pos.z)) {
                    //Delete Box
                    DestroyBox(tempx, (int)pos.z);
                    Debug.Log("BoxHit Right");
                    left = false;
                }
                else if (PlayerPlace.Contains(tempx + "," + (int)pos.z)) {
                    //Kill Player
                    Debug.Log("PlayerHit Right");
                    if (PlayerPlace[0] == tempx + "," + (int)pos.z)
                        DestroyPlayer(1);
                    else if (PlayerPlace[1] == tempx + "," + (int)pos.z)
                        DestroyPlayer(2);
                    else if (PlayerPlace[2] == tempx + "," + (int)pos.z)
                        DestroyPlayer(3);
                    else if (PlayerPlace[3] == tempx + "," + (int)pos.z)
                        DestroyPlayer(4);
                }
                else if (tempx % 2 == 0 && (int)pos.z % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Right");
                    left = false;
                }
            }
            tempx = (int)pos.x - i;
            if (right) {
                print("Left : " + tempx + "," + (int)pos.z);
                if (boxp.Contains(tempx + "," + (int)pos.z)) {
                    //Delete Box
                    DestroyBox(tempx, (int)pos.z);
                    Debug.Log("BoxHit Left");
                    right = false;
                }
                else if (PlayerPlace.Contains(tempx + "," + (int)pos.z)) {
                    Debug.Log("PlayerHit Left");
                    //Kill Player
                    if (PlayerPlace[0] == tempx + "," + (int)pos.z)
                        DestroyPlayer(1);
                    else if (PlayerPlace[1] == tempx + "," + (int)pos.z)
                        DestroyPlayer(2);
                    else if (PlayerPlace[2] == tempx + "," + (int)pos.z)
                        DestroyPlayer(3);
                    else if (PlayerPlace[3] == tempx + "," + (int)pos.z)
                        DestroyPlayer(4);
                }
                else if (tempx % 2 == 0 && (int)pos.z % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Left");
                    right = false;
                }
            }
            tempy = (int)pos.z + i;
            if (up) {
                print("Up : " + (int)pos.x + "," + tempy);
                if (boxp.Contains((int)pos.x + "," + tempy)) {
                    //Delete Box
                    DestroyBox((int)pos.x, tempy);
                    Debug.Log("BoxHit Up");
                    up = false;
                }
                else if (PlayerPlace.Contains((int)pos.x + "," + tempy)) {
                    //Kill Player
                    Debug.Log("PlayerHit Up");
                    if (PlayerPlace[0] == (int)pos.x + "," + tempy)
                        DestroyPlayer(1);
                    else if (PlayerPlace[1] == (int)pos.x + "," + tempy)
                        DestroyPlayer(2);
                    else if (PlayerPlace[2] == (int)pos.x + "," + tempy)
                        DestroyPlayer(3);
                    else if (PlayerPlace[3] == (int)pos.x + "," + tempy)
                        DestroyPlayer(4);
                }
                else if ((int)pos.x % 2 == 0 && tempy % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Up");
                    up = false;
                }
            }
            tempy = (int)pos.z - i;
            if (down) {
                print("Down : " + (int)pos.x + "," + tempy);
                if (boxp.Contains((int)pos.x + "," + tempy)) {
                    //Delete Box
                    DestroyBox((int)pos.x, tempy);
                    Debug.Log("BoxHit Down");
                    down = false;
                }
                else if (PlayerPlace.Contains((int)pos.x + "," + tempy)) {
                    //Kill Player
                    Debug.Log("PlayerHit Down");
                    if (PlayerPlace[0] == (int)pos.x + "," + tempy)
                        DestroyPlayer(1);
                    else if (PlayerPlace[1] == (int)pos.x + "," + tempy)
                        DestroyPlayer(2);
                    else if (PlayerPlace[2] == (int)pos.x + "," + tempy)
                        DestroyPlayer(3);
                    else if (PlayerPlace[3] == (int)pos.x + "," + tempy)
                        DestroyPlayer(4);
                }
                else if ((int)pos.x % 2 == 0 && tempy % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Down");
                    down = false;
                }
            }
        }
        DestroyBomb();
    }

    void DestroyBox(int x, int y) {
        boxp.Remove(x + "," + y);
        GameObject box = GameObject.Find("Box " + x + "," + y);
        DestroyObject(box);
    }

    void DestroyPlayer(int number) {
        GameObject player = GameObject.Find("Player" + number);
        DestroyObject(player);
    }

    void DestroyBomb() {
        DestroyObject(gameObject);
    }

    void OnTriggerExit(Collider col) {
        gameObject.collider.enabled = true;
    }
}
