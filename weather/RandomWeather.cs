
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomWeather : MonoBehaviour
{

    [Header("Set up: ")]
    [SerializeField] private GameObject[] weathers;
    [SerializeField] private AudioSource[] audios;

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

            if(!audios[index].isPlaying){
                audios[index].Play();
            }

            waitTime = 0;
        }

        if(waitTime >= endTime && weathers[index].activeInHierarchy){
            weathers[index].SetActive(false);

            audios[index].Stop();

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
