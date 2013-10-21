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

	// Use this for initialization
	void Start () {
	    
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

        ArrayList boxp = generateTerrainScript.BoxPlace;
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

        for (int i = 0; i < PowerBomb; i++) {
            tempx = (int)pos.x + i;
            if (left) {
                if (boxp.Contains(tempx + "," + (int)pos.y)) {
                    //Delete Box
                    Debug.Log("BoxHit Left");
                    left = false;
                }
                else if (PlayerPlace.Contains(tempx + "," + (int)pos.y)) {
                    Debug.Log("PlayerHit Left");
                    //Kill Player
                }
                else if (tempx % 2 == 0 && (int)pos.y % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Left");
                    left = false;
                }
            }
            tempx = (int)pos.x - i;
            if (right) {
                if (boxp.Contains(tempx + "," + (int)pos.y)) {
                    //Delete Box
                    Debug.Log("BoxHit Right");
                    right = false;
                }
                else if (PlayerPlace.Contains(tempx + "," + (int)pos.y)) {
                    Debug.Log("PlayerHit Right");
                    //Kill Player
                }
                else if (tempx % 2 == 0 && (int)pos.y % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Right");
                    right = false;
                }
            }
            tempy = (int)pos.y + i;
            if (up) {
                if (boxp.Contains((int)pos.x + "," + tempy)) {
                    //Delete Box
                    Debug.Log("BoxHit Up");
                    up = false;
                }
                else if (PlayerPlace.Contains((int)pos.x + "," + tempy)) {
                    //Kill Player
                    Debug.Log("PlayerHit Up");
                }
                else if ((int)pos.x % 2 == 0 && tempy % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Up");
                    up = false;
                }
            }
            tempy = (int)pos.y - i;
            if (down) {
                if (boxp.Contains((int)pos.x + "," + tempy)) {
                    //Delete Box
                    Debug.Log("BoxHit Down");
                    down = false;
                }
                else if (PlayerPlace.Contains((int)pos.x + "," + tempy)) {
                    //Kill Player
                    Debug.Log("PlayerHit Down");
                }
                else if ((int)pos.x % 2 == 0 && tempy % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Down");
                    down = false;
                }
            }
        }
    }
}
