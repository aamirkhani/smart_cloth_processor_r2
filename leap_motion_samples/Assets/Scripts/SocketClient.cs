using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    private static string TAG = "[ SOCKET_CLIENT ]";

    [SerializeField]
    private string serverIp;

    private int port = 53194;

    private Thread mThread = null;
    public TcpClient mTcpClient;
    private static readonly object lockObj = new object();
    private bool mIsThreadAborted = true;
    private bool mIsPaused = false;
    private int DELAY_WHILE_LOOP_MS = 1; // 1 ms delay
    public System.Timers.Timer mTimerCheckServerAlive = null; //this is added to check the case of wifi being down on the server.
    private static int LENGTH_STREAM_FIXED = 30;
    // Start is called before the first frame update
    void Start()
    {
        mIsThreadAborted = true;
        ConnectToServer();

    }


    // Update is called once per frame
    void Update()
    {

    }

    private void ConnectToServer()
    {
        // start recv thread        
        if (mThread == null)
        {
            mIsThreadAborted = false;
            mThread = new Thread(() =>
            {
                try
                {
                    mThread.Name = "threadClient";
                    //try to reconnect if not connected
                    while (!mIsThreadAborted)
                    {
                        if (mTcpClient == null)
                        {
                            try
                            {
                                mTcpClient = new TcpClient(serverIp, port);
                                Debug.Log(TAG + "created** new tcpClient");

                            }
                            catch (Exception e)
                            {
                                // writeExceptionLog(e, "FaceTrackerClient-creating tcp client");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    writeExceptionLog(e, "SocketClient - thread");
                }

            });
            mThread.Start();
        }
    }

    public void writeExceptionLog(Exception e, String methodName)
    {
        Debug.Log(TAG + "==> Exception: " + methodName + "||" + e.ToString() + "||" + e.StackTrace);
    }

    public bool isConnectedToServer()
    {
        if (mTcpClient == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool sendToServer(string data)
    {
        lock (lockObj)
        {
            if (mTcpClient == null)
            {
                return false;
            }

            try
            {
                NetworkStream networkStream = mTcpClient.GetStream();
                if (networkStream.CanWrite)
                {
                    string dataPad = data.PadRight(LENGTH_STREAM_FIXED);
                    networkStream.Write(System.Text.Encoding.ASCII.GetBytes(dataPad), 0, LENGTH_STREAM_FIXED);
                    networkStream.Flush();
                    return true;
                }

            }
            catch (Exception ex)
            {
                writeExceptionLog(ex, "sendToServer" + ex.ToString());
                handleException(ex);

            }
        }
        return false;
    }

    public void handleException(Exception ex)
    {

        if ((ex is IOException) && (ex.InnerException is SocketException))
        {
            SocketException se = (SocketException)ex.InnerException;
            // Debug.Log(TAG + " handle socket exception error code" + se.ErrorCode);
            //added network down as in Android Unity, socket gets closed whenever network is down
            if ((se.SocketErrorCode == SocketError.ConnectionAborted) || (se.SocketErrorCode == SocketError.ConnectionReset) || (se.SocketErrorCode == SocketError.Shutdown) || (se.SocketErrorCode == SocketError.NetworkDown))
            {
                //remote socket connection is lost
                //   Debug.Log(TAG + " disconnecting client as there was connection reset by remote host" + se.ToString());
                disconnectClient();
            }
        }
    }

    private void abortThreads()
    {
        try
        {
            if (mThread != null)
            {
                mIsThreadAborted = true;
                mThread.Abort();
                mThread = null;
            }
        }
        catch (Exception e)
        {
            writeExceptionLog(e, "abortThreads");
        }
    }

    private void disconnectClient()
    {

        lock (lockObj)
        {
            Debug.Log(TAG + "disconnecting the client ");
            // close socket
            try
            {
                if (mTcpClient != null)
                {
                    try
                    {
                        NetworkStream networkStream = mTcpClient.GetStream();
                        networkStream.Close();
                    }
                    catch (Exception e)
                    {
                        writeExceptionLog(e, "disconnectClient - networkstream");
                    }
                    mTcpClient.Close();
                    mTcpClient = null;

                }
            }
            catch (Exception e)
            {
                writeExceptionLog(e, "disconnectClient - tcpclient");
            }
        }
    }




    private void stopServer()
    {
        abortThreads();
        // close socket
        disconnectClient();
    }




    public void OnApplicationQuit()
    {
        stopServer();

    }





}
