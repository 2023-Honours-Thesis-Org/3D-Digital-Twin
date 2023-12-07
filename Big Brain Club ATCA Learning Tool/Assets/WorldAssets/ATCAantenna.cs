using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TelescopeContextMenu;

// TODO: Decide how to approach the difference between 0 degrees in Unity and 0 degrees in the User guide.
public class ATCAantenna : MonoBehaviour
{
    private GameObject contextMenu;
    private GameObject station; // target station
    private Camera telescopePositionCamera;
    private RenderTexture telescopePreview;

    private bool contextToggle = false;

    private float targetAzimuth;
    private float targetElevation;

    private float currentAzimuth;
    private float currentElevation;

    public float minElevation = 12.0f; // degrees, via User Guide
    public float maxElevation = 90.0f; // degrees, via User Guide
    public float minAzimuth = -207.5f; // degrees, via User Guide (page 49)
    public float maxAzimuth = 327.5f; // degrees, via User Guide (page 49)
    public float stowElevation = 85.0f; // degrees, via User Guide (page 123)
    public float stowAzimuth = 90.0f + 180.0f; // Adjusted by 180 because 0 degrees points South in Unity, but North in ATCA
    
    public float maxAzimuthSpeed = 19.0f;  // 19deg/min is the slew rate limit for azimuth, via User Guide
    public float maxElevationSpeed = 38.0f; // 38deg/min is the slew rate limit for elevation, via User Guide
    public float maxBtwnStationSpeed = 15.4f / 3; // m/game sec. Not from User Guide
    public float speedupFactor = 1.0f; // 1 sec in game = this many seconds in the real world

    float junctureX = 90.0f * 15.306f; // the x coordinate of where the juncture is
    float junctureZ = 0.0f; // the z coordinate of where the juncture is

    public GameTime gameTime;
    
    private void OnMouseDown()
    {
        //contextMenu.SetActive(false);
        contextToggle = !contextToggle;
        contextMenu.SetActive(contextToggle);
        if (contextToggle)
        {
            TelescopeContextData uiMod = contextMenu.GetComponent<TelescopeContextData>();
            uiMod.SetAzimuthText("" + this.currentAzimuth);
            uiMod.SetElevationText("" + this.currentElevation);
            uiMod.SetStationText(station.name);
            uiMod.SetTelescopePreview(telescopePreview);
        }
    }

