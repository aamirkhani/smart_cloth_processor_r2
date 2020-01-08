using System;
using System.Linq;
using USBHIDDRIVER;


namespace HIDConn
{
    class DeviceControl
    {
        private static string myVendorID = "vid_0483";//1155;
        private static string myProductID = "pid_5750";//22352;
        private static string leftRobotHIDChars = "55e846d";
        private static string rightRobotHIDChars = "291f89c";

        private static int FRAME_HEADER = 0x55;
        private static int CMD_SERVO_MOVE = 0x03;

        private string pathLeftRobot, pathRightRobot;



        private byte GET_LOW_BYTE(int A)
        {
            return (byte)((A & 0xFF));
        }

        private byte GET_HIGH_BYTE(int A)
        {
            return (byte)((A) >> 8 & 0xFF);
        }


        private USBHIDDRIVER.USBInterface mUsbInterface;
        private bool mIsConnected;

        private struct strCmd
        {
            public string deviceName;
            public int servoNum;
            public int pos;
            public int time;
            public int speed;
        }


        public DeviceControl()
        {
            mUsbInterface = new USBInterface(myVendorID, myProductID);
            string[] arrDevPaths = mUsbInterface.getDeviceList();
            int count = arrDevPaths.Count();
            for (int index = 0; index < count; index++)
            {
                string path = arrDevPaths[index];
                Console.WriteLine("device path: " + arrDevPaths[index]);
                if (path.Contains(leftRobotHIDChars))
                {
                    pathLeftRobot = path;
                }
                else if (path.Contains(rightRobotHIDChars))
                {
                    pathRightRobot = path;
                }

                Console.WriteLine("left device path: " + pathLeftRobot);
                Console.WriteLine("right device path: " + pathRightRobot);

            }
            mIsConnected = mUsbInterface.Connect();

        }



        public bool executeCommand(string command)
        {
            if (!mIsConnected)
            {
                Console.WriteLine("Not connected!!!");
                return false;
            }
            strCmd strCmdForHw;
            bool result = parseCommand(command, out strCmdForHw);
            if (result == true)
            {
                string dev = strCmdForHw.deviceName;
                string devPathName;
                if (dev == "L")
                {
                    devPathName = pathLeftRobot;
                }
                else if (dev == "R")
                {
                    devPathName = pathRightRobot;
                }
                else
                {
                    Console.WriteLine("devicename is not specified");
                    return false;
                }
                byte[] cmdForHw;
                result = prepareCommand(strCmdForHw, out cmdForHw);
                if (result == true)
                {
                    result = sendCommandToHardware(devPathName, cmdForHw);
                    Console.WriteLine("sent to Hardware? : " + result);
                    return result;
                }
            }
            Console.WriteLine("could not send command to hardware");
            return result;

        }

        private bool parseCommand(string command, out strCmd strCmdForHw)
        {

            strCmdForHw = default(strCmd);
            // split the items
            string[] sArray = command.Split(',');
            if (sArray.Length == 0)
            {
                return false;
            }
            strCmdForHw.deviceName = sArray[0];
            strCmdForHw.servoNum = int.Parse(sArray[1]);
            strCmdForHw.pos = int.Parse(sArray[2]);
            strCmdForHw.time = int.Parse(sArray[3]);
            if (sArray.Length == 5)
            {
                strCmdForHw.speed = int.Parse(sArray[4]);
            }

            return true;

        }

        private bool prepareMoveServo(int servoID, int position, int time, out byte[] cmdForHw)
        {
            cmdForHw = new byte[10];
            if (servoID > 6 || !(time > 0))
            {
                return false;
            }

            cmdForHw[0] = (byte)FRAME_HEADER;
            cmdForHw[1] = (byte)FRAME_HEADER;
            cmdForHw[2] = 8;
            cmdForHw[3] = (byte)CMD_SERVO_MOVE;
            cmdForHw[4] = 1;
            cmdForHw[5] = GET_LOW_BYTE(time);
            cmdForHw[6] = GET_HIGH_BYTE(time);
            cmdForHw[7] = (byte)servoID;
            cmdForHw[8] = GET_LOW_BYTE(position);
            cmdForHw[9] = GET_HIGH_BYTE(position);

            return true; ;

        }

        private bool prepareCommand(strCmd strCmdForHw, out byte[] cmdForHw)
        {
            cmdForHw = default(byte[]);

            //time
            int time = strCmdForHw.time;

            //position
            int position = strCmdForHw.pos;

            //servo num
            int servoNum = strCmdForHw.servoNum;


            bool result = prepareMoveServo(servoNum, position, time, out cmdForHw);
            return result;

        }


        private bool sendCommandToHardware(string devPathName, byte[] cmdForHw)
        {
            if (!mIsConnected)
            {
                return false;
            }

            bool result = mUsbInterface.write(devPathName, cmdForHw); ;
            return result;

        }
    }
}
