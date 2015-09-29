using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InstantiateGun : MonoBehaviour {
    public GameObject CurrentGun;
    GameObject curgun = null;
    GameObject oldgun = null;

    List<GameObject> guns = new List<GameObject>();
    int curgunid = -1;
    public int MagsLeft = 10;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (curgun != CurrentGun)
        {
            if (oldgun != null)
            {
                 foreach(var comp in oldgun.gameObject.GetComponentsInChildren<Collider>()) comp.enabled = false;
                 foreach(var comp in oldgun.gameObject.GetComponentsInChildren<Renderer>()) comp.enabled = false;
                oldgun.gameObject.GetComponent<ShootScript>().enabled = false;
            }
            else
            {
                Destroy(oldgun);
            }
            oldgun = (GameObject)GameObject.Instantiate(CurrentGun, this.transform.position, this.transform.rotation);
            oldgun.transform.parent = this.gameObject.transform;
            oldgun.gameObject.GetComponent<ShootScript>().cam = this.transform.parent.gameObject;
            oldgun.gameObject.GetComponent<ShootScript>().MagsLeft = MagsLeft;
            if (!guns.Contains(oldgun)) guns.Add(oldgun);
            curgun = CurrentGun;
        }
        if (Input.mouseScrollDelta.y > 0) {

            foreach (var comp in oldgun.gameObject.GetComponentsInChildren<Collider>()) comp.enabled = false;
            foreach (var comp in oldgun.gameObject.GetComponentsInChildren<Renderer>()) comp.enabled = false;
            oldgun.gameObject.GetComponent<ShootScript>().enabled = false;
            foreach (var comp in guns[(curgunid = curgunid + 1 < guns.Count ? curgunid + 1 : 0)].gameObject.GetComponentsInChildren<Renderer>()) comp.enabled = true;
            foreach (var comp in guns[curgunid].gameObject.GetComponentsInChildren<Renderer>()) comp.enabled = true;
            guns[curgunid].gameObject.GetComponent<ShootScript>().enabled = true;
            oldgun = guns[curgunid];
            curgun = guns[curgunid];
            CurrentGun = guns[curgunid];
        }
        if (Input.mouseScrollDelta.y < 0)
        {

            foreach (var comp in oldgun.gameObject.GetComponentsInChildren<Collider>()) comp.enabled = false;
            foreach (var comp in oldgun.gameObject.GetComponentsInChildren<Renderer>()) comp.enabled = false;
            oldgun.gameObject.GetComponent<ShootScript>().enabled = false;
            foreach (var comp in guns[(curgunid = curgunid - 1 >= 0 ? curgunid - 1 : guns.Count-1)].gameObject.GetComponentsInChildren<Renderer>()) comp.enabled = true;
            foreach (var comp in guns[curgunid].gameObject.GetComponentsInChildren<Renderer>()) comp.enabled = true;
            guns[curgunid].gameObject.GetComponent<ShootScript>().enabled = true;
            oldgun = guns[curgunid];
            curgun = guns[curgunid];
            CurrentGun = guns[curgunid];
        }
    }
        
}
