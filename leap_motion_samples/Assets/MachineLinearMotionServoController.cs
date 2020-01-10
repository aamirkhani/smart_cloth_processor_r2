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


    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        // X AXIS
        var xaxis_slider_width = GameObject.Find("(static) SPACE_MARKER (slider-width marker)").gameObject.transform.localPosition.magnitude;
        var xaxis_slider_max_shift = GameObject.Find("(static) SPACE_MARKER (maximum translation marker)").gameObject.transform.localPosition.magnitude;
        GameObject.Find("(slidable) SPACE_MARKER (SliderX_A holder)").gameObject.transform.localPosition = new Vector3(Servo100_XAxis_Channe1 * xaxis_slider_max_shift, 0, 0);
        GameObject.Find("(slidable) SPACE_MARKER (SliderX_B holder)").gameObject.transform.localPosition = new Vector3(Servo101_XAxis_Channe2 * xaxis_slider_max_shift, 0, 0);


        // Z AXIS
        var zaxis_slider_max_shift = GameObject.Find("(static) SPACE_MARKER (maximum translation marker) Z-Axis").gameObject.transform.localPosition;
        GameObject.Find("(slidable) SPACE_MARKER (SliderZ_A holder)").gameObject.transform.localPosition = zaxis_slider_max_shift * Servo102_ZAxis_Channe1;
        GameObject.Find("(slidable) SPACE_MARKER (SliderZ_B holder)").gameObject.transform.localPosition = zaxis_slider_max_shift * Servo103_ZAxis_Channe2;
        






    }
}
