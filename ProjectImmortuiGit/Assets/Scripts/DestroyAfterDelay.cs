using UnityEngine;
using System.Collections;

public class DestroyAfterDelay : MonoBehaviour {
    float curtime = 0.0f;
    public float TimeDelay = 1.0f;
	// Use this for initialization
	void Start () {
        TimeDelay = this.GetComponent<ParticleSystem>().startLifetime;
	}
	
	// Update is called once per frame
	void Update () {
        curtime += Time.deltaTime;

        if (curtime >= TimeDelay) Destroy(this.gameObject); 
	}
}
