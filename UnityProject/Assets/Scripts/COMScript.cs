using UnityEngine;
using System.Collections;

public class COMScript : MonoBehaviour {

    [SerializeField]
    private float _orientation = 0;
    public float Orientation {
        get { return _orientation; }
        set { _orientation = Mathf.Clamp(value, 0, 360); }
    }

    bool change;
    float range;
    Vector3 target;
    private GameObject origin;
    private Transform form;
    private GenerateTerrainScript generateTerrainScript;

    void Start() {
        form = this.gameObject.transform;
        origin = GameObject.Find("Origin");
        generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
        range = 2f;
        target = GetTarget();
        InvokeRepeating("NewTarget", 2.0f, 1.0f);
    }
    void Update() {
        if (change)
            target = GetTarget();
        if (Vector3.Distance(form.position, target) > range) {
            move(target);
        }
    }
    Vector3 GetTarget() {
        return new Vector3(Random.Range(0, generateTerrainScript.TerrainSize), 0, Random.Range(0, generateTerrainScript.TerrainSize));
    }
    void NewTarget() {
        int choice = Random.Range(0, 3);
        switch (choice) {
            case 0:
                change = true;
                break;
            case 1:
                change = false;
                break;
            case 2:
                target = form.position;
                break;
        }
    }

    void move(Vector3 resultVector) {
        form.position += resultVector * Time.deltaTime;
        if (resultVector.x > 0) {
            if (resultVector.z > 0) Orientation = 45;
            else if (resultVector.z > 0) Orientation = 135;
            else Orientation = 90;
        }
        else if (resultVector.x > 0) {
            if (resultVector.z > 0) Orientation = 315;
            else if (resultVector.z > 0) Orientation = 225;
            else Orientation = 270;
        }
        else {
            if (resultVector.z > 0) Orientation = 0;
            else if (resultVector.z > 0) Orientation = 180;
        }
        form.rotation = Quaternion.Euler(0, Orientation, 0);
    }
}
