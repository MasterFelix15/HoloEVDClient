using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port; // define > init
    public List<GameObject> objects;
    private Dictionary<string, string> updates;

    private void init()
    {
        updates = new Dictionary<string, string>();
        port = 8051;
        print("Sending to 127.0.0.1 : " + port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                // print(">> " + text);
                // decoding
                if (text.Length == 0)
                {
                    continue;
                }
                text = text.Substring(0, text.Length - 1);
                foreach (string objstr in text.Split('#'))
                {
                    string name = objstr.Split(':')[0];
                    // var pos = objstr.Split(':')[1].Split(',');
                    // Vector3 position = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
                    // var rot = objstr.Split(':')[2].Split(',');
                    // Quaternion rotation = new Quaternion(float.Parse(rot[0]), float.Parse(rot[1]), float.Parse(rot[2]), float.Parse(rot[3]));
                    updates[name] = objstr;
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (string name in updates.Keys)
        {
            string objstr = updates[name];
            var pos = objstr.Split(':')[1].Split(',');
            Vector3 position = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
            var rot = objstr.Split(':')[2].Split(',');
            Quaternion rotation = new Quaternion(float.Parse(rot[0]), float.Parse(rot[1]), float.Parse(rot[2]), float.Parse(rot[3]));
            GameObject trackedObject = GameObject.Find(name);
            trackedObject.transform.position = position;
            trackedObject.transform.rotation = rotation;
        }
    }
}
