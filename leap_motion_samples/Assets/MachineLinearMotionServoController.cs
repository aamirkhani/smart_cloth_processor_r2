using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineLinearMotionServoController : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float Servo100_XAxis_Channe1;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float Servo101_XAxis_Channe2;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float Servo102_ZAxis_Channe1;

    [Range(0.0f, 1.0f)]
    [SerializeField]
    public float Servo103_ZAxis_Channe2;

    public Vector3 DEBUG_zaxis_slider_max_shift;
    public Vector3 DEBUG_xaxis_slider_max_shift;

    [SerializeField]
    public GameObject TARGET_CHANNEL_A;

    [SerializeField]
    public GameObject TARGET_CHANNEL_B;

    [SerializeField]
    public bool Follow_TARGET_CHANNEL_A;

    [SerializeField]
    public bool Follow_TARGET_CHANNEL_B;


    private MachineDesign__S123456_100_102___ROBOT_POSE_MATHEMATICS math;
    private GameObject LeArm_Virtual_Robot_prefab_A;

    // Use this for initialization
    void Start () {
        math = this.GetComponent<MachineDesign__S123456_100_102___ROBOT_POSE_MATHEMATICS>();
        LeArm_Virtual_Robot_prefab_A = GameObject.Find("LeArm_Virtual_Robot_prefab_A");
    }
	
	// Update is called once per frame
	void Update () {

        Update___GameObjectTransformstions___using___S100_S101_S102_S103();
        if (Follow_TARGET_CHANNEL_A)
            Update___Follow_TARGET_CHANNEL_Aor1();
    }


    void Update___GameObjectTransformstions___using___S100_S101_S102_S103()
    {
        // X AXIS

        //float xaxis_slider_max_shift = GameObject.Find("(static) SPACE_MARKER (maximum translation marker)").gameObject.transform.localPosition.magnitude;
        //GameObject.Find("(slidable) SPACE_MARKER (SliderX_A holder)").gameObject.transform.localPosition = new Vector3(Servo100_XAxis_Channe1 * xaxis_slider_max_shift, 0, 0);
        //GameObject.Find("(slidable) SPACE_MARKER (SliderX_B holder)").gameObject.transform.localPosition = new Vector3(Servo101_XAxis_Channe2 * xaxis_slider_max_shift, 0, 0);

        var xaxis_slider_width = GameObject.Find("(static) SPACE_MARKER (slider-width marker)").gameObject.transform.localPosition.magnitude;
        Vector3 xaxis_slider_max_shift = GameObject.Find("(static) SPACE_MARKER (maximum translation marker)").gameObject.transform.localPosition;
        DEBUG_xaxis_slider_max_shift = xaxis_slider_max_shift;
        GameObject.Find("(slidable) SPACE_MARKER (SliderX_A holder)").gameObject.transform.localPosition = xaxis_slider_max_shift * Servo100_XAxis_Channe1;
        GameObject.Find("(slidable) SPACE_MARKER (SliderX_B holder)").gameObject.transform.localPosition = xaxis_slider_max_shift * Servo101_XAxis_Channe2;


        // Z AXIS
        Vector3 zaxis_slider_max_shift = GameObject.Find("(static) SPACE_MARKER (maximum translation marker) Z-Axis").gameObject.transform.localPosition;
        DEBUG_zaxis_slider_max_shift = zaxis_slider_max_shift;
        GameObject.Find("(slidable) SPACE_MARKER (SliderZ_A holder)").gameObject.transform.localPosition = zaxis_slider_max_shift * Servo102_ZAxis_Channe1;
        GameObject.Find("(slidable) SPACE_MARKER (SliderZ_B holder)").gameObject.transform.localPosition = zaxis_slider_max_shift * Servo103_ZAxis_Channe2;
    }

    void Update___Follow_TARGET_CHANNEL_Aor1()
    {
        var INPUT_XYZ = TARGET_CHANNEL_A.gameObject.transform.position;
        var INPUT_vxaxis = TARGET_CHANNEL_A.gameObject.transform.right; // x axis
        var INPUT_vyaxis = TARGET_CHANNEL_A.gameObject.transform.up; // y axis
        var INPUT_vzaxis = TARGET_CHANNEL_A.gameObject.transform.forward; // y axis

        var learm_motors_to_gripper_pose = LeArm_Virtual_Robot_prefab_A.GetComponent<LeArm__MOTORS_TO_GRIPPER_POSE>();

        var S1 = learm_motors_to_gripper_pose.Servo1;
        var S2 = learm_motors_to_gripper_pose.Servo2;
        var S3 = learm_motors_to_gripper_pose.Servo3;
        var S4 = learm_motors_to_gripper_pose.Servo4;
        var S5 = learm_motors_to_gripper_pose.Servo5;
        var S6 = learm_motors_to_gripper_pose.Servo6;
        var S100 = this.Servo100_XAxis_Channe1;
        var S102 = this.Servo102_ZAxis_Channe1;


        float out_S1, out_S2, out_S3, out_S4, out_S5, out_S6, out_S100, out_S102;
        Vector3 OUTPUT_XYZ = new Vector3(0, 0, 0);
        Vector3 OUTPUT_vxaxis = new Vector3(0, 0, 0);
        Vector3 OUTPUT_vyaxis = new Vector3(0, 0, 0);
        Vector3 OUTPUT_vzaxis = new Vector3(0, 0, 0);

        math.compute__XYZnXYZAxis__to__S123456_100_102(
                                       INPUT_XYZ,
                                       INPUT_vxaxis,
                                       INPUT_vyaxis,
                                       INPUT_vzaxis,
                                       S1, S2, S3, S4, S5, S6, S100, S102,
                                       out OUTPUT_XYZ,
                                       out OUTPUT_vxaxis,
                                       out OUTPUT_vyaxis,
                                       out OUTPUT_vzaxis,
                                       out out_S1, out out_S2, out out_S3, out out_S4, out out_S5, out out_S6, out out_S100, out out_S102);

        learm_motors_to_gripper_pose.Servo1 = out_S1;
        learm_motors_to_gripper_pose.Servo2 = out_S2;
        learm_motors_to_gripper_pose.Servo3 = out_S3;
        learm_motors_to_gripper_pose.Servo4 = out_S4;
        learm_motors_to_gripper_pose.Servo5 = out_S5;
        learm_motors_to_gripper_pose.Servo6 = out_S6;
        this.Servo100_XAxis_Channe1 = out_S100;
        this.Servo102_ZAxis_Channe1 = out_S102;

        learm_motors_to_gripper_pose.realServo1 = out_S1;
        learm_motors_to_gripper_pose.realServo2 = out_S2;
        learm_motors_to_gripper_pose.realServo3 = out_S3;
        learm_motors_to_gripper_pose.realServo4 = out_S4;
        learm_motors_to_gripper_pose.realServo5 = out_S5;
        learm_motors_to_gripper_pose.realServo6 = out_S6;

    }

}
