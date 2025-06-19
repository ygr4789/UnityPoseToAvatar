using UnityEngine;
using Mediapipe.Tasks.Components.Containers;
using System.Collections.Generic;
using System;

public enum LandmarkID
{
    Nose = 0,
    LeftEyeInner = 1,
    LeftEye = 2,
    LeftEyeOuter = 3,
    RightEyeInner = 4,
    RightEye = 5,
    RightEyeOuter = 6,
    LeftEar = 7,
    RightEar = 8,
    MouthLeft = 9,
    MouthRight = 10,
    LeftShoulder = 11,
    RightShoulder = 12,
    LeftElbow = 13,
    RightElbow = 14,
    LeftWrist = 15,
    RightWrist = 16,
    LeftPinky = 17,
    RightPinky = 18,
    LeftIndex = 19,
    RightIndex = 20,
    LeftThumb = 21,
    RightThumb = 22,
    LeftHip = 23,
    RightHip = 24,
    LeftKnee = 25,
    RightKnee = 26,
    LeftAnkle = 27,
    RightAnkle = 28,
    LeftHeel = 29,
    RightHeel = 30,
    LeftFootIndex = 31,
    RightFootIndex = 32
}

public class LandmarkWrapper
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Landmarks _landmarks;
    // private Quaternion _initialTilt = Quaternion.AngleAxis(10f, Vector3.right);
    public bool Initialized => _landmarks.landmarks is not null;

    private Dictionary<LandmarkID, KalmanFilter> kf = new Dictionary<LandmarkID, KalmanFilter>();

    public LandmarkWrapper()
    {
        foreach (LandmarkID id in Enum.GetValues(typeof(LandmarkID)))
        {
            kf[id] = new KalmanFilter(6, 3);
        }
    }

    public Vector3 PosOf(LandmarkID id)
    {
        var landmark = _landmarks.landmarks[(int)id];

        var x = landmark.x;
        var y = -landmark.y;
        var z = landmark.z;
        var pos = new Vector3(x, y, z);
        // return _initialTilt * pos;
        return pos;
    }

    public bool PresenceOf(LandmarkID id, float threshold)
    {
        // var landmark = _landmarks.landmarks[(int)id];
        // return landmark.presence > threshold;
        return true;
    }

    public float VisibilityOf(LandmarkID id)
    {
        var landmark = _landmarks.landmarks[(int)id];
        if (landmark.visibility != null) return (float)landmark.visibility;
        return 0;
    }

    public void SetLandmarks(Landmarks landmarks)
    {
        landmarks.CloneTo(ref _landmarks);
    }
}
