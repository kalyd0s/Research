using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class WebCamReceiver : MonoBehaviour
{

    public RawImage camFeed;

    public int port = 5053;

    private UdpClient client;
    private Thread receiveThread;
    private bool running;


    private byte[] recentFrame;
    private readonly object frameLock = new object();
    private Texture2D texture;

    public void Start()
    {
        texture = new Texture2D(2, 2);
        camFeed.texture = texture;

        running = true;
        client = new UdpClient(port);

        receiveThread = new Thread(ReceiveVideo);
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }


    public void ReceiveVideo()
    {

        while (running)
        {

            try
            {
                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

                byte[] bytes = client.Receive(ref sender);

                lock (frameLock)
                {
                    recentFrame = bytes;
                }
            }
            catch (SocketException)
            {
                if (!running)
                {
                    break;
                }

            }
            catch (ObjectDisposedException)
            {
                break;
            }
        }
    }



    public void Update()
    {
        byte[] frame = null;

        lock (frameLock)
        {
            if(recentFrame != null)
            {
                frame = recentFrame;
                recentFrame = null;
            }

        }

        if (frame != null)
        {
            texture.LoadImage(frame);
            camFeed.texture = texture;

        }
    }


    void OnDestroy()
    {
        running = false;
        client?.Close();
        receiveThread?.Join(1000);

        if (texture != null)
        {
           Destroy(texture);
        }
    }

}
