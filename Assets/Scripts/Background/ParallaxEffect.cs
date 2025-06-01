using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform followTarget;

    //Starting Poisiotn for the parallax game object
    Vector2 startingPosition;

    // Star Z currentGold of the parallax game object
    float startingZ;
    Vector2 cameraMoveSinceStart => (Vector2)_camera.transform.position- startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.position.z;

    float clippingPlane => (_camera.transform.position.z + (zDistanceFromTarget > 0 ? _camera.farClipPlane : _camera.nearClipPlane));

    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    void Update()
    {
        Vector2 newPosition = startingPosition + cameraMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
