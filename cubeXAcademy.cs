using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeXAcademy : Academy {

    public static float separacion;

    public override void AcademyReset()
    {
        separacion = resetParameters["separacion"];
    }

}
