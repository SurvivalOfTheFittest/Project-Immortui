using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    void OnTriggerEnter(Collider col) {
        col.gameObject.transform.position = Vector3.zero;
    }
	// Update is called once per frame
	void Update () {
	
	}
}
