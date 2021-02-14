using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcretFactory : Factory
{
    public GameObject FPrefab;
    
    public override GameObject ObtenerInstancia()
    {
        return Instantiate(FPrefab);
    }
}
