using System.Collections;
using System.Collections.Generic;
using static System.Math;
using static System.DateTime;
using UnityEngine;

public static class Coordinates
{

    public static float W196lat = -30.31277778f;
    public static float W196long = -149.5502778f;

    /** Converts from Unity world coordinates to the sky coordinates used by ATCA
     * (conventional geocentric right-handed cartesian coordinates with z axis aligned with
     * North pole and x axis aligned with the intersection of Greenwich meridian and equator)
     *
     * @param unityCoords Unity world coordinates to convert from
     */
    public static Vector3 Unity2Sky(Vector3 unityCoords)
    {
        // Distance of W196 from Earth's centre is 6372723m (sea level) + 210m (elevation) = 6372933m
        // Coords of W196 is -30.31277778 latitude, 149.5503778 longitude
        // First, translate the unity coordinates 6372933m up along the y axis
        Vector3 translated = new Vector3(unityCoords.x, unityCoords.y + 6372933, unityCoords.z);
        // Then rotate these new coordinates -90 deg along z axis to align W196 on the x axis,
        // rotate -90 about the x axis to align the stations along the equator,
        // rotate -30.31... along the z axis to get the correct latitude
        // and rotate -149.55... along the y axis to get the correct longitude.
        // Quaternion.Euler rotates along z axis, along x axis, then along y axis.
        Quaternion rotation = Quaternion.Euler(0.0f, W196long, W196lat) * Quaternion.Euler(-90.0f, 0.0f, -90.0f);
        Vector3 rotated = rotation * translated;
        // Unity's coordinates have y and z axes swapped compared to Earth's cartesian coordinates.
        return new Vector3(rotated.x, rotated.z, rotated.y);
    }

    /** Converts from sky coordinates back to Unity world coordinates
     *
     * @param skyCoords sky coordinates to convert from
     */
    public static Vector3 Sky2Unity(Vector3 skyCoords)
    {
        // Same procedure as Unity2Sky() but done backwards.
        Vector3 swappedCoords = new Vector3(skyCoords.x, skyCoords.z, skyCoords.y);
        Quaternion inverseRotation = Quaternion.Inverse(Quaternion.Euler(-90.0f, 0.0f, -90.0f)) *
            Quaternion.Inverse(Quaternion.Euler(0.0f, W196long, W196lat));
        Vector3 rotated = inverseRotation * swappedCoords;
        return new Vector3(rotated.x, rotated.y - 6372933, rotated.z);
    }

    /** Converts from sky coordinates back to Unity world coordinates
     *
     * @param skyCoords sky coordinates to convert from
     * @param yTranslation If true, assumes input coordinates were centered at the 
     * center of the earth and need to be moved upwards. Else, doesn't bother to move.
     * Case FALSE is for something like stars, which are arbitrary distance away.
     */
    public static Vector3 Sky2Unity(Vector3 skyCoords, bool yTranslation)
    {
        // Same procedure as Unity2Sky() but done backwards.
        Vector3 swappedCoords = new Vector3(skyCoords.x, skyCoords.z, skyCoords.y);
        Quaternion inverseRotation = Quaternion.Inverse(Quaternion.Euler(-90.0f, 0.0f, -90.0f)) *
            Quaternion.Inverse(Quaternion.Euler(0.0f, W196long, W196lat));
        Vector3 rotated = inverseRotation * swappedCoords;
        if (yTranslation) 
        {
            rotated.y = rotated.y - 6372933; 
        }
        return rotated;        
    }

