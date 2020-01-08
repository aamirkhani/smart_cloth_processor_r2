using System;
using System.Linq;
using System.Threading;
using USBHIDDRIVER;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace HIDConn
{
    class Program
    {
        static SocketServer sktServer;
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }


        static void Main(string[] args)
        {

            _handler += new EventHandler(ConsoleCtrlCheck);
            SetConsoleCtrlHandler(_handler, true);

            sktServer = new SocketServer();

        }



        static void OnProcessExit(object sender, EventArgs e)
        {

            sktServer.shutdownServer();
        }



        private static bool ConsoleCtrlCheck(CtrlType ctrlType)
        {
            switch (ctrlType)
            {

                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                    sktServer.shutdownServer();
                    break;

                default:
                    break;
            }

            return true;

        }

        private static string myVendorID = "vid_0483";//1155;
        private static string myProductID = "pid_5750";//22352;
        private static USBHIDDRIVER.USBInterface mUsbInterface;
        static void test()
        {
            mUsbInterface = new USBInterface(myVendorID, myProductID);
            bool mIsConnected = mUsbInterface.Connect();
            Console.WriteLine("is connected?" + mIsConnected);

            string[] list = mUsbInterface.getDeviceList();

            int count = list.Count();
            Console.WriteLine("count?" + count);

            for (int index = 0; index < count; index++)
            {
                Console.WriteLine("device path: " + list[index]);
                Debug.WriteLine(list[index]);

            }
            //start a thread for interaction with hardware
            Console.WriteLine("calling  executeCommandsInList");
            Thread.Sleep(2000);

            bool conn = mUsbInterface.Connect();
            Console.WriteLine("connection success? : " + conn);
            Thread.Sleep(2000);

            int servoTime = 1000;
            byte i = (byte)(servoTime & 0xFF);
            byte j = (byte)(servoTime >> 8 & 0xFF);
            int position = 2500;

            byte pos_low = (byte)(position & 0xFF);
            byte pos_high = (byte)(position >> 8 & 0xFF);

            byte[] cmd = { 85, 85, 8, 3, (byte)1, i, j, (byte)1, pos_low, pos_high };

            bool written1 = mUsbInterface.write(list[0], cmd);
            Console.WriteLine("data written? : " + written1);

            bool written2 = mUsbInterface.write(list[1], cmd);
            Console.WriteLine("data written? : " + written2);
        }

    }

}



