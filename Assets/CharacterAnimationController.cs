using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Android;

public class CharacterAnimationController : MonoBehaviour
{
    [Serializable]
    public class FrameData
    {
        public string right_knee;
        public string left_knee;
        public string right_elbow;
        public string left_elbow;
        public string right_shoulder;
        public string left_shoulder;
    }

    [Serializable]
    public class FrameDataList
    {
        public List<FrameData> frames;
    }

    public Transform kneeJoint;
    public Transform elbowJoint;
    public Transform shoulderJoint;
    public Transform hipJoint;
    public Transform ankleJoint;

    public Renderer Knee;
    public Renderer Elbow;
    public Renderer Shoulder;
    public Renderer Hip;
    public Renderer Ankle;

    public float KneeValue;
    public float ElbowValue;
    public float ShoulderValue;
    public float otherAngles;

    public float KneeIdealAngle;
    public float ElbowIdealAngle;
    public float ShoulderIdealAngle;
    public float HipIdealAngle;
    public float AnkleIdealAngle;

    public bool animateLeftSide; // True for left side, False for right side
    public bool Screen2;

    private List<FrameData> frameDataList;
    private Dictionary<Transform, Vector3> initialRotations;
    public int currentFrame = 0;
    private string path = "C:/python";

    void Awake()
    {

        // Capture the initial rotations of the joints
        initialRotations = new Dictionary<Transform, Vector3> {
            { kneeJoint, kneeJoint.localEulerAngles },
            { elbowJoint, elbowJoint.localEulerAngles },
            { shoulderJoint, shoulderJoint.localEulerAngles },
            { hipJoint, hipJoint.localEulerAngles },
            { ankleJoint, ankleJoint.localEulerAngles }
        };
    }

    void Update()
    {
        string filePath = Path.Combine(path, "angles.json");
        if (!File.Exists(filePath))
        {
            Debug.LogError("JSON file not found at " + filePath);
            return;
        }

        if (Screen2)
        {
            currentFrame = 1;
        }
        else { currentFrame = 0; }

        string dataAsJson = File.ReadAllText(filePath);
        FrameDataList dataList = JsonUtility.FromJson<FrameDataList>("{\"frames\":" + dataAsJson + "}");
        frameDataList = dataList.frames;

        if (frameDataList == null || frameDataList.Count == 0) return;

        FrameData frameData = frameDataList[currentFrame];
        ApplyAngles(frameData);
        ColorJointsFeedback();
    }

    void ApplyAngles(FrameData data)
    {
        string kneeData = animateLeftSide ? data.left_knee : data.right_knee;
        string elbowData = animateLeftSide ? data.left_elbow : data.right_elbow;
        string shoulderData = animateLeftSide ? data.left_shoulder : data.right_shoulder;

        RotateKneeJoint(kneeJoint, kneeData);
        RotateJointElbow(elbowJoint, elbowData);
        RotateShoulderJoint(shoulderJoint, shoulderData);

        CalculateAndRotateHipAndAnkle(hipJoint, ankleJoint, kneeData);
    }

    void RotateJointElbow(Transform joint, string angleStr)
    {
        if (joint != null && angleStr != "Not visible" && initialRotations.ContainsKey(joint))
        {
            if (float.TryParse(angleStr, out ElbowValue))
            {
                ElbowValue = 180 - ElbowValue;
                joint.localEulerAngles = initialRotations[joint] + new Vector3(0, 0, ElbowValue);
            }
        }
    }

    void RotateKneeJoint(Transform joint, string angleStr)
    {
        if (joint != null && angleStr != "Not visible" && initialRotations.ContainsKey(joint))
        {
            if (float.TryParse(angleStr, out KneeValue))
            {
                joint.localEulerAngles = initialRotations[joint] + new Vector3(0, 0, (180 - KneeValue));
            }
        }
    }

    void RotateShoulderJoint(Transform joint, string angleStr)
    {
        if (joint != null && angleStr != "Not visible" && initialRotations.ContainsKey(joint))
        {
            if (float.TryParse(angleStr, out ShoulderValue))
            {
                joint.localEulerAngles = initialRotations[joint] + new Vector3(ShoulderValue, 0, 0);
            }
        }
    }

    void CalculateAndRotateHipAndAnkle(Transform hipJoint, Transform ankleJoint, string kneeAngleStr)
    {
        if (hipJoint == null || ankleJoint == null || kneeAngleStr == "Not visible") return;

        float kneeAngle;
        if (float.TryParse(kneeAngleStr, out kneeAngle))
        {
            otherAngles = -((180f - kneeAngle) / 2f); // Divide the remaining angle equally
            hipJoint.localEulerAngles = initialRotations[hipJoint] + new Vector3(0, 0, otherAngles);
            ankleJoint.localEulerAngles = initialRotations[ankleJoint] + new Vector3(0, 0, otherAngles);
        }
    }

    public void OnToggleChange(bool tickon) {
        if (tickon)
        {
            animateLeftSide = true;
        }
        else {
            animateLeftSide = false;
        }
    }

    public void OnToggleChangeScreen(bool tickon)
    {
        if (tickon)
        {
            Screen2 = true;
        }
        else
        {
            Screen2 = false;
        }
    }

    public void ColorJointsFeedback()
    {
        calculateDifference(KneeIdealAngle, KneeValue, Knee);
        calculateDifference(ShoulderIdealAngle, ShoulderValue, Shoulder);
        calculateDifference(ElbowIdealAngle, ElbowValue, Elbow);
        calculateDifference(HipIdealAngle, otherAngles * 2, Hip);

    }

    public void calculateDifference (float ideal, float joint, Renderer jointColor) {

            float deviation = Mathf.Abs(joint - ideal);
            float normalizedDeviation = Mathf.Clamp(deviation / 90f, 0f, 1f);

            Color jointscolor = Color.Lerp(Color.green, Color.red, normalizedDeviation);

            jointColor.material.color = jointscolor;
        }

    public void OnInputStringB(String Input)
    {
        path = Input;
        Debug.Log(path);
    }
}