    // Start is called before the first frame update
    // Sets the azimuth and elevation to their at-rest mode, according to the User Guide.
    void Start()
    {
        gameTime = GameObject.Find("GameTime").GetComponent<GameTime>();
        speedupFactor = gameTime.speedFactor;
        telescopePositionCamera = transform.GetChild(1).GetComponent<Camera>();
        telescopePreview = new RenderTexture(256, 256, 32, RenderTextureFormat.Default);
        telescopePreview.Create();

        telescopePositionCamera.targetTexture = telescopePreview;
        
        contextMenu = GameObject.Find("TelescoepContext Menu");
        //contextMenu.SetActive(false);
        
        targetAzimuth = stowAzimuth;
        currentAzimuth = stowAzimuth;
        Transform mount = transform.GetChild(0).GetChild(0);
        mount.localEulerAngles = new Vector3(0.0f, 0.0f, stowAzimuth);

        targetElevation = stowElevation;
        currentElevation = stowElevation;
        Transform disk = mount.GetChild(0);
        disk.localEulerAngles = new Vector3(stowElevation, 0.0f, 0.0f);

        disk.GetChild(1).GetComponent<Camera>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        speedupFactor = gameTime.speedFactor;
        updatePosition(Time.deltaTime);
        updateAzimuth(Time.deltaTime);
        updateElevation(Time.deltaTime);
    }

    
    public void updatePosition(float deltaTime) 
    {
        // UPDATE THE POSITION ALONG THE RAILWAY BY ONE TIME STEP
        if (transform.position != station.transform.position) // if not already at the station
        {
            float EWdist = station.transform.position.x - transform.position.x;
            float NSdist = station.transform.position.z - transform.position.z;
            float deltaDist = maxBtwnStationSpeed * speedupFactor * deltaTime;

            Vector3 newPosition = transform.position;
            if (Mathf.Abs(NSdist) > 0 && Mathf.Abs(EWdist) > 0)
            {
                // For movements going from NS track -> EW track or from EW track -> NS track
                // Set target to be the juncture point
                float intermediateEWdist = junctureX - transform.position.x;
                float intermediateNSdist = junctureZ - transform.position.z;
                // Align in the z direction first, then the x direction.
                if (!(Mathf.Approximately(intermediateNSdist, 0.0f)))
                {
                    // NSdist is nonzero
                    if (Mathf.Abs(intermediateNSdist) > deltaDist)
                    {
                        newPosition.z += Mathf.Sign(intermediateNSdist) * deltaDist;
                    }
                    else 
                    {
                        newPosition.z = junctureZ;
                    }   
                }
                else 
                {
                    // Move along EW direction
                    if (Mathf.Abs(intermediateEWdist) > deltaDist) 
                    {
                        newPosition.x += Mathf.Sign(intermediateEWdist) * deltaDist;
                    }
                    else 
                    {
                        newPosition.x = junctureX;
                    }
                }
            }
            else 
            {
                // Normal order: travel in the x direction first, then the z direction.
                // (EW track -> EW track, NS track -> NS track, EW track -> NS track) 
                if (!(Mathf.Approximately(EWdist, 0.0f)))
                {
                    // Move along EW direction
                    if (Mathf.Abs(EWdist) > deltaDist) 
                    {
                        newPosition.x += Mathf.Sign(EWdist) * deltaDist;
                    }
                    else 
                    {
                        newPosition.x = station.transform.position.x;
                    }
                }
                else 
                {
                    // NSdist is nonzero, so move that direction
                    if (Mathf.Abs(NSdist) > deltaDist) 
                    {
                        newPosition.z += Mathf.Sign(NSdist) * deltaDist;
                    }
                    else 
                    {
                        newPosition.z = station.transform.position.z;
                    }   
                }
            }
            transform.position = newPosition;
        }
    }

    public void updateAzimuth(float deltaTime) 
    {
        // UPDATE THE AZIMUTH ANIMATION BY ONE TIME STEP
        // The Antenna model has just one chain of children. Base -> Mount -> Disk -> Subreflector
        Transform mount = transform.GetChild(0).GetChild(0);
        float azAngleToTravel = targetAzimuth - currentAzimuth;
        // Only move the angle if we need to.
        if (azAngleToTravel != 0.0f)
        {
            float deltaAzimuth = maxAzimuthSpeed / 60 * speedupFactor * deltaTime; 
            // ^ factor of 60 for conversion to degrees/sec because deltaTime is in secs
            if (Mathf.Abs(azAngleToTravel) > deltaAzimuth) 
            {
                currentAzimuth += Mathf.Sign(azAngleToTravel) * deltaAzimuth;
            } 
            else 
            {
                currentAzimuth = targetAzimuth;
            }
            mount.localEulerAngles = new Vector3(0.0f, 0.0f, currentAzimuth);
        }
    }

    public void updateElevation(float deltaTime) 
    {
        // UPDATE THE ELEVATION ANIMATION
        // The Antenna model has just one chain of children. Base -> Mount -> Disk -> Subreflector
        Transform disk = transform.GetChild(0).GetChild(0).GetChild(0);
        float evAngleToTravel = targetElevation - currentElevation;
        // Only move the angle if we need to.
        if (evAngleToTravel != 0.0f)
        {
            float deltaElevation = maxElevationSpeed / 60 * speedupFactor  * deltaTime; 
            // ^ factor of 60 for conversion to degrees/sec because deltaTime is in secs
            if (Mathf.Abs(evAngleToTravel) > deltaElevation) 
            {
                currentElevation += Mathf.Sign(evAngleToTravel) * deltaElevation;
            } 
            else 
            {
                currentElevation = targetElevation;
            }
            disk.localEulerAngles = new Vector3(currentElevation, 0.0f, 0.0f);
        }
    }

