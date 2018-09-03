using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuboXAgente : Agent {

    public GameObject Objetivo;
    public float Speed = 5;

    public override void AgentReset()
    {
        gameObject.transform.localPosition = Vector3.zero;
        Objetivo.transform.localPosition = ObtenerPosicionObjetivo(cubeXAcademy.separacion);
            
    }

    private Vector3 ObtenerPosicionObjetivo(float separacion)
    {
        return new Vector3(Random.Range(1.05f, 7f) * (Random.value <= 0.5 ? 1 : -1), 0, Random.Range(1f, 9f) * (Random.value <= 0.5 ? 1 : -1));
    }

    //Aqui la red neuronal identifica la posicion del agente y el objetivo
    public override void CollectObservations()
    {
        //nomralizacion de datos de entrada, para que esten entre 1 y -1
        Vector3 PosicionRelativa = Objetivo.transform.localPosition - transform.localPosition;

        AddVectorObs(PosicionRelativa.x / 15f);
        AddVectorObs(PosicionRelativa.z / 15f);

        //Quaternion relacionado a la rotacion
        /*
        Quaternion q = Quaternion.identity;

        AddVectorObs(q.eulerAngles/360f);
        */
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //float distanciaInicialAlObjetivo = Vector3.Distance(Objetivo.transform.localPosition, transform.localPosition);

        float horizontal = 0, vertical = 0;


        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            
            horizontal = Mathf.RoundToInt(Mathf.Clamp(vectorAction[0], -1, 1));
            vertical = Mathf.RoundToInt(Mathf.Clamp(vectorAction[1], -1, 1));

        }

        else if (brain.brainParameters.vectorActionSpaceType == SpaceType.discrete)
        {

            switch ((int)vectorAction[0])
            {
                case 0:
                    horizontal = 1;
                    break;
                case 1:
                    horizontal = -1;
                    break;
                case 2:
                    vertical = 1;
                    break;
                case 3:
                    vertical = -1;
                    break;


            }

        }

        float newX = transform.localPosition.x + (horizontal * Speed * Time.deltaTime);
        newX = Mathf.Clamp(newX, -9.2f, 9.2f);

        float newZ = transform.localPosition.z + (vertical * Speed * Time.deltaTime);
        newZ = Mathf.Clamp(newZ, -9.2f, 9.2f);

        //Debug.LogFormat("{0} {1}",vectorAction[0], vectorAction[1]);

        transform.localPosition = new Vector3(newX, 0, newZ);

        /*float distanciaFinalAlObjetivo = Vector3.Distance(Objetivo.transform.localPosition, transform.localPosition);

        if (distanciaFinalAlObjetivo < distanciaInicialAlObjetivo)
        {
            AddReward(0.1f);
        }
        else
        {
            AddReward(-0.3f);
        }
        */

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objetivo"))
        {
            AddReward(30f);
            Done();
        }
    }
}
