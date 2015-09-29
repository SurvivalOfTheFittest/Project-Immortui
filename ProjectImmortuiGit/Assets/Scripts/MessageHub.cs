using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageHub : MonoBehaviour {
    Dictionary<string,Vector3> positionCommList = new Dictionary<string, Vector3>();
    Dictionary<string,bool> boollist = new Dictionary<string, bool>();
    // Use this for initialization
    void Start () {
    }
    public Vector3 GetMesPos(string fstr)
    {

        return positionCommList[fstr];
    }
   
    public bool IsMesPosSet(string fstr) {
        return positionCommList.ContainsKey(fstr) ? true : false;
    }

    public void setMesPos(string fstr, Vector3 fpos)
    {
        if (positionCommList.ContainsKey(fstr))
        {
            positionCommList[fstr] = fpos;

        }
        else {
            positionCommList.Add(fstr, fpos);
        }
      
    }
    public bool GetMesBool(string fstr)
    {

        return boollist[fstr];
    }

    public bool IsMesBoolSet(string fstr)
    {
        return boollist.ContainsKey(fstr) ? true : false;
    }
    public void addMesBool(string fstr, bool fpos) {
        boollist.Add(fstr, fpos);
    }
    public void setMesBool(string fstr, bool fpos)
    {
        if (boollist.ContainsKey(fstr))
        {
            boollist[fstr] = fpos;

        }
        else
        {
            boollist.Add(fstr, fpos);
        }

    }
    // Update is called once per frame
    void Update () {
	
	}
}