    /** Immediately complete any current animations.
     *
     * Antenna will then be in current target azimuth/elevation/position.
     *
     */ 
    public void completeAnimations() 
    {
        // Between-station animations
        transform.position = station.transform.position;

        // Azimuth animation
        Transform mount = transform.GetChild(0).GetChild(0);
        mount.localEulerAngles = new Vector3(0.0f, 0.0f, targetAzimuth);

        // Elevation animation
        Transform disk = mount.GetChild(0);
        disk.localEulerAngles = new Vector3(targetElevation, 0.0f, 0.0f);
    }

    /** Allows users to modify the positions of the antennae on the tracks.
     *
     * @param stationName the name eg "W4" "N14" of the station to which that telescope should move.
     */
    public void moveAntennaTo(string stationName) 
    {
        // Only search and update position if it needs to change.
        if (!string.Equals(stationName, station)) 
        {
            // Don't move we are not CA06 and want to move to the isolated station
            if (!string.Equals(name, "antennaCA06") && string.Equals(stationName, "W392")) 
            {
                Debug.LogWarning("Cannot move to isolated station W392!");
            }
            else 
            {
                // find position of station
                GameObject stationObj = GameObject.Find(stationName);

                // set antenna position to the new station
                if (stationObj is null)
                {
                    Debug.LogWarning("Cannot find station " + stationName + "!");
                } 
                else 
                {
                    station = stationObj;
                }
            }
        }
    }


    /** Allows users to modify the azimuth of an antenna
     *
     * This function calculates the closest wrap angle (between minAzimuth and maxAzimuth) that 
     * will place the telescope facing the angle given as input.
     *
     * Under default wrap limits: 
     * xPossible wrap angles can be divided into three sections: -27.5 to 147.5 degrees (the "low end"), 
     * 147.5 degrees to 332.5 degrees (the middle, or the "no-overlap section"), and 332.5 degrees to 507.5
     * degrees (the "high end"). Note that on the circle, the high end covers the same angles as the low end,
     * so there are two possible wrap angles to achieve the rotation angles in that range, but only one 
     * possible wrap angle to achieve the other rotation angles (the ones from the no-overlap section).
     *
     * @param degreesAzimuth angle to which azimuth should be set, in the range [minAzimuth, maxAzimuth] degrees
     */
    public void setAzimuthTo(float degreesAzimuth) 
    {
        if (degreesAzimuth >= minAzimuth && degreesAzimuth <= maxAzimuth)
        {
            // Adjusted by 180 because 0 degrees points South in Unity, but North in ATCA
            targetAzimuth = degreesAzimuth + 180;
        } 
        else 
        {
            Debug.LogWarning("Invalid azimuth of " + degreesAzimuth + " was requested. Stowing.");
            // targetAzimuth = stowAzimuth; // STOW POSITION
            // targetElevation = stowElevation;
        }
    }
 
    /** Allows users to modify the elevation of an antenna
     *
     * The minimum elevation allowed is 12 degrees.
     *
     * @param degreesElevation angle to which elevation angle should be set, in the range [minElevation, maxElevation] degrees
     */
    public void setElevationTo(float degreesElevation)
    {
        if (degreesElevation >= minElevation && degreesElevation <= maxElevation)
        {
            targetElevation = degreesElevation;
        } 
        else 
        {
            Debug.LogWarning("Invalid elevation of " + degreesElevation + " was requested. Stowing.");
            // targetAzimuth = stowAzimuth; // STOW POSITION
            // targetElevation = stowElevation;
        }
    }

