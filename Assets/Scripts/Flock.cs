using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager flockManager; // Referência para o FlockManager que gerencia o cardume
    float flockSpeed; // Velocidade atual do cardume
    bool isTurning = false; // Indica se o cardume está virando
    void Start()
    {
        flockSpeed = Random.Range(flockManager.minSpeed, flockManager.maxSpeed); // Define a velocidade inicial do cardume dentro do intervalo especificado no FlockManager
    }
    void Update()
    {
        Bounds bounds = new Bounds(flockManager.transform.position, flockManager.swimLimits * 2); // Define os limites dentro dos quais o cardume pode nadar
        RaycastHit hit;
        Vector3 flockDirection = flockManager.transform.position - transform.position; // Direção para a qual o cardume deve se mover
        if (!bounds.Contains(transform.position))
        {
            isTurning = true; // Se o cardume estiver fora dos limites, ele precisa virar
            flockDirection = flockManager.transform.position - transform.position; // Atualiza a direção para a posição do FlockManager
        }
        else if (Physics.Raycast(transform.position, transform.forward * 50, out hit))
        {
            isTurning = true; // Se houver um obstáculo à frente, o cardume precisa virar
            flockDirection = Vector3.Reflect(transform.forward, hit.normal); // Reflete a direção do cardume com base na normal do obstáculo
        }
        else
        {
            isTurning = false; // O cardume não está virando
        }
        if (isTurning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(flockDirection),
                flockManager.rotationSpeed * Time.deltaTime); // Faz o cardume virar gradualmente na direção desejada
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
        transform.Translate(0, 0, Time.deltaTime * flockSpeed); // Move o cardume na direção atual com base na velocidade atual
    }
    void ApplyFlockRules()
    {
        GameObject[] allFlocks;
        allFlocks = flockManager.allFlock; // Obtém todos os objetos do cardume
        Vector3 flockCenter = Vector3.zero; // Centro do cardume
        Vector3 flockAvoidance = Vector3.zero; // Direção de evitação do cardume
        float flockAvgSpeed = 0.01f; // Velocidade média do cardume
        float neighborDistance; // Distância entre os objetos do cardume
        int flockSize = 0; // Quantidade de objetos no cardume
        foreach (GameObject flock in allFlocks)
        {
            if (flock != this.gameObject)
            {
                neighborDistance = Vector3.Distance(flock.transform.position, transform.position);

                if (neighborDistance <= flockManager.neighborDistance) // Verifica se o objeto do cardume está dentro da distância de vizinhança
                {
                    flockCenter += flock.transform.position;
                    flockSize++;

                    if (neighborDistance < 1f)
                    {
                        flockAvoidance += (transform.position - flock.transform.position); // Calcula a direção de evitação caso os objetos estejam muito próximos
                    }

                    Flock anotherFlock = flock.GetComponent<Flock>();
                    flockAvgSpeed += anotherFlock.flockSpeed; // Calcula a velocidade média do cardume
                }
            }
            if (flockSize > 0)
            {
                flockCenter /= flockSize; // Calcula o centro do cardume dividindo a soma das posições dos objetos pela quantidade de objetos
                flockSpeed = flockAvgSpeed / flockSize; // Define a velocidade do cardume como a velocidade média dos objetos
                Vector3 direction = (flockCenter - flockAvoidance) - transform.position; // Calcula a direção para a qual o cardume deve se mover

                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), flockManager.rotationSpeed * Time.deltaTime); // Faz o cardume virar gradualmente na direção desejada
                }
            }
        }
    }
}