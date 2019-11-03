using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public abstract class FaceMeshManagerBase : MonoBehaviour
{
    private UnityARSessionNativeInterface m_session;
    private const int DEFAULT_ANIMATION_LENGTH = 75;
    
    [SerializeField] protected Vector3 offset;
    [SerializeField] protected GameObject model;
    [SerializeField] protected Transform headJoint;

    protected Dictionary<string, float> currentBlendShapes;
    protected Vector3 prevRotation;
    protected Quaternion defaultRotation;

    private void Awake()
    {
        offset = model.transform.position;
    }

    private void Start ()
    {
        m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface ();

        Application.targetFrameRate = 60;
        ARKitFaceTrackingConfiguration config = new ARKitFaceTrackingConfiguration ();
        config.alignment = UnityARAlignment.UnityARAlignmentGravity;
        config.enableLightEstimation = true;

        if (config.IsSupported) {
			
            m_session.RunWithConfig (config);

            UnityARSessionNativeInterface.ARFaceAnchorAddedEvent += FaceAdded;
            UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent += FaceUpdated;
            UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent += FaceRemoved;
        }
        
        defaultRotation = headJoint.localRotation;
        StartCoroutine(DisactiveAnimator());
    }

    private void OnDestroy()
    {
        UnityARSessionNativeInterface.ARFaceAnchorAddedEvent -= FaceAdded;
        UnityARSessionNativeInterface.ARFaceAnchorUpdatedEvent -= FaceUpdated;
        UnityARSessionNativeInterface.ARFaceAnchorRemovedEvent -= FaceRemoved;
    }

    protected abstract void FaceAdded(ARFaceAnchor anchorData);
    protected abstract void FaceUpdated(ARFaceAnchor anchorData);
    protected abstract void FaceRemoved(ARFaceAnchor anchorData);
    
    // Defaultの姿勢を取らせた後、Animatorを無効化する
    protected IEnumerator DisactiveAnimator()
    {
        for (int i = 0; i < DEFAULT_ANIMATION_LENGTH; i++)
        {
            yield return null;
        }
        model.GetComponent<Animator>().enabled = false;
    }
}
