using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LeArm__MOTORS_TO_GRIPPER_POSE : MonoBehaviour {

    /*[SerializeField] public GameObject RobotSceneRoot;*/

    [Range(0.0f,1.0f)]
    [SerializeField] public float Servo1;
    [Range(0.0f, 1.0f)]
    [SerializeField] public float Servo2;
    [Range(0.0f, 1.0f)]
    [SerializeField] public float Servo3;
    [Range(0.0f, 1.0f)]
    [SerializeField] public float Servo4;
    [Range(0.0f, 1.0f)]
    [SerializeField] public float Servo5;
    [Range(0.0f, 1.0f)]
    [SerializeField] public float Servo6;

    [Range(0.01f, 0.10f)]
    [SerializeField]
    public float MOTOR_SPEED = 0.02f;

    [SerializeField] public GameObject Servo2_Hinge;
    [SerializeField] public GameObject Servo3_Hinge;
    [SerializeField] public GameObject Servo4_Hinge;
    [SerializeField] public GameObject Servo5_Hinge;
    [SerializeField] public GameObject Servo6_Hinge;

    [SerializeField] public GameObject Servo1_L_Open;
    [SerializeField] public GameObject Servo1_L_Closed;
    [SerializeField] public GameObject Servo1_L_Animating;
    [SerializeField] public GameObject Servo1_R_Open;
    [SerializeField] public GameObject Servo1_R_Closed;
    [SerializeField] public GameObject Servo1_R_Animating;


    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float realServo1;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float realServo2;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float realServo3;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float realServo4;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float realServo5;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float realServo6;

    [SerializeField] public bool FollowRobotTarget = false;
    [SerializeField] public bool FollowRobotTarget__V2 = false;
    

    private SocketClient scrSocketClient;
    private System.Timers.Timer mTimerForServos = null;
    private int TIMER_INTERVAL = 1000; // 1 second
    private LeArm___S123456_TO__ROBOT_POSE_MATHEMATICS LeArm__mathematics;
    private string strNameGameObj;



    // Use this for initialization
    void Start () {
        LeArm__mathematics = GetComponent<LeArm___S123456_TO__ROBOT_POSE_MATHEMATICS>();

        strNameGameObj = gameObject.name;
        scrSocketClient = GameObject.Find("GLOBAL_SCRIPTS").GetComponent<SocketClient>();
        startTimer();
       
        //StartCoroutine(TestCoroutine2());
        //StartCoroutine(TestCoroutine());
    }

    void Update___Follow____Sphere__ROBOT_TARGET()
    {
        var Sphere__ROBOT_TARGET = /*RobotSceneRoot.*/transform.Find("ROBOT_ORIGIN_AXIS_X0_Y0_Z0").Find("Sphere__ROBOT_TARGET");

        var X = Sphere__ROBOT_TARGET.gameObject.transform.localPosition;
        var S123456 = findClosestXYZ(X);
        var S1 = S123456[0];
        var S2 = S123456[1];
        var S3 = S123456[2];
        var S4 = S123456[3];
        var S5 = S123456[4];
        var S6 = S123456[5];

        Servo1 = S1;
        Servo2 = S2;
        Servo3 = S3;
        Servo4 = S4;
        Servo5 = S5;
        Servo6 = S6;
    }

    void Update___Follow____Sphere__ROBOT_TARGET__V2()
    {
        var Sphere__ROBOT_TARGET = transform.Find("ROBOT_ORIGIN_AXIS_X0_Y0_Z0").Find("Sphere__ROBOT_TARGET");

        //INPUT:
        var INPUT_XYZ = Sphere__ROBOT_TARGET.gameObject.transform.localPosition;
        var S1 = 1f;
        var S2 = Servo2;
        var S3 = Servo3;
        var S4 = Servo4;
        var S5 = Servo5;
        var S6 = Servo6;

        float out_S1, out_S2, out_S3, out_S4, out_S5, out_S6;
        Vector3 OUTPUT_XYZ = Vector3.zero;

        LeArm__mathematics.compute_XYZ_to_S123456(
                                       INPUT_XYZ,
                                       S1, S2, S3, S4, S5, S6,
                                       out OUTPUT_XYZ,
                                       out out_S1, out out_S2, out out_S3, out out_S4, out out_S5, out out_S6);

        //Servo1 = out_S1;
        //Servo2 = out_S2;
        Servo3 = out_S3;
        Servo4 = out_S4;
        Servo5 = out_S5;
        Servo6 = out_S6;
    }


    private void Update___AnimateVirtualServos()
    {
        float DELTA = MOTOR_SPEED;

        if (Mathf.Abs(Servo2 - realServo2) < DELTA) realServo2 = Servo2;
        else realServo2 += (Servo2 > realServo2 ? DELTA : -DELTA);

        if (Mathf.Abs(Servo3 - realServo3) < DELTA) realServo3 = Servo3;
        else realServo3 += (Servo3 > realServo3 ? DELTA : -DELTA);

        DELTA /= 1.3f;

        if (Mathf.Abs(Servo4 - realServo4) < DELTA) realServo4 = Servo4;
        else realServo4 += (Servo4 > realServo4 ? DELTA : -DELTA);

        DELTA /= 1.3f;

        if (Mathf.Abs(Servo5 - realServo5) < DELTA) realServo5 = Servo5;
        else realServo5 += (Servo5 > realServo5 ? DELTA : -DELTA);

        DELTA /= 1.3f;

        if (Mathf.Abs(Servo6 - realServo6) < DELTA) realServo6 = Servo6;
        else realServo6 += (Servo6 > realServo6 ? DELTA : -DELTA);



        realServo1 = Servo1;
        realServo2 = Servo2;
            /*
        realServo3 = Servo3;
        realServo4 = Servo4;
        realServo5 = Servo5;
        realServo6 = Servo6;
        */
    }

    void Update___LeArm_Virtual_Robot_Joints () {


        //
        // SERVO 3,4,5,6
        //
        Servo6_Hinge.gameObject.transform.localRotation = Quaternion.Euler(0, realServo6 * 180-180, 0); // Y
        Servo5_Hinge.gameObject.transform.localRotation = Quaternion.Euler(0, realServo5 * 180-90, 0); // Y
        Servo4_Hinge.gameObject.transform.localRotation = Quaternion.Euler(0, realServo4 * 180 - 90, 0); // Y
        Servo3_Hinge.gameObject.transform.localRotation = Quaternion.Euler(0, realServo3 * 180-90, 0); // Y



        //
        // SERVO 1
        //
        {

            var p0 = Servo1_L_Closed.transform.position;
            var p1 = Servo1_L_Open.transform.position;
            var r0 = Servo1_L_Closed.transform.localRotation;
            var r1 = Servo1_L_Open.transform.localRotation;
            var t = 1 - realServo1;
            Servo1_L_Animating.transform.position = p0 + (p1 - p0) * t;
            Servo1_L_Animating.transform.localRotation = Quaternion.Slerp(r0, r1, t);

            Servo1_L_Closed.GetComponent<MeshRenderer>().enabled = false;
            Servo1_L_Open.GetComponent<MeshRenderer>().enabled = false;
        }
        {

            var p0 = Servo1_R_Closed.transform.position;
            var p1 = Servo1_R_Open.transform.position;
            var r0 = Servo1_R_Closed.transform.localRotation;
            var r1 = Servo1_R_Open.transform.localRotation;
            var t = 1 - realServo1;
            Servo1_R_Animating.transform.position = p0 + (p1 - p0) * t;
            Servo1_R_Animating.transform.localRotation = Quaternion.Slerp(r0, r1, t);

            Servo1_R_Closed.GetComponent<MeshRenderer>().enabled = false;
            Servo1_R_Open.GetComponent<MeshRenderer>().enabled = false;
        }





        //
        //  SERVO 2
        //
        var LeapMotion_Integration = GetComponent<LeapMotion_n_LeArmVirtual_Integration>();
        if(LeapMotion_Integration.LeArm_ControlEnabled)
	{
		var I = LeapMotion_Integration.Index_bone3_Position;
		var T = LeapMotion_Integration.Thumb_bone3_Position;
		var Y = Vector3.up;
		var BA = (I - T).normalized;

		BA *= LeapMotion_Integration.LeftHand ? 1f : -1f;

		var cos_theta = Vector3.Dot(BA, Y);

		var cos_alpha0 = Vector3.Dot((Servo1_L_Open.transform.position - Servo1_R_Open.transform.position).normalized, Y);
		var diff_cos_angles = Mathf.Abs(cos_alpha0 - cos_theta);

		var NOISE_THRESHOLD = 0.077f;
		if (diff_cos_angles > NOISE_THRESHOLD)      // noise cancellation
		{
		    var original_Servo2 = Servo2;
		    var deltaServo2 = .03f;

		    Servo2_Hinge.gameObject.transform.localRotation = Quaternion.Euler(0, (1 - (Servo2 + deltaServo2)) * 180 - 90, 0); // Y
		    var cos_alpha1 = Vector3.Dot((Servo1_L_Open.transform.position - Servo1_R_Open.transform.position).normalized, Y);

		    Servo2_Hinge.gameObject.transform.localRotation = Quaternion.Euler(0, (1 - (Servo2 - deltaServo2)) * 180 - 90, 0); // Y
		    var cos_alpha2 = Vector3.Dot((Servo1_L_Open.transform.position - Servo1_R_Open.transform.position).normalized, Y);

		    Servo2 += (Mathf.Abs(cos_alpha1 - cos_theta) < Mathf.Abs(cos_alpha2 - cos_theta)) ? deltaServo2 : -deltaServo2;
		    if (Servo2 > 1f) Servo2 = 1f;
		    if (Servo2 < 0f) Servo2 = 0f;

		    Servo2_Hinge.gameObject.transform.localRotation = Quaternion.Euler(0, (1 - Servo2) * 180 - 90, 0); // Y
		}
	}



    }


    public Vector3 findClosestS123456(float s1, float s2, float s3, float s4, float s5, float s6)
    {
        float min_distSq = float.MaxValue;
        int index_min_distSq = -1;
        for (int i = 0; i < len; i++)
        {
            var S1 = S123456_S1p_S2up[i, 0];
            var S2 = S123456_S1p_S2up[i, 1];
            var S3 = S123456_S1p_S2up[i, 2];
            var S4 = S123456_S1p_S2up[i, 3];
            var S5 = S123456_S1p_S2up[i, 4];
            var S6 = S123456_S1p_S2up[i, 5];


            var distSq = /*(S1 - s1) * (S1 - s1)
                       + (S2 - s2) * (S2 - s2)
                       + */(S3 - s3) * (S3 - s3) * 1.5f     // WEIGHTS
                       + (S4 - s4) * (S4 - s4) * 1.3f     // WEIGHTS
                       + (S5 - s5) * (S5 - s5) * 1.0f     // WEIGHTS
                       + (S6 - s6) * (S6 - s6) * 0.8f;    // WEIGHTS;

            if (min_distSq > distSq)
            {
                min_distSq = distSq;
                index_min_distSq = i;
            }
        }

        return new Vector3(  S123456_S1p_S2up[index_min_distSq,6],
                             S123456_S1p_S2up[index_min_distSq,7],
                             S123456_S1p_S2up[index_min_distSq,8]  );
    }

    public float[] findClosestXYZ(Vector3 X)
    {
        float min_distSq = float.MaxValue;

        int index_min_distSq = -1;
        for (int i = 0; i < len; i++)
        {
            var S1p_x = S123456_S1p_S2up[i, 6];
            var S1p_y = S123456_S1p_S2up[i, 7];
            var S1p_z = S123456_S1p_S2up[i, 8];

            var distSq = (X.x - S1p_x) * (X.x - S1p_x) + (X.y - S1p_y) * (X.y - S1p_y) + (X.z - S1p_z) * (X.z - S1p_z);

            if (min_distSq > distSq)
            {
                min_distSq = distSq;
                index_min_distSq = i;
            }
        }

        return new float[] { S123456_S1p_S2up[index_min_distSq,0],
                             S123456_S1p_S2up[index_min_distSq,1],
                             S123456_S1p_S2up[index_min_distSq,2],
                             S123456_S1p_S2up[index_min_distSq,3],
                             S123456_S1p_S2up[index_min_distSq,4],
                             S123456_S1p_S2up[index_min_distSq,5]      };

    }

    public float[] findClosestXYZ(Vector3 X, float[] previousS123456)
    {
        //var pS1 = previousS123456[0];
        var pS2 = previousS123456[1];
        var pS3 = previousS123456[2];
        var pS4 = previousS123456[3];
        var pS5 = previousS123456[4];
        var pS6 = previousS123456[5];

        float min_distSq = float.MaxValue;
        //float[] min_distSq;
        int index_min_distSq = -1;
        for (int i = 0; i < len; i++)
        {
            var S1p_x = S123456_S1p_S2up[i, 6];
            var S1p_y = S123456_S1p_S2up[i, 7];
            var S1p_z = S123456_S1p_S2up[i, 8];

            var distSq1 = (X.x - S1p_x) * (X.x - S1p_x) + (X.y - S1p_y) * (X.y - S1p_y) + (X.z - S1p_z) * (X.z - S1p_z);

            //var nS1 = S123456_S1p_S2up[index_min_distSq, 0];
            var nS2 = S123456_S1p_S2up[index_min_distSq, 1];
            var nS3 = S123456_S1p_S2up[index_min_distSq, 2];
            var nS4 = S123456_S1p_S2up[index_min_distSq, 3];
            var nS5 = S123456_S1p_S2up[index_min_distSq, 4];
            var nS6 = S123456_S1p_S2up[index_min_distSq, 5];

            var distSq2 = (nS2 - pS2) * (nS2 - pS2)
                        + (nS3 - pS3) * (nS3 - pS3)
                        + (nS4 - pS4) * (nS4 - pS4)
                        + (nS5 - pS5) * (nS5 - pS5)
                        + (nS6 - pS6) * (nS6 - pS6);

            var distSq = distSq1 + distSq2;

            if (min_distSq > distSq)
            {
                min_distSq = distSq;
                index_min_distSq = i;
            }
        }

        return new float[] { S123456_S1p_S2up[index_min_distSq,0],
                             S123456_S1p_S2up[index_min_distSq,1],
                             S123456_S1p_S2up[index_min_distSq,2],
                             S123456_S1p_S2up[index_min_distSq,3],
                             S123456_S1p_S2up[index_min_distSq,4],
                             S123456_S1p_S2up[index_min_distSq,5]      };

    }





    private int len = LeArm___S123456_TO_XYZ_Lookup___V1.len;
    private float[,] S123456_S1p_S2up = LeArm___S123456_TO_XYZ_Lookup___V1.S123456_S1p_S2up;

    private void startTimer()
    {
        mTimerForServos = new System.Timers.Timer(TIMER_INTERVAL);
        mTimerForServos.Interval = TIMER_INTERVAL;
        mTimerForServos.AutoReset = true;
        mTimerForServos.Elapsed += onTimerElapse;
        mTimerForServos.Start();
    }

    private void stopTimer()
    {
        mTimerForServos.Stop();
        mTimerForServos.Dispose();
        mTimerForServos = null;
    }


    private void onTimerElapse(System.Object source, System.Timers.ElapsedEventArgs e)
    {
       if (scrSocketClient.isConnectedToServer())
        {
            if (strNameGameObj.Equals("LeArm_Virtual_Robot_prefab (1)"))
            {
                prepareAndSendToHw("L");
            }
            else if (strNameGameObj.Equals("LeArm_Virtual_Robot_prefab (2)"))
            {
                prepareAndSendToHw("R");
            }
        }
    }

    private void prepareAndSendToHw(string dev)
    {
        float servoTime = 1000.0f; // 1 second
        //keeping speed as constant default. We can also change that if required
        int servoID = 1;
        float servoPos = 1500;
        string cmd = "";
        bool result = false;

        //servo 1 in the range 1500-2500
        servoPos = 1500 + (Servo1 * 1000);
        {
            servoID = 1;
            cmd = dev + "," + servoID + "," + servoPos + "," + servoTime;
            result = scrSocketClient.sendToServer(cmd);
            Debug.Log("sent for 1? " + result);
        }


        //All other servos in the range 500-2500

        //servo 2
        servoPos = 500 + (Servo2 * 2000);
        {
            servoID = 2;
            cmd = dev + "," + servoID + "," + servoPos + "," + servoTime;
            result = scrSocketClient.sendToServer(cmd);
            Debug.Log("sent for 2? " + result);
        }

        //servo 3
        servoPos = 500 + (Servo3 * 2000);
        {
            servoID = 3;
            cmd = dev + "," + servoID + "," + servoPos + "," + servoTime;
            result = scrSocketClient.sendToServer(cmd);
            Debug.Log("sent for 3? " + result);
        }


        //TODO ; these rotate in the reverse direction
        servoPos = 500 + (Servo4 * 2000);
        {
            servoID = 4;
            cmd = dev + "," + servoID + "," + servoPos + "," + servoTime;
            result = scrSocketClient.sendToServer(cmd);
            Debug.Log("sent for 4? " + result);
        }


        servoPos = 500 + ((1 - Servo5) * 2000);
        {
            servoID = 5;
            cmd = dev + "," + servoID + "," + servoPos + "," + servoTime;
            result = scrSocketClient.sendToServer(cmd);
            Debug.Log("sent for 5? " + result);
        }


        servoPos = 500 + ((1 - Servo6) * 2000);
        {
            servoID = 6;
            cmd = dev + "," + servoID + "," + servoPos + "," + servoTime;
            result = scrSocketClient.sendToServer(cmd);
            Debug.Log("sent for 6? " + result);
        }

    }

    public void OnApplicationQuit()
    {
        stopTimer();
    }


    // Update is called once per frame
    void Update()
    {
        if (FollowRobotTarget)
            Update___Follow____Sphere__ROBOT_TARGET();

        if (FollowRobotTarget__V2)
            Update___Follow____Sphere__ROBOT_TARGET__V2();

        Update___LeArm_Virtual_Robot_Joints();
        Update___AnimateVirtualServos();




        
    }


    IEnumerator TestCoroutine2()
    {
        string fileName = @"C:\tmp\LeArm_data__LeapMotion___R2.txt";

        StreamWriter sw = null;
        try
        {
            sw = File.CreateText(fileName);
            //    sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
            //    sw.WriteLine("Author: Mahesh Chand");
        }
        catch (Exception Ex)
        {
            Debug.Log(Ex.ToString());
        }

        yield return new WaitForSeconds(5f);

        while (true)
        {
            yield return new WaitForSeconds(0f);

            var instance_LeapMotion_n_LeArmVirtual_Integration = GetComponent<LeapMotion_n_LeArmVirtual_Integration>();

            var I = instance_LeapMotion_n_LeArmVirtual_Integration.Index_bone3_Position;
            var T = instance_LeapMotion_n_LeArmVirtual_Integration.Thumb_bone3_Position;


            var str = "";

            str += I.x + "," + I.y + "," + I.z + ",";
            str += T.x + "," + T.y + "," + T.z + ",";

            sw.WriteLine(str);
        }

        sw.Close();
    }

    //…
    IEnumerator TestCoroutine()
    {
        string fileName = @"C:\tmp\LeArm_data__servo2__R2.txt";

        StreamWriter sw = null;
        try
        {
            sw = File.CreateText(fileName);
            //    sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
            //    sw.WriteLine("Author: Mahesh Chand");
        }
        catch (Exception Ex)
        {
            Debug.Log(Ex.ToString());
        }

        yield return new WaitForSeconds(5f);

        var L1_BASE = GameObject.Find("L1_BASE");
        var L2_BASE = GameObject.Find("L2_BASE");
        var L3_BASE = GameObject.Find("L3_BASE");
        var L4_BASE = GameObject.Find("L4_BASE");
        var L5_BASE = GameObject.Find("L5_BASE");

        var Gripper__LEFT_RUBBER_TIP = GameObject.Find("Gripper__LEFT_RUBBER_TIP");
        var Gripper__RIGHT_RUBBER_TIP = GameObject.Find("Gripper__RIGHT_RUBBER_TIP");

        var delta = .0499f;
        for (float s2 = 0; s2 <= 1; s2 += delta)
                        {
                            //Servo1 = 1;
                            Servo2 = s2;
                            yield return new WaitForSeconds(0f);

                            var S1_TIP1 = Gripper__LEFT_RUBBER_TIP.gameObject.transform;
                            var S1_TIP2 = Gripper__RIGHT_RUBBER_TIP.gameObject.transform;
                            var S2_BASE = L5_BASE.gameObject.transform;
                            var S3_BASE = L4_BASE.gameObject.transform;
                            var S4_BASE = L3_BASE.gameObject.transform;
                            var S5_BASE = L2_BASE.gameObject.transform;
                            var S6_BASE = L1_BASE.gameObject.transform;



                            var S1 = Servo1;
                            var S2 = Servo2;
                            var S3 = Servo3;
                            var S4 = Servo4;
                            var S5 = Servo5;
                            var S6 = Servo6;



                            var str = "";
                            str += S1 + "," + S2 + "," + S3 + "," + S4 + "," + S5 + "," + S6;

                            str += ", " + S1_TIP1.position.x + ", " + S1_TIP1.position.y + ", " + S1_TIP1.position.z;
                            str += ", " + S1_TIP1.rotation.w + ", " + S1_TIP1.rotation.x + ", " + S1_TIP1.rotation.y + ", " + S1_TIP1.rotation.z;
                            str += ", " + S1_TIP1.right.x + ", " + S1_TIP1.right.y + ", " + S1_TIP1.right.z;
                            str += ", " + S1_TIP1.up.x + ", " + S1_TIP1.up.y + ", " + S1_TIP1.up.z;
                            str += ", " + S1_TIP1.forward.x + ", " + S1_TIP1.forward.y + ", " + S1_TIP1.forward.z;

                            str += ", " + S1_TIP2.position.x + ", " + S1_TIP2.position.y + ", " + S1_TIP2.position.z;
                            str += ", " + S1_TIP2.rotation.w + ", " + S1_TIP2.rotation.x + ", " + S1_TIP2.rotation.y + ", " + S1_TIP2.rotation.z;
                            str += ", " + S1_TIP2.right.x + ", " + S1_TIP2.right.y + ", " + S1_TIP2.right.z;
                            str += ", " + S1_TIP2.up.x + ", " + S1_TIP2.up.y + ", " + S1_TIP2.up.z;
                            str += ", " + S1_TIP2.forward.x + ", " + S1_TIP2.forward.y + ", " + S1_TIP2.forward.z;

                            str += ", " + S2_BASE.position.x + ", " + S2_BASE.position.y + ", " + S2_BASE.position.z;
                            str += ", " + S2_BASE.rotation.w + ", " + S2_BASE.rotation.x + ", " + S2_BASE.rotation.y + ", " + S2_BASE.rotation.z;
                            str += ", " + S2_BASE.right.x + ", " + S2_BASE.right.y + ", " + S2_BASE.right.z;
                            str += ", " + S2_BASE.up.x + ", " + S2_BASE.up.y + ", " + S2_BASE.up.z;
                            str += ", " + S2_BASE.forward.x + ", " + S2_BASE.forward.y + ", " + S2_BASE.forward.z;

                            


                            //str += "\n";
                            sw.WriteLine(str);
                        }


        sw.Close();
    }

}


