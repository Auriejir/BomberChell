﻿using UnityEngine;
using System.Collections;

public class characterScript : MonoBehaviour {

  [SerializeField]
  private float _speed = 2;
  
  public float Speed {
    get{return _speed;}
    set{_speed = Mathf.Clamp(value,0,40);}
  }
	
  private int[] position = new int[2];
  public int[] Position {
      get { return position; }
      set { position = value; }
  }
  
  private Transform form;
  public Transform Bomb;

  private NetworkView _myNetworkView = null;
  
	// Use this for initialization
  void Start() {
      form = this.gameObject.transform;
      _myNetworkView = this.gameObject.GetComponent<NetworkView>();
  }
	
	// Update is called once per frame
    void Update() {
        if (networkView.isMine) {
            int x = (int)form.position.x;
            int y = (int)form.position.y;
            Position[0] = x;
            Position[1] = y;

            if (Input.GetKey(KeyCode.Q)) {
                _myNetworkView.RPC("move", RPCMode.Server, 1, _speed);
                //move('x',_speed);
            }
            if (Input.GetKey(KeyCode.D)) {
                _myNetworkView.RPC("move", RPCMode.Server, 1, _speed * -1);
                //move('x',_speed * -1);
            }
            if (Input.GetKey(KeyCode.Z)) {
                _myNetworkView.RPC("move", RPCMode.Server, 2, _speed);
                //move('y',_speed);
            }
            if (Input.GetKey(KeyCode.S)) {
                _myNetworkView.RPC("move", RPCMode.Server, 2, _speed * -1);
                //move('y',_speed * -1);
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
                _myNetworkView.RPC("dropBomb", RPCMode.Server);
            }
        }
    }

    [RPC]
    void dropBomb() {
        Instantiate(Bomb, new Vector3(Mathf.Round(form.position.x), 0.25f, Mathf.Round(form.position.z)), Quaternion.identity);
        if (Network.isServer) {
            _myNetworkView.RPC("dropBomb", RPCMode.Others);
        }
    }

    [RPC]
    void move(int axis, float value) {
        switch (axis) {
            case 1:
                form.position += Vector3.left * value * Time.deltaTime;
                break;
            case 2:
                form.position += Vector3.forward * value * Time.deltaTime;
                break;
        }
        if (Network.isServer) {
            _myNetworkView.RPC("move", RPCMode.Others, axis, value);
        }
    }
  
}
