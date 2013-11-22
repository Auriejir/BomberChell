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
        InvokeRepeating("NewTarget", 1.0f, 0.25f);
    }
    void Update() {
        if (change)
            target = GetTarget();
        if (Vector3.Distance(form.position, target) > range) {
            move(target);
        }
    }

    void FixedUpdate() {
        if (form.position.x > generateTerrainScript.TerrainSize) {
            form.position = new Vector3(generateTerrainScript.TerrainSize - 1, 1, form.position.z);
        }
        else if (form.position.x < 0) {
            form.position = new Vector3(1, 1, form.position.z);
        }

        if (form.position.z > generateTerrainScript.TerrainSize) {
            form.position = new Vector3(form.position.x, 1, generateTerrainScript.TerrainSize - 1);
        }
        else if (form.position.x < 0) {
            form.position = new Vector3(form.position.x, 1, 1);
        }

        if (!form.position.y.Equals(1)) {
            form.position = new Vector3(form.position.x, 1, form.position.z);
        }
    }

    Vector3 GetTarget() {
        int choice = Random.Range(0, 2);
        Vector3 target = Vector3.zero;
        switch (choice) {
            case 0:
                target = new Vector3(Random.Range(0, generateTerrainScript.TerrainSize - 1), 1, 0);
                break;
            case 1:
                target = new Vector3(0, 1, Random.Range(0, generateTerrainScript.TerrainSize - 1));
                break;
        }
        return target;
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

    void OnCollisionEnter(Collision collision) {
        target = new Vector3(target.x * -1, 1, target.z * -1);
    }
}
