using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.iOS;

public class BlendshapeVisualizer : MonoBehaviour
{
	
	[SerializeField] private Transform canvas;
	[SerializeField] private GameObject sliderObj;
	Dictionary<string, float> currentBlendShapes;
	Dictionary<string, Slider> blendShapeSliders = new Dictionary<string, Slider>();
	private UnityARSessionNativeInterface m_session;

	// Use this for initialization
	void Start () {
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
	}

	void FaceAdded (ARFaceAnchor anchorData)
	{
		currentBlendShapes = anchorData.blendShapes;
		InitSliders();
	}

	void FaceUpdated (ARFaceAnchor anchorData)
	{
		currentBlendShapes = anchorData.blendShapes;
		UpdateSliders();
	}

	void FaceRemoved (ARFaceAnchor anchorData)
	{
		blendShapeSliders.Clear();
	}


	private void InitSliders()
	{
		foreach (KeyValuePair<string, float> kvp in currentBlendShapes)
		{
			InitSlider(kvp.Key);
		}
	}
	
	private void InitSlider(string name)
	{
		var obj = Instantiate(sliderObj, canvas);
		obj.GetComponent<Text>().text = name;
		blendShapeSliders.Add(name, obj.transform.Find("Slider").GetComponent<Slider>());
	}

	private void UpdateSliders()
	{
		foreach (KeyValuePair<string, float> kvp in currentBlendShapes)
		{
			blendShapeSliders[kvp.Key].value = kvp.Value;
		}
	}


}
