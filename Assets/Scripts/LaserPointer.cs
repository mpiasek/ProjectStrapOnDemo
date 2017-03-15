using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour {
    // Transform of the camera rig
    public Transform cameraRigTransform;
    // Reference to teleport reticle prefab
    public GameObject teleportReticlePrefab;
    // Reference to instance of reticle
    private GameObject reticle;
    // Reference to reticle transform properties
    private Transform teleportReticleTransform;
    // Reference to player's head (camera)
    public Transform headTransform;
    // Reticle offset from the floor
    public Vector3 teleportReticleOffset;
    // Layer mask to filter the areas on which teleports are allowed
    public LayerMask teleportMask;
    // Set to true when valid teleport location is found
    private bool shouldTeleport;

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }
    // Reference to laser prefab
    public GameObject laserPrefab;
    // Reference to instance of laser
    private GameObject laser;
    // Transform component of laser
    private Transform laserTransform;
    // Position where the laser hits
    private Vector3 hitPoint;
    private void ShowLaser(RaycastHit hit)
    {
        // Show the laser
        laser.SetActive(true);
        // Position the laser between the controller and the point where the raycast hits
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f);
        // Point the laser at the position the raycast hit
        laserTransform.LookAt(hitPoint);
        // Scale the laser so it fits perfectly between the two locations
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance);
    }
    private void Teleport()
    {
        // Set flag to false while teleporting
        shouldTeleport = false;
        // Hide the reticle
        reticle.SetActive(false);
        // Get difference between position of camera rig center and player's head
        Vector3 difference = cameraRigTransform.position - headTransform.position;
        // Reset y-position for the above difference to 0, as the calculation doesn't take into account vertical
        // position of player's head
        difference.y = 0;
        // Move camera rig to hit point plus difference. W/o difference, player will teleport to incorrect location
        cameraRigTransform.position = hitPoint + difference;
    }
    // Update is called once per frame
    void Start ()
    {
        // Spawn a new laser and save a reference to it
        laser = Instantiate(laserPrefab);
        // Store the laser's transform component;
        laserTransform = laser.transform;
        // Spawn new reticle and save reference
        reticle = Instantiate(teleportReticlePrefab);
        // Reference to reticles transform component
        teleportReticleTransform = reticle.transform;
    }
    void Update () {
        // Check if touchpad is held down
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;
            // Shoot a ray from controller, if it hits store the hit point and show laser
            if (Physics.Raycast(trackedObj.transform.position, transform.forward, out hit, 100, teleportMask))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
                // Show teleport reticle
                reticle.SetActive(true);
                // Move reticle to where the raycast hit plus offset
                teleportReticleTransform.position = hitPoint + teleportReticleOffset;
                // Indicate a valid position for teleporting has been found
                shouldTeleport = true;
            }
        }
        // Hide reticle in abscence of valid teleport location
        else
            reticle.SetActive(false);
        // If touchpad is released and a valid teleport location has been found, teleport
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport)
        {
            Teleport();
        }
    
    }
}
