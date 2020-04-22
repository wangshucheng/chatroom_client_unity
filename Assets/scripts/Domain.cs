using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domain : MonoBehaviour {

    public string ip = "127.0.0.1";
    public int port = 1238;

    public bool isAsync = true;

    SocketClient sc;

    // Use this for initialization
    void Start () {
        sc = new SocketClient();
        if (isAsync)
            StartCoroutine(sc.Connect(ip, port));
        else
            sc.ConnectAsync(ip, port);
        //sc.Send("hello");

    }

    float time = 0;
    float times = 0;

	// Update is called once per frame
	void Update () {
		if (!isAsync && times < 10)
        {
            time += Time.deltaTime;
            if (time>2)
            {
                sc.Send("hello");
                time -= 2;
                times++;
            }
        }
	}
}