    /** Gives the sky coordinates of the real world Sun at the given date and time.
    * implementation is based on the algorithm given by
    * https://en.wikipedia.org/wiki/Position_of_the_Sun
    * Note that all angles are measured in radians, not degrees.
    *
    * @param dateTime Initialised DateTime object
    */
    public static Vector3 SunCoordinates(System.DateTime dateTime)
    {
        System.DateTime dayZero = new System.DateTime(2000, 01, 01, 12, 00, 00, System.DateTimeKind.Utc);
        System.TimeSpan timeDifference = dateTime.ToUniversalTime() - dayZero;
        double daysPassed = timeDifference.TotalDays;
        double meanLongitude = normalisedAngle(4.89495042 + 0.01720279*daysPassed);
        double meanAnomaly = normalisedAngle(6.240040768 + 0.01720197034*daysPassed);
        double eclipticLongitude = normalisedAngle(
                meanLongitude + 0.03342305518*Sin(meanAnomaly) + 0.0003490658504*Sin(2*meanAnomaly));
        double sunEarthDistance = (1.0014 - 0.01671*Cos(meanAnomaly) - 0.00014*Cos(2*meanAnomaly)) * 149597870700;
        double eclipticObiquity = 0.4089131904 - (6.981317e-9)*daysPassed;
        // Algorithm diverges from Wikipedia version here as the Wikipedia version
        // gives Earth-centred inertial coordinates while we want Earth-centred Earth-fixed coordinates
        double zCoord = sunEarthDistance * Sin(eclipticObiquity) * Sin(eclipticLongitude);
        double latitudinalDistance = Sqrt(Pow(sunEarthDistance, 2.0) - Pow(zCoord, 2.0));
        double angleFromMeridian = -(timeDifference.TotalDays - timeDifference.Days) * 2*PI;
        double xCoord = latitudinalDistance * Cos(angleFromMeridian);
        double yCoord = latitudinalDistance * Sin(angleFromMeridian);
        return new Vector3((float) xCoord, (float) yCoord, (float) zCoord);
    }

    /** Convert angle to range [0,2Pi) radians.*/
    public static double normalisedAngle(double angle)
    {
        double angleRemainder = (angle % (2*PI));
        if (angleRemainder < 0)
            return angleRemainder + (2*PI);
        return angleRemainder;
    }

    /** Convert angle to range [0,360) degrees. */
    public static double normalisedAngleDegrees(double angle)
    {
        double angleRemainder = (angle % 360);
        if (angleRemainder < 0)
            return angleRemainder + 360;
        return angleRemainder;
    }

    /** Convert sky coordinates as used by ATCA
     * (conventional geocentric right-handed cartesian coordinates with z axis aligned with
     * North pole and x axis aligned with the intersection of Greenwich meridian and equator)
     * to equatorial sky coordinates with a J2000 equinox.
     *
     * Converts Cartesian coordinates (x,y,z) to equatorial coordinates 
     * (right ascencion alpha, declination delta)
     *
     * Source: https://en.wikipedia.org/wiki/Equatorial_coordinate_system#Geocentric_equatorial_coordinates
     *
     * @param skyCoords conventional geocentric right-handed cartesian coordinates (x,y,z)
     * @param inDegrees true for equatorial coordinates to be in degrees, false for them to be in radians.
     */
    public static Vector2 Sky2EquatorialSky(Vector3 skyCoords, bool inDegrees) 
    {
        skyCoords.Normalize();
        float delta = (float) Asin(skyCoords.z);  
        float alpha = (float) Acos(skyCoords.x / Cos(delta));

        if (inDegrees) 
        {
            return new Vector2(alpha * 180/(float)PI, delta * 180/(float)PI);
        } // else 
        return new Vector2(alpha, delta);
    } 

    /** Converts equatorial coordinates (right ascencion alpha, declination delta) to 
     * right-handed geocentric Cartesian coordinates (x,y,z) both using J2000 epoch.
     *
     * The cartesian coordinates will be on a sphere of radius 1.
     *
     * Source: https://www.jameswatkins.me/posts/converting-equatorial-to-cartesian.html
     *
     * @param equatorialCoords equatorial coordinates (ra,dec)
     * @param inDegrees true if equatorial coordinates are provided in degrees, false 
     * if they are in radians.
     */
    public static Vector3 EquatorialSky2Sky(Vector2 equatorialCoords, bool inDegrees) 
    {
        double alpha = equatorialCoords.x;
        double delta = equatorialCoords.y;  
        if (inDegrees) 
        {
            alpha *= PI/180;
            delta *= PI/180;
        }

        float xCoord = (float) (Cos(alpha) * Cos(delta));
        float yCoord = (float) (Sin(alpha) * Cos(delta)); 
        float zCoord = (float) Sin(delta);

        return new Vector3(xCoord, yCoord, zCoord);
    } 

