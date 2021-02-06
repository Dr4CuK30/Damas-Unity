using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controler : MonoBehaviour
{
    public Transform[] views;
    public Transform currentView;
    public float transitionSpeed;
    public Tablero_Script tablero;

    // Start is called before the first frame update
    void Start()
    {
        currentView = transform;
    }

    // Update is called once per frame
    void Update()
    {
        CambiarCamara(tablero.TurnoBlanco);
    }

    void LateUpdate()
    {
        TranformCamera();
    }

    void TranformCamera()
    {
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);
        Vector3 currentAngle = new Vector3(
            Mathf.Lerp(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
            Mathf.Lerp(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
            Mathf.Lerp(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed)
        );
        transform.eulerAngles = currentAngle;
    }
    public void CambiarCamara(bool TurnoBlanco)
    {
        //Prueba cambio de camara
        if (TurnoBlanco)
        {
            currentView = views[0];
        }
        else
        {
            currentView = views[1];
        }
    }
}
