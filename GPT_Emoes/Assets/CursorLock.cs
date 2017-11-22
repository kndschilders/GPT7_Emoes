using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour {

    public bool CursorIsLocked = true;

#region Instance

    public static CursorLock instance = null;

    void Awake()
    {
        if (CursorLock.instance == null)
            instance = this;
    }
#endregion

    void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape))
        {
            CursorIsLocked = false;
        }
        if(Input.GetMouseButton(0))
        {
            CursorIsLocked = true;
        }

        if(CursorIsLocked)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
	}
}
