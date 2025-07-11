using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveAnnouncer : MonoBehaviour
{

    private SpawnManagerScript sps;
    [SerializeField] TextMeshProUGUI waveText;
    // Update is called once per frame

    void Start()
    {
        sps = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManagerScript>();
    }
    void Update()
    {
        waveText.text = "Wave: " + sps.waveCounter.ToString();
    }
}
