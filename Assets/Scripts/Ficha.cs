using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ficha : MonoBehaviour
{
    public bool FBlanca;
    public bool FReina;


    public bool ValidarMovimiento(Ficha[,] fichas, int xinicial, int yinicial, int xfinal, int yfinal)
    {
        //Debug.Log("No Matanza Obl");
        //Validar que se está moviendo sobre una ficha (Asesinato):
        //1) No es posible moverse encima de una ficha:
        if (fichas[xfinal, yfinal] != null)
        {
            return false;
            
        }
        //Se calcula el desplazamiento en x:
        int deltaX = Mathf.Abs(xinicial- xfinal);

        //Se calcula el desplazamiento en y:
        //(En este caso se requiere tanto el valor negativo como el positivo, pues este delta se usará tanto para
        //el desplazamiento de las fichas blanca, negras y reinas)
        int deltaY = yfinal - yinicial;

        //Se valida si la ficha es reina o blanca:
        if(FBlanca || FReina)
        {
         
            if(deltaX == 1 && deltaY == 1)
            {
                return true;
                //Si la pieza se mueve 2 posiciones horizontalmente(Esto para el caso del asesinato)
            } else if(deltaX == 2 && deltaY == 2)
            {
                //Se obtiene la ficha que supuestamente fue asesinada por el desplazamiento de la ficha movida:
                Ficha ficha = fichas[(xinicial + xfinal) / 2, (yinicial + yfinal) / 2];
                //Valida que la ficha saltada de verdad exista y que no sea una ficha del mismo equipo
                if (ficha != null && !ficha.FBlanca)
                {
                    return true;
                }
            }
        }

        //Se valida si la ficha es reina o negra:
        if (!FBlanca || FReina)
        {
            //Si la pieza se mueve un cuadro horizontalmente hacia cualquier dirección:
            if (deltaX == 1)
            {
                //Si la pieza se mueve un cuadro verticalmente hacia atrás
                if (deltaY == -1)
                {
                    //Representa un movimiento diagonal por lo que es valido
                    return true;
                }
                //Si la pieza se mueve 2 posiciones horizontalmente(Esto para el caso del asesinato)
            }
            else if (deltaX == 2)
            {
                if (deltaY == -2)
                {
                    //Se obtiene la ficha que supuestamente fue asesinada por el desplazamiento de la ficha movida:
                    Ficha ficha = fichas[(xinicial + xfinal) / 2, (yinicial + yfinal) / 2];

                    //Valida que la ficha saltada de verdad exista y que no sea una ficha del mismo equipo
                    if (ficha != null && ficha.FBlanca)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //En caso de que sea obligatorio matar

    public bool ValidarMovimiento(Ficha[,] fichas, int xinicial, int yinicial, int xfinal, int yfinal, List<int> xobl, List<int> yobl)
    {
        //Validar que se está moviendo sobre una ficha (Asesinato):
        //1) No es posible moverse encima de una ficha:
        if (fichas[xfinal, yfinal] != null)
        {
            return false;

        }

              
        //Se calcula el desplazamiento en x:
        int deltaX = Mathf.Abs(xinicial - xfinal);

        //Se calcula el desplazamiento en y:
        //(En este caso se requiere tanto el valor negativo como el positivo, pues este delta se usará tanto para
        //el desplazamiento de las fichas blanca, negras y reinas)
        int deltaY = yfinal - yinicial;

        //Se valida que se este moviendo la ficha que tiene que matar obligatoriamente
        if (!xobl.Contains(xinicial)|| !yobl.Contains(yinicial) || deltaX !=2 || Mathf.Abs(deltaY) != 2)
        {
            return false;
        }

        //Se valida si la ficha es reina o blanca:
        if (FBlanca || FReina)
        {

            if (deltaX == 1 && deltaY == 1)
            {
                return true;
                //Si la pieza se mueve 2 posiciones horizontalmente(Esto para el caso del asesinato)
            }
            else if (deltaX == 2 && deltaY == 2)
            {
                //Se obtiene la ficha que supuestamente fue asesinada por el desplazamiento de la ficha movida:
                Ficha ficha = fichas[(xinicial + xfinal) / 2, (yinicial + yfinal) / 2];
                //Valida que la ficha saltada de verdad exista y que no sea una ficha del mismo equipo
                if (ficha != null && !ficha.FBlanca)
                {
                    return true;
                }
            }
            if (deltaX == 2 && deltaY == -2 && FReina)
            {
                //Se obtiene la ficha que supuestamente fue asesinada por el desplazamiento de la ficha movida:
                Ficha ficha = fichas[(xinicial + xfinal) / 2, (yinicial + yfinal) / 2];

                //Valida que la ficha saltada de verdad exista y que no sea una ficha del mismo equipo
                if (ficha != null && !ficha.FBlanca)
                {
                    return true;
                }
            }
        }

        //Se valida si la ficha es reina o negra:
        if (!FBlanca || FReina)
        {
            //Si la pieza se mueve un cuadro horizontalmente hacia cualquier dirección:
            if (deltaX == 1)
            {
                //Si la pieza se mueve un cuadro verticalmente hacia atrás
                if (deltaY == -1)
                {
                    //Representa un movimiento diagonal por lo que es valido
                    return true;
                }
                //Si la pieza se mueve 2 posiciones horizontalmente(Esto para el caso del asesinato)
            }
            else if (deltaX == 2)
            {
                if (deltaY == -2)
                {
                    //Se obtiene la ficha que supuestamente fue asesinada por el desplazamiento de la ficha movida:
                    Ficha ficha = fichas[(xinicial + xfinal) / 2, (yinicial + yfinal) / 2];

                    //Valida que la ficha saltada de verdad exista y que no sea una ficha del mismo equipo
                    if (ficha != null && ficha.FBlanca)
                    {
                        return true;
                    }
                }

                if(deltaY == 2 && FReina)
                {
                    Debug.Log("Matanza Obl" + xobl[0] + yobl[0]);
                    //Se obtiene la ficha que supuestamente fue asesinada por el desplazamiento de la ficha movida:
                    Ficha ficha = fichas[(xinicial + xfinal) / 2, (yinicial + yfinal) / 2];

                    //Valida que la ficha saltada de verdad exista y que no sea una ficha del mismo equipo
                    if (ficha != null && ficha.FBlanca)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
