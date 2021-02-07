using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tablero_Script : MonoBehaviour
{
    public Ficha[,] fichas = new Ficha[8,8];
    public GameObject FBlancaPrefab;
    public GameObject FNegraPregab;
    private Vector2 mouseOver;
    public bool TurnoBlanco;
    private Vector2 OffsetRaycast = new Vector2(0.510f, 0.510f);

    private Vector2 PInicial;
    private Vector2 PFinal;
    private Ficha FichaSelec;
    private bool haMatado;
    private bool puedeVolverAMatar;
    
    public Text contadorTiempo;
    private float segundos;
    private int minutos;
    private bool movimientoBloqueado;

    private void Start()
    {
        movimientoBloqueado = false;
        segundos = 0.0f;
        minutos = 0;
        TurnoBlanco = true;
        GenerarTablero(); 
    }

    private void Update()
    {
        contadorUpdate();
        UpdateMousePos();
        int x = (int)mouseOver.x;
        int y = (int)mouseOver.y;

        if(FichaSelec!=null && movimientoBloqueado == false)
        {
            UpdateAgarreFicha(FichaSelec);
        }

        if(Input.GetMouseButtonDown(0) && movimientoBloqueado == false)
        {
            SeleccionarFicha(x, y);
        }

        if(Input.GetMouseButtonUp(0) && movimientoBloqueado == false)
        {
            DesplazarFicha((int)PInicial.x,(int)PInicial.y,x,y);
        }
    }

    private void UpdateMousePos()
    {
        //Validación para verificar que existe una cámara (en caso de borrarla o cambiarla)
        if(!Camera.main)
        {
            Debug.Log("Cámara no encontrada");
        }

        //RaycastHit devuelve la información provista por un Raycast al golpear un objeto que posea un collider
        //Raycast es un "rayo" o un proyectil que se dirige en una dirección
        //Se lanzará un Raycast desde la cámara con una dirección dada hacia donde apunta el mouse, con la intención de que este
        //colisione con una de las posiciones del tablero, para que así se pueda seleccionar la ficha a mover:

        RaycastHit raycastHit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 25f, LayerMask.GetMask("Tablero")))
        {
            mouseOver.x = (int)(raycastHit.point.x + OffsetRaycast.x);
            mouseOver.y = (int)(raycastHit.point.z + OffsetRaycast.y);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    public void GenerarTablero()
    {
        //Lado Blanco:
        for(int fila = 0; fila < 3; fila++)
        {
            for(int columna = 0; columna < 8; columna+=2)
            {
                //Crear Piezas
                GenerarPieza((fila%2==0)?columna:columna+1,fila, FBlancaPrefab);
            }
        }

        //Lado Negro:
        for (int fila = 7; fila > 4; fila--)
        {
            for (int columna = 0; columna < 8; columna += 2)
            {
                //Crear Piezas
                GenerarPieza((fila % 2 == 0) ? columna : columna + 1, fila, FNegraPregab);
            }
        }
    }

    public void GenerarPieza(int x, int y, GameObject prefab)
    {
        //Creación de "clones" de la ficha:
        GameObject piezaPre = Instantiate(prefab) as GameObject;
        //Accede a la propiedad Transform del GameObject pierzaPre, y define al padre de dicha propiedad como transform
        //Esto para que las piezas sean hijas del tablero
        piezaPre.transform.SetParent(transform);
        //Obtiene el Primer Componente de Tipo Ficha del GameObject piezaPre (Asigna el clon como ficha)
        Ficha ficha = piezaPre.GetComponent<Ficha>();
        fichas[x,y] = ficha;
        //Desplaza a la ficha cierta distancia al momento de crearla:
        PosicionarFicha(ficha,x,y);
    }

    public void PosicionarFicha(Ficha ficha, int x, int y)
    {
        ficha.transform.position = (Vector3.right * x) + (Vector3.forward * y);
        Debug.Log((Vector3.right * x) + (Vector3.forward * y));
    }

    public void SeleccionarFicha(int x, int y)
    {
        //En caso de pulsar fuera de los límites del tablero:

        if(x<0 || x>=fichas.Length || y<0 || y>=fichas.Length)
        {
            return;
        }

        //Se obtiene la ficha seleccionada del arreglo de fichas del tablero:
        Ficha ficha = fichas[x, y];
        if(ficha!=null)
        {
            if((ficha.FBlanca && TurnoBlanco) || (!ficha.FBlanca && !TurnoBlanco))
            {
                FichaSelec = ficha;
                PInicial = mouseOver;
            }
            //Debug.Log("Ficha Seleccionada:" + "("+x+","+y+")");
        }
    }

    public void DesplazarFicha(int PIniX, int PIniY, int PFinX, int PFinY)
    {
        haMatado = false;
        puedeVolverAMatar = false;
        PInicial = new Vector2(PIniX, PIniY);
        PFinal = new Vector2(PFinX,PFinY);
        //FichaSelec = fichas[PIniX,PIniY];

        //Condicional para evitar que se tome una posición fuera del tablero, tanto para tomar lo ficha como para dejarla:
        if(PFinX<0 || PFinX> fichas.Length || PFinY < 0 || PFinY > fichas.Length)
        {
            if(FichaSelec!=null)
            {
                //En caso de que se tenga un ficha seleccionada y se suelte en una posición incorrecta, será devuelta a su posición inicial:
                PosicionarFicha(FichaSelec,PIniX,PIniY);
            }

            PInicial = Vector2.zero;
            FichaSelec = null;
            return;
        }

        //Validación en caso de cancelar el movimiento (Seleccionar la misma casilla como posición final e inicial.)
        if(FichaSelec!=null)
        {
            if(PFinal==PInicial)
            {
                PosicionarFicha(FichaSelec,PIniX,PIniY);

                PInicial = Vector2.zero;
                FichaSelec = null;
                return;
            }

            //Validar desplazamiento según las reglas(En diagonal):
            if(FichaSelec.ValidarMovimiento(fichas, PIniX, PIniY, PFinX, PFinY))
            {
                Debug.Log(FichaSelec.ValidarMovimiento(fichas, PIniX, PIniY, PFinX, PFinY));
                //Validación para confirmar si fue un movimiento de asesinato o no:
                //(Siendo un salto de dos posiciones)
                if(Mathf.Abs(PFinX - PIniX)==2)
                {
                    haMatado = true;
                    ComprobarSiMataAgain(PFinX,PFinY);
                    Ficha ficha = fichas[(PIniX + PFinX)/2, (PIniY+ PFinY)/2];
                    //Destrucción de la pieza si no es nula:
                    if(ficha!=null)
                    {
                        //Elimina la ficha del tablero
                        fichas[(PIniX + PFinX) / 2, (PIniY + PFinY) / 2] = null;
                        //Destruye la ficha:
                        Destroy(ficha.gameObject);
                    }
                    
                }
                //Se define la nueva posición de la ficha en el tablero
                fichas[PFinX, PFinY] = FichaSelec;
                //Se deja el espacio vacío dejado en la posición inicial de movimiento:
                fichas[PIniX, PIniY] = null;
                //Se posiciona la pieza:
                PosicionarFicha(FichaSelec, PFinX, PFinY);
                FichaSelec = null;
                PInicial = Vector2.zero;
                if (!haMatado || (haMatado && !puedeVolverAMatar))
                {
                    StartCoroutine(TerminarTurnoDelay());
                }

            }
            else
            {
                //De no ser así, se reinicia la posición de la ficha:
                PosicionarFicha(FichaSelec, PIniX, PIniY);

                PInicial = Vector2.zero;
                FichaSelec = null;
                return;
            }
        }
    }

    public void UpdateAgarreFicha(Ficha ficha)
    {
        //Validación para verificar que existe una cámara (en caso de borrarla o cambiarla)
        if (!Camera.main)
        {
            Debug.Log("Cámara no encontrada");
        }

        RaycastHit raycastHit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 25f, LayerMask.GetMask("Tablero")))
        {
            //Esto hará que cuando la ficha sea agarrada aumente sus coordenadas en y, por lo que se elevará mientras el mouse la sostenga
            //De igual forma desplazara la ficha junto con el cursor:
            ficha.transform.position = raycastHit.point + Vector3.up;
        }
    }

    public void TerminarTurno()
    {
        if (TurnoBlanco)
        {
            TurnoBlanco = false;
        }
        else
        {
            TurnoBlanco = true;
        }
    }
    public void ComprobarSiMataAgain(int PFinX, int PFinY)
    {
        if (TurnoBlanco && PFinY <= 5)
        {
            if (PFinX < 2 && fichas[PFinX + 1, PFinY + 1] != null)
            {
                if (!fichas[PFinX + 1, PFinY + 1].FBlanca && fichas[PFinX + 2, PFinY + 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
            if (PFinX > 5 && fichas[PFinX - 1, PFinY + 1] != null)
            {
                if (!fichas[PFinX - 1, PFinY + 1].FBlanca && fichas[PFinX - 2, PFinY + 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
            if (PFinX >= 2 && PFinX <= 5 && fichas[PFinX - 1, PFinY + 1] != null)
            {
                if (!fichas[PFinX - 1, PFinY + 1].FBlanca && fichas[PFinX - 2, PFinY + 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
            if (PFinX >= 2 && PFinX <= 5 && fichas[PFinX + 1, PFinY + 1] != null)
            {
                if (!fichas[PFinX + 1, PFinY + 1].FBlanca && fichas[PFinX + 2, PFinY + 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
        }
        if (!TurnoBlanco && PFinY >= 2)
        {
            if (PFinX < 2 && fichas[PFinX + 1, PFinY - 1] != null)
            {
                if (fichas[PFinX + 1, PFinY - 1].FBlanca && fichas[PFinX + 2, PFinY - 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
            if (PFinX > 5 && fichas[PFinX - 1, PFinY - 1] != null)
            {
                if (fichas[PFinX - 1, PFinY - 1].FBlanca && fichas[PFinX - 2, PFinY - 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
            if (PFinX >= 2 && PFinX <= 5 && fichas[PFinX - 1, PFinY - 1] != null)
            {
                if (fichas[PFinX - 1, PFinY - 1].FBlanca && fichas[PFinX - 2, PFinY - 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
            if (PFinX >= 2 && PFinX <= 5 && fichas[PFinX + 1, PFinY - 1] != null)
            {
                if (fichas[PFinX + 1, PFinY - 1].FBlanca && fichas[PFinX + 2, PFinY - 2] == null)
                {
                    puedeVolverAMatar = true;
                }
            }
        }
    }

    public void contadorUpdate()
    {
        segundos += Time.deltaTime;
        if (segundos >= 60)
        {
            minutos++;
            segundos = 0.0f;
        }
        contadorTiempo.text = "Tiempo: " + minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    public IEnumerator TerminarTurnoDelay()
    {
        movimientoBloqueado = true;
        yield return new WaitForSeconds(1);
        TerminarTurno();
        yield return new WaitForSeconds(1);
        movimientoBloqueado = false;
    }
}
