using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject flockPrefab; // Prefab do cardume
    public int numFlocks = 20; // Número de cardumes
    public GameObject[] allFlock; // Array que armazena todos os objetos do cardume
    public Vector3 swimLimits = new Vector3(5, 5, 5); // Limites dentro dos quais o cardume pode nadar
    public Vector3 goalPosition; // Posição do objetivo do cardume
    public GameObject goal; // Objeto do objetivo

    [Header("Bird Configuration")]
    [Range(0f, 5f)]
    public float minSpeed; // Velocidade mínima dos cardumes
    [Range(0f, 5f)]
    public float maxSpeed; // Velocidade máxima dos cardumes
    [Range(1f, 10f)]
    public float neighborDistance; // Distância entre os cardumes para determinar a vizinhança
    [Range(5f, 5f)]
    public float rotationSpeed; // Velocidade de rotação dos cardumes

    void Start()
    {
        allFlock = new GameObject[numFlocks]; // Inicializa o array com o tamanho definido pelo número de cardumes
        for (int i = 0; i < numFlocks; i++)
        {
            Vector3 position = transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x), // Gera uma posição aleatória dentro dos limites especificados
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            allFlock[i] = Instantiate(flockPrefab, position, Quaternion.identity); // Instancia o cardume no cenário na posição gerada
            allFlock[i].GetComponent<Flock>().flockManager = this; // Define o FlockManager deste cardume como o FlockManager atual
        }
        goalPosition = transform.position; // Define a posição do objetivo como a posição atual do FlockManager
    }

    void Update()
    {
        goalPosition = transform.position; // Atualiza a posição do objetivo como a posição atual do FlockManager

        if (Random.Range(0, 100) < 10) // Com uma probabilidade de 10%, muda a posição do objetivo
        {
            goalPosition = transform.position + new Vector3(
                Random.Range(-swimLimits.x, swimLimits.x), // Gera uma nova posição aleatória dentro dos limites especificados
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
