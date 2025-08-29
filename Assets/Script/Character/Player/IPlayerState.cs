using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    void KeyInput(Vector3 moveInput, bool runInput, bool sitInput);
    void Move();
}
