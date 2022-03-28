using System;
using System.Collections;
using System.Collections.Generic;
using ET;
using UnityEngine;

namespace ETEditor
{
    public class LiteEntity: IDisposable
    {
        private Dictionary<Type, LiteEntity> components;

        protected LiteEntity parent;

        public LiteEntity Parent
        {
            get => this.parent;
            private set
            {
                if (value == null)
                {
                    throw new Exception($"cant set parent null: {this.GetType().Name}");
                }
                
                if (value == this)
                {
                    throw new Exception($"cant set parent self: {this.GetType().Name}");
                }
                if (this.parent != null) // 之前有parent
                {
                    // parent相同，不设置
                    if (this.parent == value)
                    {
                        Log.Error($"重复设置了Parent: {this.GetType().Name} parent: {this.parent.GetType().Name}");
                        return;
                    }
                    this.parent.RemoveComponent(this.GetType());
                }
                this.parent = value;
            }
        }
        
        public Dictionary<Type, LiteEntity> Components
        {
            get
            {
                if (this.components == null)
                {
                    this.components = MonoPool.Instance.Fetch(typeof(Dictionary<Type, LiteEntity>)) as Dictionary<Type, LiteEntity>;
                }
                return this.components;
            }
        }

        public T AddComponent<T>() where T : LiteEntity
        {
            if (this.Components.ContainsKey(typeof(T)))
            {
                Debug.LogError("Key allready added");
                return null;
            }
            T component = Create<T>();
            component.Parent = this;
            this.Components.Add(typeof(T),component);
            return component;
        }

        public LiteEntity AddComponent(LiteEntity component)
        {
            if (this.Components.ContainsKey(component.GetType()))
            {
                Debug.LogError("Key allready added");
                return null;
            }
            
            component.Parent = this;
            this.Components.Add(component.GetType(),component);
            return component;
        }

        public void RemoveComponent<T>() where T:LiteEntity
        {
            RemoveComponent(typeof(T));
        }

        public void RemoveComponent(Type type)
        {
            if (this.Components.ContainsKey(type))
            {
                this.Components.Remove(type);
            }
        }

        public T GetComponent<T>() where T : LiteEntity
        {
            LiteEntity component = null;
            this.Components.TryGetValue(typeof (T), out component);
            return component as T;
        }

        public T GetParent<T>() where T : LiteEntity
        {
            if (this.parent==null)
            {
                return null;
            }
            return this.parent as T;
        }

        public static T Create<T>() where T : LiteEntity
        {
            T entity = Activator.CreateInstance<T>();
            return entity;
        }

        public void Dispose()
        {
            if (this.components!=null)
            {
                foreach (var componentKV in this.components)
                {
                    componentKV.Value.Dispose();
                }
                this.components.Clear();
                MonoPool.Instance.Recycle(this.components);
                this.components = null;
            }

            if (this.parent!=null )
            {
                this.parent.RemoveComponent(this.GetType());
                this.parent = null;
            }
        }
    }
}

