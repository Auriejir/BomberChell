using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    [SerializeField]
    private int _powerBomb = 1;
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

    [SerializeField]
    private GameObject _fire;
    public GameObject Fire {
        get { return _fire; }
        set { _fire = value; }
    }

    private int alive = 0;
    private bool finish = false;
    private GameObject origin;
    private GenerateTerrainScript generateTerrainScript;
    private bool done = false;
    private ArrayList boxp;
    private ArrayList bombp;
    private Transform form;
    // Use this for initialization
    void Start() {
        origin = GameObject.Find("Origin");
        generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
        bombp = generateTerrainScript.BombPlace;
        form = this.gameObject.transform;
        gameObject.collider.enabled = false;
    }

    // Update is called once per frame
    void Update() {
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
        Vector3 pos = form.position;
        boxp = generateTerrainScript.BoxPlace;
        ArrayList PlayerPlace = new ArrayList();
        ArrayList ComPlace = new ArrayList();
        if (generateTerrainScript.Player1Alive) {
            GameObject player1 = GameObject.Find("Player1");
            characterScript p1 = player1.GetComponent<characterScript>();
            PlayerPlace.Add(p1.Position[0] + "," + p1.Position[1]);
            Debug.Log(p1.Position[0] + "," + p1.Position[1]);
            alive++;
        }
        else {
            PlayerPlace.Add("aurix , jojo");
        }
        if (generateTerrainScript.Player2Alive) {
            GameObject player2 = GameObject.Find("Player2");
            characterScript p2 = player2.GetComponent<characterScript>();
            PlayerPlace.Add(p2.Position[0] + "," + p2.Position[1]);
            alive++;
        }
        else {
            PlayerPlace.Add("aurix , jojo");
        }
        if (generateTerrainScript.Player3Alive) {
            GameObject player3 = GameObject.Find("Player3");
            characterScript p3 = player3.GetComponent<characterScript>();
            PlayerPlace.Add(p3.Position[0] + "," + p3.Position[1]);
            alive++;
        }
        else {
            PlayerPlace.Add("aurix , jojo");
        }
        if (generateTerrainScript.Player4Alive) {
            GameObject player4 = GameObject.Find("Player4");
            characterScript p4 = player4.GetComponent<characterScript>();
            PlayerPlace.Add(p4.Position[0] + "," + p4.Position[1]);
            alive++;
        }
        else {
            PlayerPlace.Add("aurix , jojo");
        }

        if (generateTerrainScript.Com1Alive) {
            GameObject com1 = GameObject.Find("COM1");
            COMScript c1 = com1.GetComponent<COMScript>();
            ComPlace.Add(c1.Position[0] + "," + c1.Position[1]);
            alive++;
        }
        else {
            ComPlace.Add("aurix , jojo");
        }
        if (generateTerrainScript.Com1Alive) {
            GameObject com2 = GameObject.Find("COM2");
            COMScript c2 = com2.GetComponent<COMScript>();
            ComPlace.Add(c2.Position[0] + "," + c2.Position[1]);
            alive++;
        }
        else {
            ComPlace.Add("aurix , jojo");
        }
        if (generateTerrainScript.Com3Alive) {
            GameObject com3 = GameObject.Find("COM3");
            COMScript c3 = com3.GetComponent<COMScript>();
            ComPlace.Add(c3.Position[0] + "," + c3.Position[1]);
            alive++;
        }
        else {
            ComPlace.Add("aurix , jojo");
        }

        bool up = true, down = true, left = true, right = true;
        int tempy;
        int tempx;

        print("Bomb : " + (int)pos.x + "," + (int)pos.z);
        GameObject expl = Instantiate(Fire, new Vector3(pos.x, 1, pos.z), Quaternion.identity) as GameObject;
        Destroy(expl, 1);
        for (int i = 0; i <= PowerBomb; i++) {
            tempx = (int)pos.x + i;
            if (left) {
                expl = Instantiate(Fire, new Vector3(tempx, 1, pos.z), Quaternion.identity) as GameObject;
                Destroy(expl, 1);
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
                    if (PlayerPlace[0].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(1);
                        generateTerrainScript.Player1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[1].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(2);
                        generateTerrainScript.Player2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[2].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(3);
                        generateTerrainScript.Player3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[3].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(4);
                        generateTerrainScript.Player4Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (ComPlace.Contains(tempx + "," + (int)pos.z)) {
                    if (ComPlace[0].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(1);
                        generateTerrainScript.Com1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[1].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(2);
                        generateTerrainScript.Com2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[2].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(3);
                        generateTerrainScript.Com3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (tempx == generateTerrainScript.TerrainX || tempx % 2 == 0 && (int)pos.z % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Right");
                    left = false;
                }
            }
            tempx = (int)pos.x - i;
            if (right) {
                expl = Instantiate(Fire, new Vector3(tempx, 1, pos.z), Quaternion.identity) as GameObject;
                Destroy(expl, 1);
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
                    if (PlayerPlace[0].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(1);
                        generateTerrainScript.Player1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[1].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(2);
                        generateTerrainScript.Player2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[2].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(3);
                        generateTerrainScript.Player3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[3].ToString() == tempx + "," + (int)pos.z) {
                        DestroyPlayer(4);
                        generateTerrainScript.Player4Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (ComPlace.Contains(tempx + "," + (int)pos.z)) {
                    if (ComPlace[0].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(1);
                        generateTerrainScript.Com1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[1].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(2);
                        generateTerrainScript.Com2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[2].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(3);
                        generateTerrainScript.Com3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (tempx == generateTerrainScript.TerrainX || tempx % 2 == 0 && (int)pos.z % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Left");
                    right = false;
                }
            }
            tempy = (int)pos.z + i;
            if (up) {
                expl = Instantiate(Fire, new Vector3(pos.x, 1, tempy), Quaternion.identity) as GameObject;
                Destroy(expl, 1);
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
                    if (PlayerPlace[0].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(1);
                        generateTerrainScript.Player1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[1].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(2);
                        generateTerrainScript.Player2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[2].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(3);
                        generateTerrainScript.Player3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[3].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(4);
                        generateTerrainScript.Player4Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (ComPlace.Contains((int)pos.x + "," + tempy)) {
                    if (ComPlace[0].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(1);
                        generateTerrainScript.Com1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[1].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(2);
                        generateTerrainScript.Com2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[2].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(3);
                        generateTerrainScript.Com3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (tempy == generateTerrainScript.TerrainZ || (int)pos.x % 2 == 0 && tempy % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Up");
                    up = false;
                }
            }
            tempy = (int)pos.z - i;
            if (down) {
                expl = Instantiate(Fire, new Vector3(pos.x, 1, tempy), Quaternion.identity) as GameObject;
                Destroy(expl, 1);
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
                    if (PlayerPlace[0].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(1);
                        generateTerrainScript.Player1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[1].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(2);
                        generateTerrainScript.Player2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[2].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(3);
                        generateTerrainScript.Player3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (PlayerPlace[3].ToString() == (int)pos.x + "," + tempy) {
                        DestroyPlayer(4);
                        generateTerrainScript.Player4Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (ComPlace.Contains((int)pos.x + "," + tempy)) {
                    if (ComPlace[0].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(1);
                        generateTerrainScript.Com1Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[1].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(2);
                        generateTerrainScript.Com2Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                    else if (ComPlace[2].ToString() == tempx + "," + (int)pos.z) {
                        DestroyCom(3);
                        generateTerrainScript.Com3Alive = false;
                        alive--;
                        if (alive <= 1) {
                            finish = true;
                        }
                    }
                }
                else if (tempy == generateTerrainScript.TerrainZ || (int)pos.x % 2 == 0 && tempy % 2 == 0) {
                    //Wall Touch
                    Debug.Log("WallHit Down");
                    down = false;
                }
            }
        }
        if (!finish) {
            DestroyBomb((int)form.position.x, (int)form.position.y);
        }
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

    void DestroyCom(int number) {
        GameObject player = GameObject.Find("COM" + number);
        DestroyObject(player);
    }

    void DestroyBomb(int x, int y) {
        bombp.Remove(x + "," + y);
        DestroyObject(gameObject);
    }

    void OnTriggerExit(Collider col) {
        gameObject.collider.enabled = true;
    }

    void OnGUI() {
        if (alive <= 1 && finish) {
            if (generateTerrainScript.Player1Alive) {
                GUI.Box(new Rect(250, 150, 150, 50), "Player1 Win");
            }
            else if (generateTerrainScript.Player2Alive) {
                GUI.Box(new Rect(250, 150, 150, 50), "Player2 Win");
            }
            else if (generateTerrainScript.Player3Alive) {
                GUI.Box(new Rect(250, 150, 150, 50), "Player3 Win");
            }
            else if (generateTerrainScript.Player4Alive) {
                GUI.Box(new Rect(250, 150, 150, 50), "Player4 Win");
            }
            else if (generateTerrainScript.Com1Alive) {
                GUI.Box(new Rect(250, 150, 150, 50), "COM1 Win");
            }
            else if (generateTerrainScript.Com2Alive) {
                GUI.Box(new Rect(250, 150, 150, 50), "COM2 Win");
            }
            else if (generateTerrainScript.Com3Alive) {
                GUI.Box(new Rect(250, 150, 150, 50), "COM3 Win");
            }
            else {
                GUI.Box(new Rect(250, 150, 150, 50), "Draw");
            }
        }
    }
}
