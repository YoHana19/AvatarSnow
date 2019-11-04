using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class HeartBeam : MonoBehaviour, IEffector
{
    [SerializeField] private Transform parent;
    private Vector3 initPosistion = Vector3.zero;
    private VisualEffect vfx;
    private Transform originParent;

    public void Init()
    {
        gameObject.SetActive(true);
        originParent = transform.parent;
        transform.SetParent(parent);
        transform.localPosition = initPosistion;
        if (vfx == null)
        {
            vfx = GetComponent<VisualEffect>();
        }
    }
    
    public void Deinit()
    {
        gameObject.SetActive(false);
        transform.SetParent(originParent);
        vfx.SetBool("isDead", true);
        vfx.SetBool("isStop", false);
    }
    
    public void OnMouthOpen()
    {
        vfx.SetBool("isDead", false);
        vfx.SetBool("isStop", false);
    }

    public void OnMouthClose()
    {
        vfx.SetBool("isDead", true);
        vfx.SetBool("isStop", false);
    }

    public void OnMouthPuckered()
    {
        vfx.SetBool("isStop", true);
    }

    public void OnMouthUnPuckered()
    {
        vfx.SetBool("isStop", false);
    }
}
