using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace skeleton.Events
{
	public class Skeleton : MonoBehaviour
	{

		/// <summary>
		/// definition of a humanoid biped.
		/// </summary>
		[System.Serializable]
		public class Bones
		{
			public Transform root;          // 0
			public Transform pelvis;        // 1
			public Transform spine;         // 2

			[Tooltip("Optional")]
			public Transform chest;         // 3 Optional

			[Tooltip("Optional")]
			public Transform neck;          // 4 Optional
			public Transform head;          // 5

			[Tooltip("Optional")]
			public Transform leftShoulder;  // 6 Optional
			public Transform leftUpperArm;  // 7
			public Transform leftForearm;   // 8
			public Transform leftHand;      // 9

			[Tooltip("Optional")]
			public Transform rightShoulder; // 10 Optional
			public Transform rightUpperArm; // 11
			public Transform rightForearm;  // 12
			public Transform rightHand;     // 13

			[Tooltip("VRIK also supports legless characters.If you do not wish to use legs, leave all leg references empty.")]
			public Transform leftThigh;     // 14 Optional

			[Tooltip("VRIK also supports legless characters.If you do not wish to use legs, leave all leg references empty.")]
			public Transform leftCalf;      // 15 Optional

			[Tooltip("VRIK also supports legless characters.If you do not wish to use legs, leave all leg references empty.")]
			public Transform leftFoot;      // 16 Optional

			[Tooltip("Optional")]
			public Transform leftToes;      // 17 Optional

			[Tooltip("VRIK also supports legless characters.If you do not wish to use legs, leave all leg references empty.")]
			public Transform rightThigh;    // 18 Optional

			[Tooltip("VRIK also supports legless characters.If you do not wish to use legs, leave all leg references empty.")]
			public Transform rightCalf;     // 19 Optional

			[Tooltip("VRIK also supports legless characters.If you do not wish to use legs, leave all leg references empty.")]
			public Transform rightFoot;     // 20 Optional

			[Tooltip("Optional")]
			public Transform rightToes;     // 21 Optional

			/// <summary>
			/// Returns an array of all the Transforms in the definition.
			/// </summary>
			public Transform[] GetTransforms()
			{
				return new Transform[22] {
					root, pelvis, spine, chest, neck, head, leftShoulder, leftUpperArm, leftForearm, leftHand, rightShoulder, rightUpperArm, rightForearm, rightHand, leftThigh, leftCalf, leftFoot, leftToes, rightThigh, rightCalf, rightFoot, rightToes
				};
			}

			/// <summary>
			/// Returns true if all required Transforms have been assigned (shoulder, toe and neck bones are optional).
			/// </summary>
			public bool isFilled
			{
				get
				{
					if (
						root == null ||
						pelvis == null ||
						spine == null ||
						head == null ||
						leftUpperArm == null ||
						leftForearm == null ||
						leftHand == null ||
						rightUpperArm == null ||
						rightForearm == null ||
						rightHand == null
					)
						return false;

					// If all leg bones are null, it is valid
					bool noLegBones =
						leftThigh == null &&
						leftCalf == null &&
						leftFoot == null &&
						rightThigh == null &&
						rightCalf == null &&
						rightFoot == null;

					if (noLegBones)
						return true;

					bool atLeastOneLegBoneMissing =
						leftThigh == null ||
						leftCalf == null ||
						leftFoot == null ||
						rightThigh == null ||
						rightCalf == null ||
						rightFoot == null;

					if (atLeastOneLegBoneMissing)
						return false;

					// Shoulder, toe and neck bones are optional
					return true;
				}
			}

			/// <summary>
			/// Returns true if none of the Transforms have been assigned.
			/// </summary>
			public bool isEmpty
			{
				get
				{
					if (
						root != null ||
						pelvis != null ||
						spine != null ||
						chest != null ||
						neck != null ||
						head != null ||
						leftShoulder != null ||
						leftUpperArm != null ||
						leftForearm != null ||
						leftHand != null ||
						rightShoulder != null ||
						rightUpperArm != null ||
						rightForearm != null ||
						rightHand != null ||
						leftThigh != null ||
						leftCalf != null ||
						leftFoot != null ||
						leftToes != null ||
						rightThigh != null ||
						rightCalf != null ||
						rightFoot != null ||
						rightToes != null
					)
						return false;

					return true;
				}
			}
			/// <summary>
			/// Auto-detects bones. Works with a Humanoid Animator on the root gameobject only.
			/// </summary>
			public static bool AutoDetectReferences(Transform root, out Bones bones)
			{
				bones = new Bones();

				var animator = root.GetComponentInChildren<Animator>();
				if (animator == null || !animator.isHuman)
				{
					Debug.LogWarning("needs a Humanoid Animator to auto-detect biped references. Please assign references manually.");
					return false;
				}

				bones.root = root;
				bones.pelvis = animator.GetBoneTransform(HumanBodyBones.Hips);
				bones.spine = animator.GetBoneTransform(HumanBodyBones.Spine);
				bones.chest = animator.GetBoneTransform(HumanBodyBones.Chest);
				bones.neck = animator.GetBoneTransform(HumanBodyBones.Neck);
				bones.head = animator.GetBoneTransform(HumanBodyBones.Head);
				bones.leftShoulder = animator.GetBoneTransform(HumanBodyBones.LeftShoulder);
				bones.leftUpperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
				bones.leftForearm = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm);
				bones.leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
				bones.rightShoulder = animator.GetBoneTransform(HumanBodyBones.RightShoulder);
				bones.rightUpperArm = animator.GetBoneTransform(HumanBodyBones.RightUpperArm);
				bones.rightForearm = animator.GetBoneTransform(HumanBodyBones.RightLowerArm);
				bones.rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
				bones.leftThigh = animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
				bones.leftCalf = animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
				bones.leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
				bones.leftToes = animator.GetBoneTransform(HumanBodyBones.LeftToes);
				bones.rightThigh = animator.GetBoneTransform(HumanBodyBones.RightUpperLeg);
				bones.rightCalf = animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
				bones.rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
				bones.rightToes = animator.GetBoneTransform(HumanBodyBones.RightToes);

				return true;
			}
		}
		/// <summary>
		/// Bone mapping. Right-click on the component header and select 'Auto-detect References' of fill in manually if not a Humanoid character. Chest, neck, shoulder and toe bones are optional. VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.
		/// </summary>
		[ContextMenuItem("Auto-detect References", "AutoDetectReferences")]
		[Tooltip("Bone mapping. Right-click on the component header and select 'Auto-detect References' of fill in manually if not a Humanoid character. Chest, neck, shoulder and toe bones are optional. VRIK also supports legless characters. If you do not wish to use legs, leave all leg references empty.")]
		public Bones bones = new Bones();

		/// <summary>
		/// Auto-detects bone references for this VRIK. Works with a Humanoid Animator on the gameobject only.
		/// </summary>
		[ContextMenu("Auto-detect References")]
		public void AutoDetectReferences()
		{
			Bones.AutoDetectReferences(transform, out bones);
		}



	}
}
