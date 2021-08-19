using Slate;
using Slate.ActionClips;
using UnityEngine;

namespace ET
{
    [Category("Paths")]
    public class TestActionClip:ActorActionClip
    {
        [SerializeField, HideInInspector]
        private float _length = 5f;
        [SerializeField, HideInInspector]
        private float _blendIn = 0f;
        [SerializeField, HideInInspector]
        private float _blendOut = 0f;

        [Required]
        public Path path;
        public bool useSpeed = false;
        [ShowIf("useSpeed", 1)]
        public float speed = 3f;
        [Range(0, 1)]
        public float lookAhead = 0f;
        public Vector3 upVector = Vector3.up;
        public EaseType blendInterpolation = EaseType.QuadraticInOut;

        private Vector3 lastPos;
        private Quaternion lastRot;

        private GameObject realActor;

        public override string info {
            get { return string.Format("Test Follow Path\n{0}", path != null ? path.name : "NONE"); }
        }

        public override float length {
            get { return useSpeed && path != null ? path.length / Mathf.Max(speed, 0.01f) : _length; }
            set { _length = value; }
        }

        public override float blendIn {
            get { return _blendIn; }
            set { _blendIn = value; }
        }

        public override float blendOut {
            get { return _blendOut; }
            set { _blendOut = value; }
        }

        public override bool isValid {
            get { return path != null; }
        }

        protected override void OnEnter()
        {
            realActor = this.actor.GetComponent<DynamicActorWrapper>().GetActor();
            path.Compute();
            lastPos = realActor.transform.position;
            lastRot = realActor.transform.rotation;
        }

        protected override void OnUpdate(float time) {
            if ( length == 0 ) {
                realActor.transform.position = path.GetPointAt(0);
                return;
            }

            var newPos = path.GetPointAt(time / length);
            realActor.transform.position = Easing.Ease(blendInterpolation, lastPos, newPos, GetClipWeight(time));

            if ( lookAhead > 0 ) {
                var lookPos = path.GetPointAt(( time / length ) + lookAhead);
                var dir = lookPos - realActor.transform.position;
                if ( dir.magnitude > 0.001f ) {
                    var lookRot = Quaternion.LookRotation(dir, upVector);
                    realActor.transform.rotation = Easing.Ease(blendInterpolation, lastRot, lookRot, GetClipWeight(time));
                }
            }
        }

        protected override void OnReverse() {
            realActor.transform.position = lastPos;
            realActor.transform.rotation = lastRot;
        }
    }
}