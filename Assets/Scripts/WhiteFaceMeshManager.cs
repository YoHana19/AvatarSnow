using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public sealed class WhiteFaceMeshManager : FaceMeshManagerBase
{
    
    [SerializeField] private SkinnedMeshRenderer ref_FACE;
    [SerializeField] private Transform ref_EYE_R;
    [SerializeField] private Transform ref_EYE_L;

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
        var rot = new Vector3 (rotation.eulerAngles.x - prevRotation.x, rotation.eulerAngles.y - prevRotation.y, rotation.eulerAngles.z - prevRotation.z);
        rot = new Vector3 (rot.x * -1f, rot.y * 1f, rot.z * -1f);
        headJoint.transform.Rotate (rot);
        prevRotation = new Vector3 (rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
        
        foreach (KeyValuePair<string,float> kvp in currentBlendShapes) {
            switch (kvp.Key)
            {
                case ARBlendShapeLocation.EyeBlinkRight:
                    var rightEyeValue = kvp.Value * 100f;
                    if (rightEyeValue > 70f) {
                        rightEyeValue *= 1.2f;
                    }
                    ref_FACE.SetBlendShapeWeight(14, rightEyeValue);
                    break;
                case ARBlendShapeLocation.EyeBlinkLeft:
                    var leftEyeValue = kvp.Value * 100f;
                    if (leftEyeValue > 70f) {
                        leftEyeValue *= 1.2f;
                    }
                    ref_FACE.SetBlendShapeWeight(15, leftEyeValue);
                    break;
                case ARBlendShapeLocation.EyeWideRight:
                    var wideEyeValue = kvp.Value * 100f;
                    ref_FACE.SetBlendShapeWeight(18, wideEyeValue);
                    break;
                case ARBlendShapeLocation.JawOpen:
                    ref_FACE.SetBlendShapeWeight(23, kvp.Value*100f);
                    break;
                case ARBlendShapeLocation.MouthPucker:
                    ref_FACE.SetBlendShapeWeight(25, kvp.Value*100f);
                    break;
                case ARBlendShapeLocation.MouthSmileRight:
                    ref_FACE.SetBlendShapeWeight(29, kvp.Value*100f);
                    break;
                case ARBlendShapeLocation.MouthShrugLower:
                    ref_FACE.SetBlendShapeWeight(31, kvp.Value*100f);
                    break;
                case ARBlendShapeLocation.BrowInnerUp:
                    ref_FACE.SetBlendShapeWeight(9, kvp.Value*100f);
                    break;
                case ARBlendShapeLocation.BrowDownRight:
                    ref_FACE.SetBlendShapeWeight(7, kvp.Value*100f);
                    break;
                case ARBlendShapeLocation.EyeLookDownRight:
                    ref_EYE_R.localEulerAngles = new Vector3(kvp.Value*5f, ref_EYE_R.localEulerAngles .y, 0f);
                    break;
                case ARBlendShapeLocation.EyeLookUpRight:
                    ref_EYE_R.localEulerAngles = new Vector3(kvp.Value*-5f, ref_EYE_R.localEulerAngles .y, 0f);
                    break;
                case ARBlendShapeLocation.EyeLookInRight:
                    ref_EYE_R.localEulerAngles = new Vector3(ref_EYE_R.localEulerAngles.x, kvp.Value*-10f, 0f);
                    break;
                case ARBlendShapeLocation.EyeLookOutRight:
                    ref_EYE_R.localEulerAngles = new Vector3(ref_EYE_R.localEulerAngles.x, kvp.Value*10f, 0f);
                    break;
                case ARBlendShapeLocation.EyeLookDownLeft:
                    ref_EYE_L.localEulerAngles = new Vector3(kvp.Value*5f, ref_EYE_L.localEulerAngles.y, 0f);
                    break;
                case ARBlendShapeLocation.EyeLookUpLeft:
                    ref_EYE_L.localEulerAngles = new Vector3(kvp.Value*-5f, ref_EYE_L.localEulerAngles.y, 0f);
                    break;
                case ARBlendShapeLocation.EyeLookInLeft:
                    ref_EYE_L.localEulerAngles = new Vector3(ref_EYE_L.localEulerAngles.x, kvp.Value*10f, 0f);
                    break;
                case ARBlendShapeLocation.EyeLookOutLeft:
                    ref_EYE_L.localEulerAngles = new Vector3(ref_EYE_L.localEulerAngles.x, kvp.Value*-10f, 0f);
                    break;
            }
        }

        InvokEffector();
    }

    protected override void FaceRemoved (ARFaceAnchor anchorData)
    {
        headJoint.localRotation = defaultRotation;
        prevRotation = Vector3.zero;
    }
}
