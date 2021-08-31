using UnityEngine;
using System;

namespace FlowCanvas.Nodes
{

    [Obsolete]
    public class NewGameObject : CallableFunctionNode<GameObject, string, Vector3, Quaternion>
    {
        public override GameObject Invoke(string name, Vector3 position, Quaternion rotation) {
            var go = new GameObject(name);
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }
    }

    [Obsolete]
    public class NewVector2 : PureFunctionNode<Vector2, float, float>
    {
        public override Vector2 Invoke(float x, float y) {
            return new Vector2(x, y);
        }
    }

    [Obsolete]
    public class NewVector3 : PureFunctionNode<Vector3, float, float, float>
    {
        public override Vector3 Invoke(float x, float y, float z) {
            return new Vector3(x, y, z);
        }
    }

    [Obsolete]
    public class NewVector4 : PureFunctionNode<Vector4, float, float, float, float>
    {
        public override Vector4 Invoke(float x, float y, float z, float w) {
            return new Vector4(x, y, z, w);
        }
    }

    [Obsolete]
    public class NewQuaternion : PureFunctionNode<Quaternion, float, float, float, float>
    {
        public override Quaternion Invoke(float x, float y, float z, float w) {
            return new Quaternion(x, y, z, w);
        }
    }

    [Obsolete]
    public class NewColor : PureFunctionNode<Color, float, float, float, float>
    {
        public override Color Invoke(float r, float g, float b, float a = 1) {
            return new Color(r, g, b, a);
        }
    }

    [Obsolete]
    public class NewBounds : PureFunctionNode<Bounds, Vector3, Vector3>
    {
        public override Bounds Invoke(Vector3 center, Vector3 size) {
            return new Bounds(center, size);
        }
    }

    [Obsolete]
    public class NewRect : PureFunctionNode<Rect, float, float, float, float>
    {
        public override Rect Invoke(float left, float top, float width, float height) {
            return new Rect(left, top, width, height);
        }
    }

    [Obsolete]
    public class NewRay : PureFunctionNode<Ray, Vector3, Vector3>
    {
        public override Ray Invoke(Vector3 origin, Vector3 direction) {
            return new Ray(origin, direction);
        }
    }

}