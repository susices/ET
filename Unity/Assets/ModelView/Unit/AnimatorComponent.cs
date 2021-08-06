using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	public enum MotionType
	{
		None,
		Idle,
		Run,
	}

	public class AnimatorComponent : Entity
	{
		public Dictionary<string, AnimationClip> animationClips;
		public HashSet<string> Parameter;

		public MotionType MotionType;
		public float MontionSpeed;
		public bool isStop;
		public float stopSpeed;
		public Animator Animator;
	}
}