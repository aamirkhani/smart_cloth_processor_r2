using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace HIDConn
{
    class SocketServer
    {
        private int port = 53194;
        private static string TAG = "[ SOCKET_SERVER ]";
        private TcpListener mTcpListener = null;
        private TcpClient mTcpClient = null;
        private Thread mThreadClient = null;
        DeviceControl mDevCtrl;

        private int DELAY_ONE_SEC = 1000; // 1 sec delay for while loop for setting up new connections only
        private static int LENGTH_STREAM_FIXED = 30;


        public SocketServer()
        {
            mDevCtrl = new DeviceControl();
            startServer();
        }

        private void startServer()
        {
            try
            {
                Console.WriteLine(TAG + " port is: " + port);
                mTcpListener = new TcpListener(IPAddress.Any, port);
                mTcpListener.Start();
                //connect to clients in a thread
                listenForClient();


            }
            catch (Exception e)
            {
                writeExceptionLog(e, "startServer");
            }

        }

        private void listenForClient()
        {
            mThreadClient = new Thread(() =>
            {
                try
                {
                    mThreadClient.Name = "threadConnectClient";

                    while (true) //if client is null, accept new client connection
                    {
                        try
                        {
                            Console.WriteLine(TAG + "Waiting for connection..");
                            var client = mTcpListener.AcceptTcpClient();
                            disconnectClient();
                            mTcpClient = client;
                            Console.WriteLine(TAG + "Connected to client..");
                            startTakingCommand();


                        }
                        catch (Exception e)
                        {
                            writeExceptionLog(e, "connectToClients");

                        }
                        Thread.Sleep(DELAY_ONE_SEC);
                    }
                }
                catch (Exception e)
                {
                    writeExceptionLog(e, "connectToClients - thread");
                }
            });
            mThreadClient.Start();
        }


        private void startTakingCommand()
        {
            while (mTcpClient != null)
            {
                try
                {
                    NetworkStream networkStream = mTcpClient.GetStream();
                    //  if (networkStream.DataAvailable && networkStream.CanRead)
                    if (networkStream.CanRead)
                    {


                        byte[] buffer = new byte[LENGTH_STREAM_FIXED];
                        int numRead = networkStream.Read(buffer, 0, LENGTH_STREAM_FIXED);
                        if (numRead > 0)
                        {
                            string rcvdData = System.Text.Encoding.ASCII.GetString(buffer);

                            while (numRead < LENGTH_STREAM_FIXED)
                            {
                                int count = networkStream.Read(buffer, 0, (LENGTH_STREAM_FIXED - numRead));
                                if (count == 0)
                                {
                                    return;
                                }
                                numRead += count;
                                string bufferData = System.Text.Encoding.ASCII.GetString(buffer);
                                rcvdData += bufferData;

                            }
                            string data = rcvdData.TrimEnd();
                            Console.WriteLine("Received command: " + data);
                            mDevCtrl.executeCommand(data);
                        }
                        else
                        {
                            Console.WriteLine("read data is 0");
                            disconnectClient();

                        }
                    }

                }

                catch (Exception e)
                {
                    Console.Write("Exception");
                    handleException(e);

                }
            }
        }

        public void handleException(Exception ex)
        {

            if ((ex is IOException) && (ex.InnerException is SocketException))
            {
                SocketException se = (SocketException)ex.InnerException;
                // Debug.Log(TAG + " handle socket exception error code" + se.ErrorCode);
                //added network down as in Android Unity, socket gets closed whenever network is down
                if ((se.SocketErrorCode == SocketError.ConnectionReset) || (se.SocketErrorCode == SocketError.Shutdown) || (se.SocketErrorCode == SocketError.NetworkDown))
                {
                    //remote socket connection is lost
                    //   Debug.Log(TAG + " disconnecting client as there was connection reset by remote host" + se.ToString());
                    disconnectClient();
                }
            }
            else if (ex is ObjectDisposedException)
            {
                disconnectClient();
            }
        }





        private void disconnectClient()
        {

            try
            {
                Console.WriteLine(TAG + "disconnecting client...");
                if (mTcpClient != null)
                {
                    try
                    {
                        NetworkStream networkStream = mTcpClient.GetStream(); //this can throw exceptions
                        networkStream.Close();
                    }
                    catch (Exception e)
                    {
                        writeExceptionLog(e, "disconnectClient - network stream");
                    }

                    mTcpClient.Close();
                    mTcpClient = null;

                }
            }
            catch (Exception e)
            {
                writeExceptionLog(e, "disconnectClient");
            }

        }



        private void writeExceptionLog(Exception e, string methodName)
        {
            Debug.WriteLine(TAG + "==> Exception: " + methodName + "||" + e.ToString() + "||" + e.StackTrace);
        }


        private void abortThreads()
        {
            try
            {
                if (mThreadClient != null)
                {
                    Debug.WriteLine("in abort threads 1");
                    // mThreadClient.Abort(); //this needs to be fixed TODO ...causes app to freeze here
                    Debug.WriteLine("in abort threads 2");
                    mThreadClient = null;

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION");
                writeExceptionLog(e, "abortThreads");
            }
        }

        public void shutdownServer()
        {
            Console.WriteLine("shutting down server...");
            disconnectClient();
            abortThreads();
            if (mTcpListener != null)
            {
                mTcpListener.Stop();
                mTcpListener = null;

            }
            Console.WriteLine("server is shut down...");

        }
    }
}