    public float getAzimuth() 
    {
        return targetAzimuth;
    }

    public float getElevation()
    {
        return targetElevation;
    }

    public string getStation()
    {
        return station.name;
    }


    /** Points antenna in the direction of the sky coordinate (real world coordinates, not Unity coordinates).
     *
     * @param skyCoords coordinates to point to (in conventional geocentric right-handed cartesian coordinates 
     * with z axis aligned with North pole and x axis aligned with the intersection of Greenwich meridian and equator)
     */
    public void pointToCoordinates(Vector3 skyCoords)
    {
        Vector3 unityCoords = Coordinates.Sky2Unity(skyCoords);

        lookAt(unityCoords);
    }

    /** Points antenna in the direction of the Unity World coordinate
     *
     * Functions like Tranform.lookAt(), but constrainted to antenna's possible movements: 
     * horizontal rotation of the mount and vertical tilt of the disk, within angle constraints.
     *
     * @param worldPosition coordinates to point to 
     */
    public void lookAt(Vector3 worldPosition) {
        // The Antenna model has just one chain of children. Base -> Mount -> Disk -> Subreflector
        Transform mount = transform.GetChild(0).GetChild(0);
        Transform disk = mount.GetChild(0);
        Vector3 directionVector = worldPosition - disk.position;

        // Find horizontal angle using triangle of horizontal coordinates.
        // (angle is negated for Unity's left-handed coordinate system. 90deg is added for Unity's coordinate system)
        float azimuthOfCoord = -(float) (System.Math.Atan2(directionVector.z, directionVector.x) * 180 / System.Math.PI) + 90.0f;
        azimuthOfCoord = normaliseAngle(azimuthOfCoord, minAzimuth, maxAzimuth);
        setAzimuthTo(azimuthOfCoord);

        Vector3 lateralDirectionVector = new Vector3(directionVector.x, 0.0f, directionVector.z);

        // Find vertical angle using triangle of base/ground distance & height.
        // Consider, for the moment, that the disk is the same height as the mount
        float elevationOfCoord = (float) (System.Math.Atan2(directionVector.y, lateralDirectionVector.magnitude) * 180 / System.Math.PI);
        elevationOfCoord = normaliseAngle(elevationOfCoord, minElevation, maxElevation);
        setElevationTo(elevationOfCoord);
    }

    /** Toggle on or off the camera from the perspective of this antenna
     *
     * @param enable true to enable the camera, false to disable it.
     */
    public void enableCamera(bool enable) 
    {
        // The Antenna model has just one chain of children. Base -> Mount -> Disk -> FOV & Camera
        Transform camera = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1);
        camera.GetComponent<Camera>().enabled = enable;
    }

    /** Takes an angle and returns an equivalent angle that's (inclusively) within some interval.
     *
     * @param
     * angle    - the angle to be converted (in degrees units)
     * minAngle - the lower bound of the interval
     * maxAngle - the upper bound of the interval
     */
    public float normaliseAngle(float angle, float minAngle, float maxAngle)
    {
        float angleDifference;
        float newAngle = angle;
        if (angle > maxAngle)
        {
            angleDifference = (angle - maxAngle) % 360; // get the difference between given angle and max angle modulo 360
            newAngle = maxAngle + angleDifference - 360; // get the largest equivalent angle less than the max angle
        }
        else if (angle < minAngle)
        {
            angleDifference = (minAngle - angle) % 360; // get the difference between given angle and min angle modulo 360
            newAngle = minAngle - angleDifference + 360; // get the smallest equivalent angle greater than the min angle
        }
        if (newAngle > maxAngle || newAngle < minAngle)
        {
            Debug.LogWarning("Angle given has no equivalent value between " + minAngle + " and " + maxAngle);
            return angle;
        }
        return newAngle;
    }

    private void OnDestroy()
    {
        telescopePreview.Release();
    }
}
