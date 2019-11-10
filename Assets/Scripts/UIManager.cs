using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarSnow
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private EffectManager effectManager;

        public void TappedEffectButton(int index)
        {
            effectManager.SetEffect((EffectType)Enum.ToObject(typeof(EffectType), index));
        }
    }
}