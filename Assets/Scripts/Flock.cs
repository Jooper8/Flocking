using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager flockManager; // Refer�ncia para o FlockManager que gerencia o cardume
    float flockSpeed; // Velocidade atual do cardume
    bool isTurning = false; // Indica se o cardume est� virando
    void Start()
    {
        flockSpeed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed); // Define a velocidade inicial do cardume dentro do intervalo especificado no FlockManager
    }
    void Update()
    {
        Bounds bounds = new Bounds(flockManager.transform.position, flockManager.swimLimits * 2); // Define os limites dentro dos quais o cardume pode nadar
        RaycastHit hit;
        Vector3 flockDirection = flockManager.transform.position - transform.position; // Dire��o para a qual o cardume deve se mover
        if (!bounds.Contains(transform.position))
        {
            isTurning = true; // Se o cardume estiver fora dos limites, ele precisa virar
            flockDirection = flockManager.transform.position - transform.position; // Atualiza a dire��o para a posi��o do FlockManager
        }
        else if (Physics.Raycast(transform.position, transform.forward * 50, out hit))
        {
            isTurning = true; // Se houver um obst�culo � frente, o cardume precisa virar
            flockDirection = Vector3.Reflect(transform.forward, hit.normal); // Reflete a dire��o do cardume com base na normal do obst�culo
        }
        else
        {
            isTurning = false; // O cardume n�o est� virando
        }
        if (isTurning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(flockDirection),
                flockManager.rotationSpeed * Time.deltaTime); // Faz o cardume virar gradualmente na dire��o desejada
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                flockSpeed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed); // Altera aleatoriamente a velocidade do cardume com uma probabilidade de 10%
            }
            if (Random.Range(0, 100) < 20)
            {
                ApplyFlockRules(); // Aplica as regras de comportamento do cardume com uma probabilidade de 20%
            }
        }
        transform.Translate(0, 0, Time.deltaTime * flockSpeed); // Move o cardume na dire��o atual com base na velocidade atual
    }
    void ApplyFlockRules()
    {
        GameObject[] allFlocks;
        allFlocks = flockManager.allFlock; // Obt�m todos os objetos do cardume
        Vector3 flockCenter = Vector3.zero; // Centro do cardume
        Vector3 flockAvoidance = Vector3.zero; // Dire��o de evita��o do cardume
        float flockAvgSpeed = 0.01f; // Velocidade m�dia do cardume
        float neighborDistance; // Dist�ncia entre os objetos do cardume
        int flockSize = 0; // Quantidade de objetos no cardume
        foreach (GameObject flock in allFlocks)
        {
            if (flock != this.gameObject)
            {
                neighborDistance = Vector3.Distance(flock.transform.position, transform.position);

                if (neighborDistance <= flockManager.neighborDistance) // Verifica se o objeto do cardume est� dentro da dist�ncia de vizinhan�a
                {
                    flockCenter += flock.transform.position;
                    flockSize++;

                    if (neighborDistance < 1f)
                    {
                        flockAvoidance += (transform.position - flock.transform.position); // Calcula a dire��o de evita��o caso os objetos estejam muito pr�ximos
                    }

                    Flock anotherFlock = flock.GetComponent<Flock>();
                    flockAvgSpeed += anotherFlock.flockSpeed; // Calcula a velocidade m�dia do cardume
                }
            }
            if (flockSize > 0)
            {
                flockCenter /= flockSize; // Calcula o centro do cardume dividindo a soma das posi��es dos objetos pela quantidade de objetos
                flockSpeed = flockAvgSpeed / flockSize; // Define a velocidade do cardume como a velocidade m�dia dos objetos
                Vector3 direction = (flockCenter - flockAvoidance) - transform.position; // Calcula a dire��o para a qual o cardume deve se mover

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), flockManager.rotationSpeed * Time.deltaTime); // Faz o cardume virar gradualmente na dire��o desejada
                }
            }
        }
    }
}