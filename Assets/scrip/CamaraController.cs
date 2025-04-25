using Unity.VisualScripting;
using UnityEngine;

public class CamaraController : MonoBehaviour
{

    GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("NinjaPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        float positionx = player.transform.position.x;
        float positiony= player.transform.position.y;
        float positionz= player.transform.position.z-10;
        transform.position= new Vector3 (positionx,positiony,positionz);
    }
}
