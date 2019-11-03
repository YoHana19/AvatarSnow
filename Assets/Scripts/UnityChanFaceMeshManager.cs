using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public sealed class UnityChanFaceMeshManager : FaceMeshManagerBase
{
    
    [SerializeField] private SkinnedMeshRenderer ref_SMR_EYE_DEF;
    [SerializeField] private SkinnedMeshRenderer ref_SMR_EL_DEF;
    [SerializeField] private SkinnedMeshRenderer ref_SMR_MTH_DEF;

    protected override void FaceAdded (ARFaceAnchor anchorData)
    {
        currentBlendShapes = anchorData.blendShapes;
        model.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform) + offset;
        prevRotation = UnityARMatrixOps.GetRotation (anchorData.transform).eulerAngles;
    }

    protected override void FaceUpdated (ARFaceAnchor anchorData)
    {
        currentBlendShapes = anchorData.blendShapes;
        model.transform.localPosition = UnityARMatrixOps.GetPosition (anchorData.transform) + offset;
        var rotation = UnityARMatrixOps.GetRotation (anchorData.transform);
        var rot = new Vector3 (rotation.eulerAngles.y - prevRotation.y, rotation.eulerAngles.z - prevRotation.z, rotation.eulerAngles.x - prevRotation.x);
        rot = new Vector3 (rot.x * -1f, rot.y * -1f, rot.z * 1f);
        headJoint.transform.Rotate (rot);
        prevRotation = new Vector3 (rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
        
        foreach (KeyValuePair<string,float> kvp in currentBlendShapes) {
            if (kvp.Key == ARBlendShapeLocation.EyeBlinkLeft) {
                var leftEyeValue = kvp.Value * 100f;
                // 眼を完全に閉じても値が1.0にならないので補正
                if (leftEyeValue > 70f) {
                    leftEyeValue *= 1.2f;
                }
                ref_SMR_EYE_DEF.SetBlendShapeWeight (6, leftEyeValue);
                ref_SMR_EL_DEF.SetBlendShapeWeight (6, leftEyeValue);
            } else if (kvp.Key == ARBlendShapeLocation.JawOpen) {
                ref_SMR_MTH_DEF.SetBlendShapeWeight (6, kvp.Value * 100f);
            }
        }
    }

    protected override void FaceRemoved (ARFaceAnchor anchorData)
    {
        headJoint.localRotation = defaultRotation;
        prevRotation = Vector3.zero;
    }
}
