using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireBless : MonoBehaviour, IEffector
{
    [SerializeField] private Transform parent;
    private readonly Vector3 initPosistion = new Vector3(0.0025f, -0.019f, 0.112f);
    private Transform originParent;

    public void Init()
    {
        originParent = transform.parent;
        transform.SetParent(parent);
        transform.localPosition = initPosistion;
    }
    
    public void Deinit()
    {
        gameObject.SetActive(false);
        transform.SetParent(originParent);
    }
    
    public void OnMouthOpen()
    {
        gameObject.SetActive(true);
    }

    public void OnMouthClose()
    {
        gameObject.SetActive(false);
    }

    public void OnMouthPuckered() {}

    public void OnMouthUnPuckered() {}
}