    /** Converts equatorial coordinates (right ascencion alpha, declination delta) to 
     * right-handed geocentric Cartesian coordinates (x,y,z) both using J2000 epoch.
     *
     * The cartesian coordinates will be on a sphere of radius `distance`.
     *
     * Source: https://www.jameswatkins.me/posts/converting-equatorial-to-cartesian.html
     *
     * @param equatorialCoords equatorial coordinates (ra,dec)
     * @param distance distance of the cartesian coordinates from the origin
     * @param inDegrees true if equatorial coordinates are provided in degrees, false 
     * if they are in radians.
     */
    public static Vector3 EquatorialSky2Sky(Vector2 equatorialCoords, double distance, bool inDegrees) 
    {
        double alpha = equatorialCoords.x;
        double delta = equatorialCoords.y;  
        if (inDegrees) 
        {
            alpha *= PI/180;
            delta *= PI/180;
        }

        float xCoord = (float) (distance * Cos(alpha) * Cos(delta));
        float yCoord = (float) (distance * Sin(alpha) * Cos(delta)); 
        float zCoord = (float) (distance * Sin(delta));

        return new Vector3(xCoord, yCoord, zCoord);
    } 

    /** Function to convert a (CE) DateTime date to a (CE) Julian date
     *
     * Julian date is the number of days after noon on Jan 1, 4713 BC.
     * 
     * Source: https://stackoverflow.com/questions/5248827/convert-datetime-to-julian-date-in-c-sharp-tooadate-safe,
     */
    public static double ToJulianDate(System.DateTime date)
    {
        return date.ToOADate() + 2415018.5; 
    }

    /** Function to convert a (CE) DateTime date to a (CE) J2000 Julian date.
     *
     * J2000 date is the number of days after noon on Jan 1, 2000 CE
     *
     * Source: https://en.wikipedia.org/wiki/J2000
     */
    public static double ToJ2000(System.DateTime date)
    {
        // return ToJulianDate(date) - 2451545.0;
        return date.ToOADate() - 36526.5;
    }

    /** Calculate Earth Rotation Angle (ERA) of a given date and time.
     *
     * ERA is an alternative to Hour Angles to calculate which stars are visible at which 
     * time of day. It should be easier to calculate than Hour Angles because its origin does
     * not move.
     *
     * The formula used is an approximation of ERA for the modern day. Don't use for dates too
     * far away from 2017 (date of latest Wikipedia source that uses the formula).
     * 
     * Source: https://en.wikipedia.org/wiki/Sidereal_time#Earth_rotation_angle
     * and https://astronomy.stackexchange.com/questions/29986/official-degrees-of-earth-s-rotation-per-day
     * for further explanation.
     *
     * @param date the date to calculate the ERA of
     * @param inDegrees if true, the ERA is returned in degrees, if false, in radians.
     */
    public static double EarthRotationAngle(System.DateTime date, bool inDegrees)
    {
        // Linear polynomial of (Julian date - 2451545.0) (which is the J2000 date)
        if (inDegrees)
        {
            return 360*(0.7790572732640d + 1.002737811911d * ToJ2000(date)); // degrees
        }
        else 
        {
            return 2*PI*(0.7790572732640d + 1.002737811911d * ToJ2000(date)); // radians
        }
        
    }

    /** Calculate Local Sidereal Time (LST) at W196 at a given date and time.
     *
     * LST is the time of day at a local lat/long coordinate for a given date and 
     * time. It can be converted to Local Hour Angle, i.e. the apparent right ascencion
     * of a star in the sky at a given time, by simply subtracting the right ascencion 
     * of the star. 
     *
     * First, we use an Astronomical Almanac formula to calculate the Greenwich Mean Sidereal Time,
     * then add the longitude of the local position (where western longitudes are negative, eastern
     * longitudes are positive).
     *
     * Source: https://astronomy.stackexchange.com/questions/24859/local-sidereal-time?rq=1
     * and Practical Astronomy With Your Calculator (1988) p34 
     * (Retrieved from https://archive.org/details/practicalastrono0000duff/page/34/mode/2up)
     */
    public static double LocalSiderealTime(System.DateTime date)
    {
        double decimalHours = sexagesimalToDecimalDegrees(date.TimeOfDay.Hours,
                                                          date.TimeOfDay.Minutes,
                                                          date.TimeOfDay.Seconds);
        double GMST = 100.46 + 0.985647 * ToJ2000(date) + 15 * decimalHours;

        return normalisedAngleDegrees(GMST + W196long); // longitude hardcoded
    }

    /** Convert degrees/minutes/seconds angle to decimal degrees angle. */
    public static double sexagesimalToDecimalDegrees(double degrees, double minutes, double seconds) 
    {
        return degrees + (minutes / 60) + (seconds / 3600);
    }
}
