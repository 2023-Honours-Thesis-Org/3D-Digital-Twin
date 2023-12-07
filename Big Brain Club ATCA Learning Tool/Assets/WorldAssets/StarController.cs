using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class StarController : MonoBehaviour
{
    /* Via tutorials: https://stackoverflow.com/questions/35909129/particles-to-create-star-field-in-unity
       & https://medium.com/microsoft-design/building-a-virtual-sky-883d4d1080f4
       & very much 64bleat's https://github.com/64bleat/AstronomicSkybox-Unity, because I very much struggle
       understanding what's going on with astronomic coordinates and rotating them to match a particular lat/long on earth.
       64bleat's package already does that, just doesn't work on HDRP project with the scale of our tool.
    */

    public int maxParticle = 10000;
    public int starfieldRadius = 3500;
    public string starDataFile = "hygdata_extract";

    [SerializeField]
    private Vector2[] eqPositions; // positions of stars at J2000 in equatorial coordinate system
    [SerializeField]
    private ParticleSystem.Particle[] points; 
    private int realNPoints; // in case there's less than maxParticle stars in the datafile.
    //private ParticleSystem particleSystem;

    /* Via AstronomicSkybox-Unity: convert string to float, or null if fails.
    */
    private static float? ParseFloat(string s) => float.TryParse(s, out float f) ? (float?)f : null;

    /** Generate stars from starFile as a Particle System.
     *
     * The stars are placed equidistant from the origin on a ball of radius starfieldRadius
     * and at most maxParticle of them will be drawn;
     */
    private void CreateStars() 
    {

        TextAsset starData = (TextAsset)Resources.Load(starDataFile, typeof(TextAsset));
        if (!starData) 
        {
            Debug.LogError("Star data failed to load\n");
            return;
        }

        // else we have star data file.
        int i = 0;
        points = new ParticleSystem.Particle[maxParticle];
        eqPositions = new Vector2[maxParticle];

        foreach (string ln in starData.text.Split('\n'))
        {
            string[] data = ln.Split(',');

            float? ra = ParseFloat(data[1]);
            float? dec = ParseFloat(data[2]);
            float? mag = ParseFloat(data[3]);
            //float? ci = ParseFloat(data[4]);

            if (ra != null && dec != null && mag != null) //&& ci != null) 
            {
                float raf = (float)ra *15; //convert from hours to degrees
                float decf = (float)dec;
                eqPositions[i] = new Vector2(raf, decf);
                points[i].position = Coordinates.Sky2Unity(Coordinates.EquatorialSky2Sky(new Vector2(raf, decf), starfieldRadius, true), false);
                       
                points[i].startSize = 0.015f + ((8.0f - (float)mag) / 170.0f); // scale so size ranges from 0.015 to 0.07

                // Could convert colour index to get something closer to real colour, but there's
                // no nice way to do it. https://stackoverflow.com/questions/21977786/star-b-v-color-index-to-apparent-rgb-color
                // Leaving aside for now.
                points[i].startColor = Color.white * Mathf.Clamp01(0.2f * (8.0f - (float)mag));

                i++;
                if (i >= maxParticle)
                {
                    break;
                }
            }
        }

        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();

        realNPoints = Max(i, points.Length);
        particleSystem.SetParticles(points, realNPoints);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateStars();
    }

    /** Update the rotation of the starfield to match the new given time.
     *
     * Currently, the only effect accounted for is the daily rotation of the Earth - 
     * no precession, nutation, orbit aberration, proper motion of stars, etc.
     *
     * @param dateTime starfield should be rotated to match what would be visible at this
     * time.
     */
    public void UpdatePosition(System.DateTime dateTime)
    {
        double LocalSiderealTime = Coordinates.LocalSiderealTime(dateTime);
        /*double ERA = Coordinates.normalisedAngleDegrees(Coordinates.EarthRotationAngle(dateTime,true));
        Quaternion earthRotation = Quaternion.AngleAxis((float)ERA, Vector3.up);*/

        for (int i = 0; i < realNPoints; ++i) 
        {
            // Local Hour Angle = Local Sidereal Time - right Ascencion of star under consideration.
            // [to account for the daily rotation of the earth]
            Vector2 eqCoords = new Vector2((float)LocalSiderealTime - eqPositions[i].x, eqPositions[i].y);
            points[i].position = Coordinates.Sky2Unity(Coordinates.EquatorialSky2Sky(eqCoords, starfieldRadius, true), false);
        }

        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
        particleSystem.SetParticles(points, realNPoints);
    }

    /**
    * Does the same as above function, but finds updated position of a specific
    * coordinate based on the date time. Useful when wating to know where the
    * telescopes should point when observation over some period of time to still
    * keep inital point in view.
    */
    public Vector3 UpdatePosition(System.DateTime dateTime, Vector2 eqCoords)
    {
        // Adjust for earths rotation of (15/3600) degrees/second
        double ra = Coordinates.normalisedAngleDegrees(eqCoords.x - (15.0f/3600.0f));
        Vector2 eq = new Vector2((float) ra, eqCoords.y);
        Vector3 sky = Coordinates.EquatorialSky2Sky(eq, starfieldRadius, true);

        return sky;
    }
}
