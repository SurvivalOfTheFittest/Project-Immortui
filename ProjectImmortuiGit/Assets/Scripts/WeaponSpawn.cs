using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponSpawn : MonoBehaviour {
    public GameObject[] weapons;
    public GameObject Terrain;
    public GameObject WeaponDispenser;
    public float maxWait;
    public float minWait;
    public float Timeout;
    float curtime = 0.0f;
    float spawntime;
    // Use this for initialization
    void Start () {
        spawntime = Random.Range(minWait,maxWait);
	}
	
	// Update is called once per frame
	void Update () {
        if (curtime >= spawntime) {
            Vector3 randvec = new Vector3(Random.Range(Terrain.transform.localScale.x/2, -Terrain.transform.localScale.x/2),
                0.5f,   
                Random.Range(Terrain.transform.localScale.z/2, -Terrain.transform.localScale.z/2));
            GameObject gunspawner = (GameObject)GameObject.Instantiate(WeaponDispenser,Terrain.transform.position + randvec,this.transform.rotation);
            gunspawner.GetComponent<WeaponDispenser>().Weapon = weapons[(int)Random.Range(0,weapons.Length)];
            gunspawner.GetComponent<WeaponDispenser>().Timeout = Timeout;
            spawntime = Random.Range(minWait, maxWait);
            curtime = 0.0f;
        }
        curtime += Time.deltaTime;
	}
}
