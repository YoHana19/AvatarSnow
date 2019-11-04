using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvatarSnow
{
    public enum EffectType
    {
        None = 0,
        HeartBeam = 1
    }
    
    public class EffectManager : MonoBehaviour
    {

        private EffectType currentEffectType = EffectType.None;
        private IEffector currentEffector;
        
        private bool isMouthOpen;
        private bool isMouthPuckered;

        [SerializeField] private GameObject[] effectors;

        public void SetEffect(EffectType type)
        {
            if (currentEffectType == type) return;
            if (currentEffector != null) currentEffector.Deinit();
            currentEffectType = type;
            var index = (int) type;
            if (index == 0)
            {
                currentEffector = null;
            }
            else
            {
                currentEffector = effectors[index-1].GetComponent<IEffector>();
                currentEffector.Init();
            }
        }

        public void OnMouthOpen()
        {
            if (isMouthOpen) return;
            isMouthOpen = true;
            if (currentEffector != null) currentEffector.OnMouthOpen();
        }

        public void OnMouthClose()
        {
            if (!isMouthOpen) return;
            isMouthOpen = false;
            if (currentEffector != null) currentEffector.OnMouthClose();
        }

        public void OnMouthPuckered()
        {
            if (isMouthPuckered) return;
            isMouthPuckered = true;
            if (currentEffector != null) currentEffector.OnMouthPuckered();
        }

        public void OnMouthUnPuckered()
        {
            if (!isMouthPuckered) return;
            isMouthPuckered = false;
            if (currentEffector != null) currentEffector.OnMouthUnPuckered();
        }
    }
}