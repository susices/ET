using System.Collections;
using UnityEngine;
using ParadoxNotion.Design;
using ParadoxNotion.Animation;


namespace FlowCanvas.Nodes
{

    [Category("Tweening/Values")]
    public class LerpFloat : PureFunctionNode<float, float, float, float, EaseType>
    {
        public override float Invoke(float a, float b, float t, EaseType interpolation) {
            return Easing.Ease(interpolation, a, b, t);
        }
    }

    [Category("Tweening/Values")]
    public class LerpVector3 : PureFunctionNode<Vector3, Vector3, Vector3, float, EaseType>
    {
        public override Vector3 Invoke(Vector3 a, Vector3 b, float t, EaseType interpolation) {
            return Easing.Ease(interpolation, a, b, t);
        }
    }

    [Category("Tweening/Values")]
    public class LerpQuaternion : PureFunctionNode<Quaternion, Quaternion, Quaternion, float, EaseType>
    {
        public override Quaternion Invoke(Quaternion a, Quaternion b, float t, EaseType interpolation) {
            return Easing.Ease(interpolation, a, b, t);
        }
    }

    [Category("Tweening/Values")]
    public class LerpColor : PureFunctionNode<Color, Color, Color, float, EaseType>
    {
        public override Color Invoke(Color a, Color b, float t, EaseType interpolation) {
            return Easing.Ease(interpolation, a, b, t);
        }
    }

    ///----------------------------------------------------------------------------------------------

    [Category("Tweening/Values")]
    public class TweenFloat : LatentActionNode<float, float, float, EaseType>
    {
        public float value { get; private set; }
        public override IEnumerator Invoke(float from, float to, float duration = 1, EaseType interpolation = EaseType.QuadraticInOut) {
            var t = 0f;
            while ( t <= duration ) {
                t += parentNode.graph.deltaTime;
                value = Easing.Ease(interpolation, from, to, t / duration);
                yield return null;
            }
        }
    }

    [Category("Tweening/Values")]
    public class TweenVector3 : LatentActionNode<Vector3, Vector3, float, EaseType>
    {
        public Vector3 value { get; private set; }
        public override IEnumerator Invoke(Vector3 from, Vector3 to, float duration = 1, EaseType interpolation = EaseType.QuadraticInOut) {
            var t = 0f;
            while ( t <= duration ) {
                t += parentNode.graph.deltaTime;
                value = Easing.Ease(interpolation, from, to, t / duration);
                yield return null;
            }
        }
    }

    [Category("Tweening/Values")]
    public class TweenQuaternion : LatentActionNode<Quaternion, Quaternion, float, EaseType>
    {
        public Quaternion value { get; private set; }
        public override IEnumerator Invoke(Quaternion from, Quaternion to, float duration = 1, EaseType interpolation = EaseType.QuadraticInOut) {
            var t = 0f;
            while ( t <= duration ) {
                t += parentNode.graph.deltaTime;
                value = Easing.Ease(interpolation, from, to, t / duration);
                yield return null;
            }
        }
    }

    [Category("Tweening/Values")]
    public class TweenColor : LatentActionNode<Color, Color, float, EaseType>
    {
        public Color value { get; private set; }
        public override IEnumerator Invoke(Color from, Color to, float duration = 1, EaseType interpolation = EaseType.QuadraticInOut) {
            var t = 0f;
            while ( t <= duration ) {
                t += parentNode.graph.deltaTime;
                value = Easing.Ease(interpolation, from, to, t / duration);
                yield return null;
            }
        }
    }

    ///----------------------------------------------------------------------------------------------

    [Category("Tweening")]
    public class TweenPosition : LatentActionNode<Transform, Vector3, float, EaseType, bool>
    {
        public override IEnumerator Invoke(Transform transform, Vector3 position, float duration = 0.25f, EaseType interpolation = EaseType.QuadraticInOut, bool relative = false) {
            position = relative ? transform.localPosition + position : position;
            if ( transform.localPosition != position ) {
                var t = 0f;
                var from = transform.localPosition;
                while ( t <= duration ) {
                    t += parentNode.graph.deltaTime;
                    transform.localPosition = Easing.Ease(interpolation, from, position, t / duration);
                    yield return null;
                }
            }
        }
    }

    [Category("Tweening")]
    public class TweenRotation : LatentActionNode<Transform, Vector3, float, EaseType, bool>
    {
        public override IEnumerator Invoke(Transform transform, Vector3 rotation, float duration = 0.25f, EaseType interpolation = EaseType.QuadraticInOut, bool relative = false) {
            rotation = relative ? transform.localEulerAngles + rotation : rotation;
            if ( transform.localEulerAngles != rotation ) {
                var t = 0f;
                var from = transform.localEulerAngles;
                while ( t <= duration ) {
                    t += parentNode.graph.deltaTime;
                    transform.localEulerAngles = Easing.Ease(interpolation, from, rotation, t / duration);
                    yield return null;
                }
            }
        }
    }

    [Category("Tweening")]
    public class TweenScale : LatentActionNode<Transform, Vector3, float, EaseType, bool>
    {
        public override IEnumerator Invoke(Transform transform, Vector3 scale, float duration = 0.25f, EaseType interpolation = EaseType.QuadraticInOut, bool relative = false) {
            scale = relative ? transform.localScale + scale : scale;
            if ( transform.localScale != scale ) {
                var t = 0f;
                var from = transform.localScale;
                while ( t <= duration ) {
                    t += parentNode.graph.deltaTime;
                    transform.localScale = Easing.Ease(interpolation, from, scale, t / duration);
                    yield return null;
                }
            }
        }
    }
}