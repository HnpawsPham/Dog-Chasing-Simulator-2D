
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWeather : MonoBehaviour
{

    [SerializeField] private GameObject[] weathers;
    private float appearTime;
    private float endTime;
    private int index;
    private float waitTime;

    void Start()
    {
        CreateRandom();
    }


    void Update()
    {
        waitTime += Time.deltaTime;

        if(waitTime >= appearTime && !weathers[index].activeInHierarchy){
            weathers[index].SetActive(true);


            waitTime = 0;
        }

        if(waitTime >= endTime && weathers[index].activeInHierarchy){
            weathers[index].SetActive(false);
            waitTime = 0;

            CreateRandom();
        }
    }
    private void CreateRandom()
    {
        index = Random.Range(0, weathers.Length);
        appearTime = Random.Range(10, 100);
        endTime = Random.Range(10, 40);
    }
}