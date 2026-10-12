using UnityEngine;

public class Webcam : MonoBehaviour
{
    [SerializeField] private Material camMaterial;
    [SerializeField] private string camName = "HD Webcam";

    void Start()
    {
        // Log out all webcams adn then add one syou're using

        WebCamDevice[] devices = WebCamTexture.devices;

        // Logging out each webcam

        Debug.Log("Available Webcams:");
        foreach (WebCamDevice webcam in devices)
        {
            Debug.Log(webcam.name);

        }

        // Retrieving webcam by name 
        // Can use new() to get default - pc can have multiple but mobile cannot

        // Pass webcam name, and resolution and fps
        // Inherits from texture
        WebCamTexture camTexture = new(camName, 1920, 1080, 30);

        // Making material texture as webcam texture
        camMaterial.mainTexture = camTexture;

        // Begin cam

        // stop and pause exist

        camTexture.Play();



    }

}
