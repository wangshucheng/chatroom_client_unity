using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MQCenter : MonoBehaviour
{

    private static MQCenter _ins;
    public static MQCenter Ins
    {
        get
        {
            if (_ins == null)
            {
                GameObject go = new GameObject(typeof(MQCenter).Name);
                go.AddComponent<MQCenter>();
                DontDestroyOnLoad(go);
            }
            return _ins;
        }
    }

    public void Init()
    {

    }

    private void Awake()
    {
        _ins = this;
    }

    Queue<byte[]> _q;

    // Use this for initialization
    void Start()
    {
        _q = new Queue<byte[]>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_q != null && _q.Count > 0)
        {
            var buf = _q.Dequeue();
            Debug.Log(string.Format("收到服务器消息:{0}", Encoding.UTF8.GetString(buf)));
        }
    }

    public void Add(byte[] buf)
    {
        lock (_q)
        {
            _q.Enqueue(buf);
        }
    }
}
