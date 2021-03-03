using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraP2 : State
{
    private Camera_Controler cam;
    public void changeCamera()
    {
        cam.currentView = cam.views[1];
    }
}
