//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System;
using System.Collections;
using UnityEngine;
using Valve.VR;

using System.Linq;

namespace Valve.VR.o
{
    public class SteamVR_Skeleton_Pose0 : ScriptableObject
    {
        public SteamVR_Skeleton_Pose_Hand0 leftHand = new SteamVR_Skeleton_Pose_Hand0(SteamVR_Input_Sources.LeftHand);
        public SteamVR_Skeleton_Pose_Hand0 rightHand = new SteamVR_Skeleton_Pose_Hand0(SteamVR_Input_Sources.RightHand);

        protected const int leftHandInputSource = (int)SteamVR_Input_Sources.LeftHand;
        protected const int rightHandInputSource = (int)SteamVR_Input_Sources.RightHand;

        public SteamVR_Skeleton_Pose_Hand0 GetHand(int hand)
        {
            if (hand == leftHandInputSource)
                return leftHand;
            else if (hand == rightHandInputSource)
                return rightHand;
            return null;
        }

        public SteamVR_Skeleton_Pose_Hand0 GetHand(SteamVR_Input_Sources hand)
        {
            if (hand == SteamVR_Input_Sources.LeftHand)
                return leftHand;
            else if (hand == SteamVR_Input_Sources.RightHand)
                return rightHand;
            return null;
        }
    }

    [Serializable]
    public class SteamVR_Skeleton_Pose_Hand0
    {
        public SteamVR_Input_Sources inputSource;

        public SteamVR_Skeleton_FingerExtensionTypes thumbFingerMovementType = SteamVR_Skeleton_FingerExtensionTypes.Static;
        public SteamVR_Skeleton_FingerExtensionTypes indexFingerMovementType = SteamVR_Skeleton_FingerExtensionTypes.Static;
        public SteamVR_Skeleton_FingerExtensionTypes middleFingerMovementType = SteamVR_Skeleton_FingerExtensionTypes.Static;
        public SteamVR_Skeleton_FingerExtensionTypes ringFingerMovementType = SteamVR_Skeleton_FingerExtensionTypes.Static;
        public SteamVR_Skeleton_FingerExtensionTypes pinkyFingerMovementType = SteamVR_Skeleton_FingerExtensionTypes.Static;

        /// <summary>
        /// Get extension type for a particular finger. Thumb is 0, Index is 1, etc.
        /// </summary>
        public SteamVR_Skeleton_FingerExtensionTypes GetFingerExtensionType(int finger)
        {
            if (finger == 0)
                return thumbFingerMovementType;
            if (finger == 1)
                return indexFingerMovementType;
            if (finger == 2)
                return middleFingerMovementType;
            if (finger == 3)
                return ringFingerMovementType;
            if (finger == 4)
                return pinkyFingerMovementType;

            //default to static
            Debug.LogWarning("Finger not in range!");
            return SteamVR_Skeleton_FingerExtensionTypes.Static;
        }

        public bool ignoreRootPoseData = true;
        public bool ignoreWristPoseData = true;

        public Vector3 position;
        public Quaternion rotation;

        public Vector3[] bonePositions;
        public Quaternion[] boneRotations;

        public SteamVR_Skeleton_Pose_Hand0(SteamVR_Input_Sources source)
        {
            inputSource = source;
        }

        public SteamVR_Skeleton_FingerExtensionTypes GetMovementTypeForBone(int boneIndex)
        {
            int fingerIndex = SteamVR_Skeleton_JointIndexes0.GetFingerForBone(boneIndex);

            switch (fingerIndex)
            {
                case SteamVR_Skeleton_FingerIndexes0.thumb:
                    return thumbFingerMovementType;

                case SteamVR_Skeleton_FingerIndexes0.index:
                    return indexFingerMovementType;

                case SteamVR_Skeleton_FingerIndexes0.middle:
                    return middleFingerMovementType;

                case SteamVR_Skeleton_FingerIndexes0.ring:
                    return ringFingerMovementType;

                case SteamVR_Skeleton_FingerIndexes0.pinky:
                    return pinkyFingerMovementType;
            }

            return SteamVR_Skeleton_FingerExtensionTypes.Static;
        }
    }

    public enum SteamVR_Skeleton_FingerExtensionTypes
    {
        Static,
        Free,
        Extend,
        Contract,
    }

    public class SteamVR_Skeleton_FingerExtensionTypeLists0
    {
        private SteamVR_Skeleton_FingerExtensionTypes[] _enumList;
        public SteamVR_Skeleton_FingerExtensionTypes[] enumList
        {
            get
            {
                if (_enumList == null)
                    _enumList = (SteamVR_Skeleton_FingerExtensionTypes[])System.Enum.GetValues(typeof(SteamVR_Skeleton_FingerExtensionTypes));
                return _enumList;
            }
        }

        private string[] _stringList;
        public string[] stringList
        {
            get
            {
                if (_stringList == null)
                    _stringList = enumList.Select(element => element.ToString()).ToArray();
                return _stringList;
            }
        }
    }
}