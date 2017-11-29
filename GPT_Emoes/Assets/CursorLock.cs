using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLock : MonoBehaviour {

    public bool CursorIsLocked = true;

	public bool CanHide = true;

#region Instance

    public static CursorLock instance = null;

    void Awake()
    {
        if (CursorLock.instance == null)
            instance = this;
    }
#endregion

    void Update () {
		if (CanHide) {
			if (Input.GetKeyDown (KeyCode.Escape))
				CursorIsLocked = false;

			if (Input.GetMouseButton (0))
				CursorIsLocked = true;
		}

		Cursor.visible = CursorIsLocked ? false : true;
		Cursor.lockState = CursorIsLocked ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
