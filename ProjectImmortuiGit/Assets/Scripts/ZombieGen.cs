using UnityEngine;
using System.Collections;

public class ZombieGen : MonoBehaviour {
    public Vector3 difbetween;
    public int amountofzombies;
    public GameObject Terrain;
    public GameObject prefab;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < amountofzombies; i++) {
          Instantiate(prefab,
              this.transform.position 
                  +new Vector3(
                      Random.Range(Terrain.transform.position.x-Terrain.transform.localScale.x/2, 
                                   Terrain.transform.position.x + Terrain.transform.localScale.x / 2),
                      Random.Range(Terrain.transform.position.y - Terrain.transform.localScale.y / 2, 
                                   Terrain.transform.position.y + Terrain.transform.localScale.y / 2),
                      Random.Range(Terrain.transform.position.z - Terrain.transform.localScale.z / 2, 
                                   Terrain.transform.position.z + Terrain.transform.localScale.z / 2)
                                   ),
              Quaternion.identity
              );
        }
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
