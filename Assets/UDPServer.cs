using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPServer : MonoBehaviour
{
    private static int localPort;

    // prefs
    private string IP;  // define in init
    public int port;  // define in init
    private float timeleft = 1;
   
    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;
   
    // gui
    string strMessage="";

    void init()
    {
        // define
        IP="127.0.0.1";
        port=8051;
       
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
       
        // status
        print("Sending to "+IP+" : "+port);
        print("Testing: nc -lu "+IP+" : "+port);
    }

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        // print(timeleft);
        if (timeleft<0)
        {
            var timestr = Time.time.ToString("f6");
            byte[] data = Encoding.UTF8.GetBytes(timestr);
            client.Send(data, data.Length, remoteEndPoint);
            print("packet sent at " + timestr);
            timeleft = 1;
        }
    }
}
