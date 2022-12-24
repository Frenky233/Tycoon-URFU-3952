using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCheckerManager : MonoBehaviour
{
    public static MouseCheckerManager instance;

    public bool mouseCheck = false;

    void Awake() { instance = this; }
}
