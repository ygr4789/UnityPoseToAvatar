using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Tasks.Vision.PoseLandmarker;

public class LandmarkRig : MonoBehaviour
{
    private Animator _animator;
    
    private readonly LandmarkWrapper _landmarkWrapper = new();
    private readonly List<BoneRotationHelper> _helpers = new();

    private void Start()
    {
        _animator = GetComponent<Animator>();

        var boneHips = new Bone(_animator.GetBoneTransform(HumanBodyBones.Hips));
        var boneSpine = new Bone(_animator.GetBoneTransform(HumanBodyBones.Spine), boneHips);
        var boneChest = new Bone(_animator.GetBoneTransform(HumanBodyBones.Chest), boneSpine);
        var boneUpperChest = new Bone(_animator.GetBoneTransform(HumanBodyBones.UpperChest), boneChest);
        var boneNeck = new Bone(_animator.GetBoneTransform(HumanBodyBones.Neck), boneUpperChest);
        var boneHead = new Bone(_animator.GetBoneTransform(HumanBodyBones.Head), boneNeck);
        
        var boneLeftShoulder = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftShoulder), boneUpperChest);
        var boneLeftUpperArm = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftUpperArm), boneLeftShoulder);
        var boneLeftLowerArm = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftLowerArm), boneLeftUpperArm);
        var boneLeftHand = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftHand), boneLeftLowerArm);
        
        var boneRightShoulder = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightShoulder), boneUpperChest);
        var boneRightUpperArm = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightUpperArm), boneRightShoulder);
        var boneRightLowerArm = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightLowerArm), boneRightUpperArm);
        var boneRightHand = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightHand), boneRightLowerArm);
        
        var boneLeftUpperLeg = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg), boneHips);
        var boneLeftLowerLeg = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg), boneLeftUpperLeg);
        var boneLeftFoot = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftFoot), boneLeftLowerLeg);
        var boneLeftToes = new Bone(_animator.GetBoneTransform(HumanBodyBones.LeftToes), boneLeftFoot);
        
        var boneRightUpperLeg = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightUpperLeg), boneHips);
        var boneRightLowerLeg = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightLowerLeg), boneRightUpperLeg);
        var boneRightFoot = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightFoot), boneRightLowerLeg);
        var boneRightToes = new Bone(_animator.GetBoneTransform(HumanBodyBones.RightToes), boneRightFoot);
 
        var leftUpperArmHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.LeftShoulder, LandmarkID.LeftElbow, boneLeftUpperArm);
        var leftLowerArmHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.LeftElbow, LandmarkID.LeftWrist, boneLeftLowerArm);

        var rightUpperArmHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.RightShoulder, LandmarkID.RightElbow, boneRightUpperArm);
        var rightLowerArmHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.RightElbow, LandmarkID.RightWrist, boneRightLowerArm);

        var leftUpperLegHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.LeftHip, LandmarkID.LeftKnee, boneLeftUpperLeg);
        var leftLowerLegHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.LeftKnee, LandmarkID.LeftAnkle, boneLeftLowerLeg);
        var leftFootHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.LeftAnkle, LandmarkID.LeftFootIndex, boneLeftFoot);
        var leftToesHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.LeftHeel, LandmarkID.LeftFootIndex, boneLeftToes);
        
        var rightUpperLegHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.RightHip, LandmarkID.RightKnee, boneRightUpperLeg);
        var rightLowerLegHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.RightKnee, LandmarkID.RightAnkle, boneRightLowerLeg);
        var rightFootHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.RightAnkle, LandmarkID.RightFootIndex, boneRightFoot);
        var rightToesHelper = new TwoLandmarkBoneHelper(_landmarkWrapper, LandmarkID.RightHeel, LandmarkID.RightFootIndex, boneRightToes);

        var bodyHelper = new BodyLandmarkBonesHelper(
            _landmarkWrapper,
            boneHips,
            boneSpine,
            boneChest,
            boneNeck,
            boneLeftUpperLeg,
            boneRightUpperLeg,
            boneLeftShoulder,
            boneRightShoulder
            );
        var headHelper = new HeadRotationHelper(_landmarkWrapper, boneNeck, boneHead);

        _helpers.Add(bodyHelper);
        _helpers.Add(headHelper);

        _helpers.Add(leftUpperArmHelper);
        _helpers.Add(leftLowerArmHelper);
        _helpers.Add(rightUpperArmHelper);
        _helpers.Add(rightLowerArmHelper);
        _helpers.Add(leftUpperLegHelper);
        _helpers.Add(leftLowerLegHelper);
        _helpers.Add(leftFootHelper);
        _helpers.Add(leftToesHelper);
        _helpers.Add(rightUpperLegHelper);
        _helpers.Add(rightLowerLegHelper);
        _helpers.Add(rightFootHelper);
        _helpers.Add(rightToesHelper);
    }

    private void Update()
    {
        if (!_landmarkWrapper.Initialized) return;
        
        foreach (var helper in _helpers)
        {
            helper.UpdateRotation();
        }
    }

    public void SetPose(PoseLandmarkerResult result)
    {
        var resultLandmarks  = result.poseWorldLandmarks[0];
        _landmarkWrapper.SetLandmarks(resultLandmarks);
    }
}

