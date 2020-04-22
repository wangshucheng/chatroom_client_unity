using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domain : MonoBehaviour {

    public string ip = "172.0.0.1";
    public int port = 8086;

    // Use this for initialization
    void Start () {
        SocketClient sc = new SocketClient();
        sc.Connect(ip, port);
        sc.Send("hello");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
