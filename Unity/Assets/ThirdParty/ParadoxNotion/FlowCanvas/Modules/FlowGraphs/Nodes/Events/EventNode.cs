using System.Collections.Generic;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace FlowCanvas.Nodes
{

    [Category("Events")]
    [Color("ff5c5c")]
    [ContextDefinedOutputs(typeof(Flow))]
    [ExecutionPriority(1)]
    ///Base class for event nodes.
    abstract public class EventNode : FlowNode
    {
        public override string name {
            get { return string.Format("➥ {0}", base.name.ToUpper()); }
        }
    }

    ///----------------------------------------------------------------------------------------------

    ///Base class for event nodes that require a specific target Component or GameObject.
    ///(use Transform for GameObjects).
    [ContextDefinedOutputs(typeof(Wild))]
    abstract public class EventNode<T> : EventNode where T : Component
    {

        public BBParameter<T> target;

        public override string name {
            get { return string.Format("{0} ({1})", base.name.ToUpper(), !target.useBlackboard && target.value == null ? "Self" : target.ToString()); }
        }

        //...
        public override void OnPostGraphStarted() {
            ResolveSelf();
        }

        ///Resolve component from Self if 'target' is null
        protected void ResolveSelf() {
            if ( !target.useBlackboard && target.value == null ) {
                target.value = graphAgent.GetComponent<T>();
            }
        }
    }

    ///----------------------------------------------------------------------------------------------

    ///Base class for event nodes with single or multiple target Component(s) that work with MonoBehaviour-based event callbacks raised through EventRouter.
    ///(use Transform for GameObjects)
    [ContextDefinedOutputs(typeof(Wild))]
    abstract public class RouterEventNode<T> : EventNode where T : Component
    {

        public enum TargetMode
        {
            SingleTarget = 0,
            MultipleTargets = 1
        }

        public TargetMode targetMode;
        [ShowIf("targetMode", 0)]
        public BBParameter<T> target;
        [ShowIf("targetMode", 1)]
        public BBParameter<List<T>> targets;

        public override string name {
            get
            {
                string text = string.Empty;
                if ( targetMode == TargetMode.SingleTarget ) {
                    text = !target.useBlackboard && target.value == null ? "Self" : target.ToString();
                } else {
                    text = targets.ToString();
                }
                return string.Format("{0} ({1})", base.name.ToUpper(), text);
            }
        }

        ///Called per target object with argument being the EventRouter of that object (never null)
        abstract protected void Subscribe(ParadoxNotion.Services.EventRouter router);
        ///Called per target object with argument being the EventRouter of that object (never null)
        abstract protected void UnSubscribe(ParadoxNotion.Services.EventRouter router);

        //...
        public override void OnPostGraphStarted() {

            //SINGLE TARGET
            if ( targetMode == TargetMode.SingleTarget ) {
                if ( !target.useBlackboard && target.value == null ) {
                    target.value = graphAgent.GetComponent<T>();
                }

                var o = target.value;
                if ( o == null ) {
                    Fail(string.Format("Target is missing component of type '{0}'", typeof(T).Name));
                    return;
                }

                Subscribe(o.gameObject.GetAddComponent<ParadoxNotion.Services.EventRouter>());
            }

            //MULTIPLE TARGETS
            if ( targetMode == TargetMode.MultipleTargets ) {
                var list = targets.value;
                if ( list == null || list.Count == 0 ) {
                    Fail("No Targets specified");
                    return;
                }

                foreach ( var target in list ) {
                    if ( target != null ) {
                        Subscribe(target.gameObject.GetAddComponent<ParadoxNotion.Services.EventRouter>());
                    }
                }
            }
        }

        //...
        public override void OnGraphStoped() {
            //SINGLE TARGET
            if ( targetMode == TargetMode.SingleTarget && target.value != null ) {
                var router = target.value.gameObject.GetComponent<ParadoxNotion.Services.EventRouter>();
                if ( router != null ) { UnSubscribe(router); }
            }

            //MULTIPLE TARGETS
            if ( targetMode == TargetMode.MultipleTargets && targets.value != null ) {
                foreach ( var target in targets.value ) {
                    if ( target != null ) {
                        var router = target.gameObject.GetComponent<ParadoxNotion.Services.EventRouter>();
                        if ( router != null ) { UnSubscribe(router); }
                    }
                }
            }
        }

        ///Utility to resolve receiver T object faster and for better performance
        protected T ResolveReceiver(GameObject receiver) {
            if ( receiver == null ) { return null; }

            if ( targetMode == TargetMode.SingleTarget ) {
                var result = target.value;
                return result != null && result.gameObject == receiver ? result : null;
            }

            var listResult = targets.value;
            for ( var i = 0; i < listResult.Count; i++ ) {
                var element = listResult[i];
                if ( element != null && element.gameObject == receiver ) {
                    return element;
                }
            }

            return null;
        }
    }
}