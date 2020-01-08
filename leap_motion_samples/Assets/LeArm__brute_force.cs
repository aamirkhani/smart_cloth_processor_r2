using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LeArm__brute_force : MonoBehaviour {

    private LeArm__MOTORS_TO_GRIPPER_POSE LeArm;

    // Use this for initialization
    void Start () {
        LeArm = this.GetComponent<LeArm__MOTORS_TO_GRIPPER_POSE>();
        StartCoroutine(TestCoroutine());
    }
	
	// Update is called once per frame
	void Update () {
    }

    
    //…
    IEnumerator TestCoroutine()
    {
        string fileName = @"C:\tmp\LeArm_data__R13_delta__0.08__.txt";
        var delta = 0.08f;

        StreamWriter sw = null;
        try {
            sw = File.CreateText(fileName);
            //    sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
            //    sw.WriteLine("Author: Mahesh Chand");
        } catch (Exception Ex) {
            Debug.Log(Ex.ToString());
        }

        var L1_BASE = /*LeArm.RobotSceneRoot.*/transform.Find("L1_BASE");
        var L2_BASE = /*LeArm.RobotSceneRoot.*/transform.Find("L2_BASE");
        var L3_BASE = /*LeArm.RobotSceneRoot.*/transform.Find("L3_BASE");
        var L4_BASE = /*LeArm.RobotSceneRoot.*/transform.Find("L4_BASE");
        var L5_BASE = /*LeArm.RobotSceneRoot.*/transform.Find("L5_BASE");
       
        var Gripper__LEFT_RUBBER_TIP = /*LeArm.RobotSceneRoot.*/transform.Find("Gripper__LEFT_RUBBER_TIP");
        var Gripper__RIGHT_RUBBER_TIP = /*LeArm.RobotSceneRoot.*/transform.Find("Gripper__RIGHT_RUBBER_TIP");

        //for (float s1 = 0; s1 <= 1; s1 += delta)
               float s1 = 1; 
            for (float s2 = 0; s2 <= 1; s2 += delta)
                for (float s3 = 0; s3 <= 1; s3 += delta)
                    for (float s4 = 0; s4 <= 1; s4 += delta)
                        for (float s5 = 0; s5 <= 1; s5 += delta/2)
                            for (float s6 = 0; s6 <= 1; s6 += delta/5)
                            {
                                LeArm.Servo1 = s1;
                                LeArm.Servo2 = s2;
                                LeArm.Servo3 = s3;
                                LeArm.Servo4 = s4;
                                LeArm.Servo5 = s5;
                                LeArm.Servo6 = s6;
                                yield return new WaitForSeconds(0f);

                                var S1_TIP1 = Gripper__LEFT_RUBBER_TIP.gameObject.transform;
                                var S1_TIP2 = Gripper__RIGHT_RUBBER_TIP.gameObject.transform;
                                var S2_BASE = L5_BASE.gameObject.transform;
                                var S3_BASE = L4_BASE.gameObject.transform;
                                var S4_BASE = L3_BASE.gameObject.transform;
                                var S5_BASE = L2_BASE.gameObject.transform;
                                var S6_BASE = L1_BASE.gameObject.transform;
                                

                                
                                var S1 = LeArm.Servo1;
                                var S2 = LeArm.Servo2;
                                var S3 = LeArm.Servo3;
                                var S4 = LeArm.Servo4;
                                var S5 = LeArm.Servo5;
                                var S6 = LeArm.Servo6;



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

                                str += ", " + S3_BASE.position.x + ", " + S3_BASE.position.y + ", " + S3_BASE.position.z;
                                str += ", " + S3_BASE.rotation.w + ", " + S3_BASE.rotation.x + ", " + S3_BASE.rotation.y + ", " + S3_BASE.rotation.z;
                                str += ", " + S3_BASE.right.x + ", " + S3_BASE.right.y + ", " + S3_BASE.right.z;
                                str += ", " + S3_BASE.up.x + ", " + S3_BASE.up.y + ", " + S3_BASE.up.z;
                                str += ", " + S3_BASE.forward.x + ", " + S3_BASE.forward.y + ", " + S3_BASE.forward.z;

                                str += ", " + S4_BASE.position.x + ", " + S4_BASE.position.y + ", " + S4_BASE.position.z;
                                str += ", " + S4_BASE.rotation.w + ", " + S4_BASE.rotation.x + ", " + S4_BASE.rotation.y + ", " + S4_BASE.rotation.z;
                                str += ", " + S4_BASE.right.x + ", " + S4_BASE.right.y + ", " + S4_BASE.right.z;
                                str += ", " + S4_BASE.up.x + ", " + S4_BASE.up.y + ", " + S4_BASE.up.z;
                                str += ", " + S4_BASE.forward.x + ", " + S4_BASE.forward.y + ", " + S4_BASE.forward.z;

                                str += ", " + S5_BASE.position.x + ", " + S5_BASE.position.y + ", " + S5_BASE.position.z;
                                str += ", " + S5_BASE.rotation.w + ", " + S5_BASE.rotation.x + ", " + S5_BASE.rotation.y + ", " + S5_BASE.rotation.z;
                                str += ", " + S5_BASE.right.x + ", " + S5_BASE.right.y + ", " + S5_BASE.right.z;
                                str += ", " + S5_BASE.up.x + ", " + S5_BASE.up.y + ", " + S5_BASE.up.z;
                                str += ", " + S5_BASE.forward.x + ", " + S5_BASE.forward.y + ", " + S5_BASE.forward.z;

                                str += ", " + S6_BASE.position.x + ", " + S6_BASE.position.y + ", " + S6_BASE.position.z;
                                str += ", " + S6_BASE.rotation.w + ", " + S6_BASE.rotation.x + ", " + S6_BASE.rotation.y + ", " + S6_BASE.rotation.z;
                                str += ", " + S6_BASE.right.x + ", " + S6_BASE.right.y + ", " + S6_BASE.right.z;
                                str += ", " + S6_BASE.up.x + ", " + S6_BASE.up.y + ", " + S6_BASE.up.z;
                                str += ", " + S6_BASE.forward.x + ", " + S6_BASE.forward.y + ", " + S6_BASE.forward.z;

                                //str += "\n";
                                sw.WriteLine(str);
                            }


        sw.Close();
    }
}


