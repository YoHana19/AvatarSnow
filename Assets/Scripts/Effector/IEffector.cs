using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEffector
{
    void Init();
    void Deinit();
    void OnMouthOpen();
    void OnMouthClose();
    void OnMouthPuckered();
    void OnMouthUnPuckered();
}
