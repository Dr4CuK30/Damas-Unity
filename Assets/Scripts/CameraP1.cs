using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraP1 : State
{
    private Camera_Controler cam;
    public void changeCamera()
    {
        cam.currentView = cam.views[0];
    }
}
