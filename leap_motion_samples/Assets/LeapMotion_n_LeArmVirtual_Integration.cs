using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapMotion_n_LeArmVirtual_Integration : MonoBehaviour {

    public string LeapMotionAttachedObject;
    [SerializeField]
    public bool LeArm_ControlEnabled = false;
    private bool last__LeArm_ControlEnabled;

    public bool LeapMotionDetectedHand;

    [SerializeField]
    public bool LeftHand;

    public Vector3 Palm_Position;
    public Quaternion Palm_Rosition;
    public Vector3 Thumb_bone3_Position;
    public Quaternion Thumb_bone3_Rosition;

    public Vector3 Index_bone3_Position;
    public Quaternion Index_bone3_Rosition;

    public Vector3 Middle_bone3_Position;
    public Quaternion Middle_bone3_Rosition;

    public Vector3 Ring_bone3_Position;
    public Quaternion Ring_bone3_Rosition;

    public Vector3 Pinky_bone3_Position;
    public Quaternion Pinky_bone3_Rosition;



    private LeArm__MOTORS_TO_GRIPPER_POSE LeArm;
    private LeArm___S123456_TO__ROBOT_POSE_MATHEMATICS LeArm__mathematics;
    private GameObject Gripper__RIGHT_RUBBER_TIP;
    private GameObject Sphere__ROBOT_TARGET;


    // Use this for initialization
    void Start () {
        LeArm = this.GetComponent<LeArm__MOTORS_TO_GRIPPER_POSE>();
        LeArm__mathematics = GetComponent<LeArm___S123456_TO__ROBOT_POSE_MATHEMATICS>();

        Gripper__RIGHT_RUBBER_TIP = FindDeepChild(this.transform, "Gripper__RIGHT_RUBBER_TIP").gameObject;
        Sphere__ROBOT_TARGET = transform.Find("ROBOT_ORIGIN_AXIS_X0_Y0_Z0").transform.Find("Sphere__ROBOT_TARGET").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        var RigidRoundHand_L_or_R = GameObject.Find(LeftHand ? "RigidRoundHand_L" : "RigidRoundHand_R");
        var RigidRoundHand_L_or_R_Clone = GameObject.Find(LeftHand ? "RigidRoundHand_L(Clone)":"RigidRoundHand_R(Clone)");

        //if (RigidRoundHand_L_or_R != null && RigidRoundHand_L_or_R.activeSelf == false)
         //   RigidRoundHand_L_or_R = RigidRoundHand_L_or_R_Clone;


        if (RigidRoundHand_L_or_R != null && RigidRoundHand_L_or_R.activeSelf == true)
        {
            LeapMotionAttachedObject = RigidRoundHand_L_or_R.gameObject.name;
            var palm = RigidRoundHand_L_or_R.transform.Find("palm");
            Palm_Position = palm.position;
            Palm_Rosition = palm.rotation;

            var thumb = RigidRoundHand_L_or_R.transform.Find("thumb");
            var bone3 = thumb.Find("bone3");
            Thumb_bone3_Position = bone3.position;
            Thumb_bone3_Rosition = bone3.rotation;

            var index = RigidRoundHand_L_or_R.transform.Find("index");
            bone3 = index.gameObject.transform.Find("bone3");
            Index_bone3_Position = bone3.position;
            Index_bone3_Rosition = bone3.rotation;

            var middle = RigidRoundHand_L_or_R.transform.Find("middle");
            bone3 = middle.gameObject.transform.Find("bone3");
            Middle_bone3_Position = bone3.position;
            Middle_bone3_Rosition = bone3.rotation;

            LeapMotionDetectedHand = true;
        }
        else if (RigidRoundHand_L_or_R_Clone != null && RigidRoundHand_L_or_R_Clone.activeSelf == true)
        {
            LeapMotionAttachedObject = RigidRoundHand_L_or_R_Clone.gameObject.name;
            var palm = RigidRoundHand_L_or_R_Clone.transform.Find("palm");
            Palm_Position = palm.position;
            Palm_Rosition = palm.rotation;

            var thumb = RigidRoundHand_L_or_R_Clone.transform.Find("thumb");
            var bone3 = thumb.Find("bone3");
            Thumb_bone3_Position = bone3.position;
            Thumb_bone3_Rosition = bone3.rotation;

            var index = RigidRoundHand_L_or_R_Clone.transform.Find("index");
            bone3 = index.gameObject.transform.Find("bone3");
            Index_bone3_Position = bone3.position;
            Index_bone3_Rosition = bone3.rotation;

            var middle = RigidRoundHand_L_or_R_Clone.transform.Find("middle");
            bone3 = middle.gameObject.transform.Find("bone3");
            Middle_bone3_Position = bone3.position;
            Middle_bone3_Rosition = bone3.rotation;

            LeapMotionDetectedHand = true;
        } else
        {
            LeapMotionDetectedHand = false;
        }

        Update___LeArm_VirtualRobot();
        last__LeArm_ControlEnabled = LeArm_ControlEnabled;

    }

    public float DistanceBetweenIndexFinderAndThumb;
    public float d;
    public bool IndexFingerTouchingThumb = false;

    public float DistanceBetweenMiddleFinderAndThumb;
    public float d2;
    public bool MiddleFingerTouchingThumb = false;

    public float DistanceBetweenRingFinderAndThumb;
    public float d3;
    public bool RingFingerTouchingThumb = false;

    public float DistanceBetweenPinkyFinderAndThumb;
    public float d4;
    public bool PinkyFingerTouchingThumb = false;

    public bool HandClosed = false;

    public bool last_HandClosed { get; private set; }

    public float distancePalmMoved;

    public Vector3 startingPositionOfPalm;
    public Vector3 startingPositionOfLeArm;
    public Vector3 LeArm_XYZ;
    private void Update___LeArm_VirtualRobot()
    {
        if (!LeapMotionDetectedHand /*&& LeArm_ControlEnabled)*/) return;

        DistanceBetweenIndexFinderAndThumb = (Thumb_bone3_Position - Index_bone3_Position).magnitude;
        DistanceBetweenMiddleFinderAndThumb = (Thumb_bone3_Position - Middle_bone3_Position).magnitude;
        DistanceBetweenRingFinderAndThumb = (Thumb_bone3_Position - Ring_bone3_Position).magnitude;
        DistanceBetweenPinkyFinderAndThumb = (Thumb_bone3_Position - Pinky_bone3_Position).magnitude;

        var DIST_MAX = 0.08f;
        var DIST_MIN = 0.02f;

        d = (DistanceBetweenIndexFinderAndThumb - DIST_MIN) / (DIST_MAX - DIST_MIN);
        if (d > 1) d = 1;
        if (d < 0) d = 0;
        d = 1 - d;
	    IndexFingerTouchingThumb = d>.75f;

        d2 = (DistanceBetweenMiddleFinderAndThumb - DIST_MIN) / (DIST_MAX - DIST_MIN);
        if (d2 > 1) d2 = 1;
        if (d2 < 0) d2 = 0;
        d2 = 1 - d2;
	    MiddleFingerTouchingThumb = d2> .75f;

        d3 = (DistanceBetweenRingFinderAndThumb - DIST_MIN) / (DIST_MAX - DIST_MIN);
        if (d3 > 1) d3 = 1;
        if (d3 < 0) d3 = 0;
        d3 = 1 - d3;
	    RingFingerTouchingThumb = d3> .75f;

        d4 = (DistanceBetweenPinkyFinderAndThumb - DIST_MIN) / (DIST_MAX - DIST_MIN);
        if (d4 > 1) d4 = 1;
        if (d4 < 0) d4 = 0;
        d4 = 1 - d4;
	    PinkyFingerTouchingThumb = d4> .75f;

        // approximation
        HandClosed = MiddleFingerTouchingThumb & IndexFingerTouchingThumb;

        if (HandClosed == false && last_HandClosed)
        {
            // Trigger Switch
            LeArm_ControlEnabled = !LeArm_ControlEnabled;
        }
        last_HandClosed = HandClosed;


        if (!(LeapMotionDetectedHand && LeArm_ControlEnabled)) return;

        LeArm.Servo1 = d;



        ////////////////////////////
        if (LeArm_ControlEnabled==true && last__LeArm_ControlEnabled==false)
        {
            //startingPositionOfLeArm = LeArm.findClosestS123456(/*LeArm.Servo1*/1, LeArm.Servo2, LeArm.Servo3, LeArm.Servo4, LeArm.Servo5, LeArm.Servo6);
            Sphere__ROBOT_TARGET.transform.position = Gripper__RIGHT_RUBBER_TIP.transform.position;
            startingPositionOfLeArm = Sphere__ROBOT_TARGET.transform.localPosition;

            startingPositionOfPalm = Palm_Position;
            last__LeArm_ControlEnabled = true;
            return;
        }
         
        distancePalmMoved = (startingPositionOfPalm - Palm_Position).magnitude;
        

        LeArm_XYZ = startingPositionOfLeArm + (Palm_Position - startingPositionOfPalm) * 1000.0f;

        Sphere__ROBOT_TARGET.gameObject.transform.localPosition = LeArm_XYZ;

        LeArm.FollowRobotTarget__V2 = true;

        /*
        var S123456 = LeArm.findClosestXYZ(LeArm_XYZ);
        //LeArm.Servo1 = S123456[0];
        //LeArm.Servo2 = S123456[1];
        LeArm.Servo3 = S123456[2];
        LeArm.Servo4 = S123456[3];
        LeArm.Servo5 = S123456[4];
        LeArm.Servo6 = S123456[5];
        */

        /*
        //INPUT:
        var INPUT_XYZ = Sphere__ROBOT_TARGET.gameObject.transform.localPosition;
        var S1 = 1f;
        var S2 = LeArm.Servo2;
        var S3 = LeArm.Servo3;
        var S4 = LeArm.Servo4;
        var S5 = LeArm.Servo5;
        var S6 = LeArm.Servo6;

        float out_S1, out_S2, out_S3, out_S4, out_S5, out_S6;
        Vector3 OUTPUT_XYZ = Vector3.zero;

        LeArm__mathematics.compute_XYZ_to_S123456(
                                       INPUT_XYZ,
                                       S1, S2, S3, S4, S5, S6,
                                       out OUTPUT_XYZ,
                                       out out_S1, out out_S2, out out_S3, out out_S4, out out_S5, out out_S6);

        LeArm.Servo1 = out_S1;
        LeArm.Servo2 = out_S2;
        LeArm.Servo3 = out_S3;
        LeArm.Servo4 = out_S4;
        LeArm.Servo5 = out_S5;
        LeArm.Servo6 = out_S6;
        */

    }

    //True Breadth-first search
    private Transform FindDeepChild(Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();

        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }

}
