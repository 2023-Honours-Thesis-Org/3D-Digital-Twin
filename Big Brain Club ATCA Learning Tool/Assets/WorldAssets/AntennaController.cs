using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Telescope
{
    public class AntennaController : MonoBehaviour
    {
        // A list of dictionaries. First key = the number in the antenna name eg. 1 for CA01. 
        // Second key = desired array configuration eg 6A. Result = station name eg W392
        public Dictionary<string, string>[] arrayConfigurations = new Dictionary<string, string>[]{null,
            new Dictionary<string, string>(), new Dictionary<string, string>(), new Dictionary<string, string>(), 
            new Dictionary<string, string>(), new Dictionary<string, string>(), new Dictionary<string, string>()
            };

        public string defaultConfiguration = "6A";

    public GameObject antennaPrefab;
    public GameObject[] antennaClones;

        // Start is called before the first frame update
        void Start()
        {
            populateArrayConfigurations(); 

            antennaClones = new GameObject[6];
            // Create the 6 telescope antennae
            for (int i = 1; i <= 6; ++i) {
                antennaClones[i-1] = Instantiate(antennaPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                antennaClones[i-1].name = "antennaCA0" + i;

                ATCAantenna antenna = ((GameObject)antennaClones[i-1]).GetComponent<ATCAantenna>();
                antenna.moveAntennaTo(arrayConfigurations[i][defaultConfiguration]);
                antenna.completeAnimations();
            }

        }

        /** Allows users to modify the positions of all antenna in the array to one of the present configurations.
        *
        * Preset configurations are obtained from Appendix G in the ATCA manual.
        * 
        * Note this sets the target array configuration and the antennas will then immediately start moving towards
        * their new positions. Call allAntennaCompleteAnimations() to complete the animations immediately.
        *
        * @param configuration the name eg "6A" of the target preset configuration.
        */
        public void setArrayConfigurationTo(string configuration) {
            // judicious checks just in case someone's been messing with arrayConfigurations. We don't want any key errors 
            // or confusing partial array configuration changes
            if (arrayConfigurations[1].ContainsKey(configuration) && arrayConfigurations[2].ContainsKey(configuration) && 
                arrayConfigurations[3].ContainsKey(configuration) && arrayConfigurations[4].ContainsKey(configuration) &&
                arrayConfigurations[5].ContainsKey(configuration) && arrayConfigurations[6].ContainsKey(configuration)) {

                for (int i = 1; i <= 6; ++i) {
                    ATCAantenna antenna = ((GameObject)antennaClones[i-1]).GetComponent<ATCAantenna>();
                    antenna.moveAntennaTo(arrayConfigurations[i][configuration]);
                }

            }

        }

        /** Set the positions of all antennas to their target positions, finishing all ongoing animations.
        *
        */
        public void allAntennaCompleteAnimations() {
            for (int i = 1; i <= 6; ++i) {
                ATCAantenna antenna = ((GameObject)antennaClones[i-1]).GetComponent<ATCAantenna>();
                antenna.completeAnimations();
            }
        }

        /** Return the position that antenna CA01 is looking at, in Unity Coordinates
        *
        * (antenna CA01 because the current-status page lists this one as the reference azimuth/elevation)
        *
        * Looks at a distance of 10000
        *
        */
        /*public Vector3 getArrayObservingCoordinates() {
            ((GameObject)antennaClones[0]).GetComponent<ATCAantenna>();
        }*/

        /** Allows users to modify the positions of the antennae on the tracks.
        *
        * @param antennaNumber a number from 1-5, representing one of the moveable telescope antennae
        * @param stationName the name eg "W4" "N14" of the station to which that telescope should move.
        */
        public void moveAntennaTo(int antennaNumber, string stationName) 
        {
            if (antennaNumber < 1 || antennaNumber > 5) 
            {
                Debug.LogWarning("Cannot move antenna number " + antennaNumber + "!");
            } 
            else 
            {
                ATCAantenna antenna = ((GameObject)antennaClones[antennaNumber - 1]).GetComponent<ATCAantenna>();
                antenna.moveAntennaTo(stationName);
            }
        }

        /** Fills in the 6 dictionaries of `arrayConfigurations` with 
        * array configurations from Appendix G of ATCA manual. 
        *
        * ATCA manual accessed 3 August 2022
        *
        * Once set up, `arrayConfigurations` allows you to look up the station that a 
        * particular antenna will be located at for a given preset array configuration.
        */
        private void populateArrayConfigurations() {
            // array 0 is left null so that array indexes match with antenna names eg CA01
            arrayConfigurations[1]["6A"] = "W4";
            arrayConfigurations[2]["6A"] = "W45";
            arrayConfigurations[3]["6A"] = "W102";
            arrayConfigurations[4]["6A"] = "W173";
            arrayConfigurations[5]["6A"] = "W196";
            arrayConfigurations[6]["6A"] = "W392";

            arrayConfigurations[1]["6B"] = "W2";
            arrayConfigurations[2]["6B"] = "W64";
            arrayConfigurations[3]["6B"] = "W147";
            arrayConfigurations[4]["6B"] = "W182";
            arrayConfigurations[5]["6B"] = "W196";
            arrayConfigurations[6]["6B"] = "W392";

            arrayConfigurations[1]["6C"] = "W0";
            arrayConfigurations[2]["6C"] = "W10";
            arrayConfigurations[3]["6C"] = "W113";
            arrayConfigurations[4]["6C"] = "W140";
            arrayConfigurations[5]["6C"] = "W182";
            arrayConfigurations[6]["6C"] = "W392";

            arrayConfigurations[1]["6D"] = "W8";
            arrayConfigurations[2]["6D"] = "W32";
            arrayConfigurations[3]["6D"] = "W84";
            arrayConfigurations[4]["6D"] = "W168";
            arrayConfigurations[5]["6D"] = "W173";
            arrayConfigurations[6]["6D"] = "W392";

            arrayConfigurations[1]["1.5A"] = "W100";
            arrayConfigurations[2]["1.5A"] = "W110";
            arrayConfigurations[3]["1.5A"] = "W147";
            arrayConfigurations[4]["1.5A"] = "W168";
            arrayConfigurations[5]["1.5A"] = "W173";
            arrayConfigurations[6]["1.5A"] = "W392";

            arrayConfigurations[1]["1.5B"] = "W111";
            arrayConfigurations[2]["1.5B"] = "W113";
            arrayConfigurations[3]["1.5B"] = "W163";
            arrayConfigurations[4]["1.5B"] = "W182";
            arrayConfigurations[5]["1.5B"] = "W195";
            arrayConfigurations[6]["1.5B"] = "W392";

            arrayConfigurations[1]["1.5C"] = "W98";
            arrayConfigurations[2]["1.5C"] = "W128";
            arrayConfigurations[3]["1.5C"] = "W173";
            arrayConfigurations[4]["1.5C"] = "W190";
            arrayConfigurations[5]["1.5C"] = "W195";
            arrayConfigurations[6]["1.5C"] = "W392";

            arrayConfigurations[1]["1.5D"] = "W102";
            arrayConfigurations[2]["1.5D"] = "W109";
            arrayConfigurations[3]["1.5D"] = "W140";
            arrayConfigurations[4]["1.5D"] = "W182";
            arrayConfigurations[5]["1.5D"] = "W196";
            arrayConfigurations[6]["1.5D"] = "W392";

            arrayConfigurations[1]["750A"] = "W147";
            arrayConfigurations[2]["750A"] = "W163";
            arrayConfigurations[3]["750A"] = "W172";
            arrayConfigurations[4]["750A"] = "W190";
            arrayConfigurations[5]["750A"] = "W195";
            arrayConfigurations[6]["750A"] = "W392";

            arrayConfigurations[1]["750B"] = "W98";
            arrayConfigurations[2]["750B"] = "W109";
            arrayConfigurations[3]["750B"] = "W113";
            arrayConfigurations[4]["750B"] = "W140";
            arrayConfigurations[5]["750B"] = "W148";
            arrayConfigurations[6]["750B"] = "W392";

            arrayConfigurations[1]["750C"] = "W64";
            arrayConfigurations[2]["750C"] = "W84";
            arrayConfigurations[3]["750C"] = "W100";
            arrayConfigurations[4]["750C"] = "W110";
            arrayConfigurations[5]["750C"] = "W113";
            arrayConfigurations[6]["750C"] = "W392";

            arrayConfigurations[1]["750D"] = "W100";
            arrayConfigurations[2]["750D"] = "W102";
            arrayConfigurations[3]["750D"] = "W128";
            arrayConfigurations[4]["750D"] = "W140";
            arrayConfigurations[5]["750D"] = "W147";
            arrayConfigurations[6]["750D"] = "W392";

            arrayConfigurations[1]["EW367"] = "W104";
            arrayConfigurations[2]["EW367"] = "W110";
            arrayConfigurations[3]["EW367"] = "W113";
            arrayConfigurations[4]["EW367"] = "W124";
            arrayConfigurations[5]["EW367"] = "W128";
            arrayConfigurations[6]["EW367"] = "W392";
            arrayConfigurations[1]["EW367B"] = "W124";
            arrayConfigurations[2]["EW367B"] = "W104";
            arrayConfigurations[3]["EW367B"] = "W110";
            arrayConfigurations[4]["EW367B"] = "W113";
            arrayConfigurations[5]["EW367B"] = "W128";
            arrayConfigurations[6]["EW367B"] = "W392";

            arrayConfigurations[1]["EW352"] = "W102";
            arrayConfigurations[2]["EW352"] = "W104";
            arrayConfigurations[3]["EW352"] = "W109";
            arrayConfigurations[4]["EW352"] = "W112";
            arrayConfigurations[5]["EW352"] = "W125";
            arrayConfigurations[6]["EW352"] = "W392";
            arrayConfigurations[1]["EW352B"] = "W112";
            arrayConfigurations[2]["EW352B"] = "W102";
            arrayConfigurations[3]["EW352B"] = "W104";
            arrayConfigurations[4]["EW352B"] = "W109";
            arrayConfigurations[5]["EW352B"] = "W125";
            arrayConfigurations[6]["EW352B"] = "W392";

            arrayConfigurations[1]["EW214"] = "W98";
            arrayConfigurations[2]["EW214"] = "W102";
            arrayConfigurations[3]["EW214"] = "W104";
            arrayConfigurations[4]["EW214"] = "W109";
            arrayConfigurations[5]["EW214"] = "W112";
            arrayConfigurations[6]["EW214"] = "W392";
            arrayConfigurations[1]["EW214B"] = "W109";
            arrayConfigurations[2]["EW214B"] = "W98";
            arrayConfigurations[3]["EW214B"] = "W102";
            arrayConfigurations[4]["EW214B"] = "W104";
            arrayConfigurations[5]["EW214B"] = "W112";
            arrayConfigurations[6]["EW214B"] = "W392";

            arrayConfigurations[1]["NS214"] = "W106";
            arrayConfigurations[2]["NS214"] = "N2";
            arrayConfigurations[3]["NS214"] = "N7";
            arrayConfigurations[4]["NS214"] = "N11";
            arrayConfigurations[5]["NS214"] = "N14";
            arrayConfigurations[6]["NS214"] = "W392";

            arrayConfigurations[1]["H214"] = "W98";
            arrayConfigurations[2]["H214"] = "W104";
            arrayConfigurations[3]["H214"] = "W113";
            arrayConfigurations[4]["H214"] = "N5";
            arrayConfigurations[5]["H214"] = "N14";
            arrayConfigurations[6]["H214"] = "W392";
            arrayConfigurations[1]["H214B"] = "W98";
            arrayConfigurations[2]["H214B"] = "W104";
            arrayConfigurations[3]["H214B"] = "N14";
            arrayConfigurations[4]["H214B"] = "N5";
            arrayConfigurations[5]["H214B"] = "W113";
            arrayConfigurations[6]["H214B"] = "W392";

            arrayConfigurations[1]["H214C"] = "W98";
            arrayConfigurations[2]["H214C"] = "N5";
            arrayConfigurations[3]["H214C"] = "N14";
            arrayConfigurations[4]["H214C"] = "W104";
            arrayConfigurations[5]["H214C"] = "W113";
            arrayConfigurations[6]["H214C"] = "W392";
            arrayConfigurations[1]["H214D"] = "W98";
            arrayConfigurations[2]["H214D"] = "N14";
            arrayConfigurations[3]["H214D"] = "N5";
            arrayConfigurations[4]["H214D"] = "W104";
            arrayConfigurations[5]["H214D"] = "W113";
            arrayConfigurations[6]["H214D"] = "W392";

            arrayConfigurations[1]["H168"] = "W100";
            arrayConfigurations[2]["H168"] = "W104";
            arrayConfigurations[3]["H168"] = "W111";
            arrayConfigurations[4]["H168"] = "N7";
            arrayConfigurations[5]["H168"] = "N11";
            arrayConfigurations[6]["H168"] = "W392";
            arrayConfigurations[1]["H168B"] = "W100";
            arrayConfigurations[2]["H168B"] = "W104";
            arrayConfigurations[3]["H168B"] = "N11";
            arrayConfigurations[4]["H168B"] = "N7";
            arrayConfigurations[5]["H168B"] = "W111";
            arrayConfigurations[6]["H168B"] = "W392";

            arrayConfigurations[1]["H168C"] = "W100";
            arrayConfigurations[2]["H168C"] = "N7";
            arrayConfigurations[3]["H168C"] = "N11";
            arrayConfigurations[4]["H168C"] = "W104";
            arrayConfigurations[5]["H168C"] = "W111";
            arrayConfigurations[6]["H168C"] = "W392";
            arrayConfigurations[1]["H168D"] = "W100";
            arrayConfigurations[2]["H168D"] = "N11";
            arrayConfigurations[3]["H168D"] = "N7";
            arrayConfigurations[4]["H168D"] = "W104";
            arrayConfigurations[5]["H168D"] = "W111";
            arrayConfigurations[6]["H168D"] = "W392";

            arrayConfigurations[1]["H75"] = "W104";
            arrayConfigurations[2]["H75"] = "W106";
            arrayConfigurations[3]["H75"] = "W109";
            arrayConfigurations[4]["H75"] = "N2";
            arrayConfigurations[5]["H75"] = "N5";
            arrayConfigurations[6]["H75"] = "W392";
            arrayConfigurations[1]["H75B"] = "W104";
            arrayConfigurations[2]["H75B"] = "N5";
            arrayConfigurations[3]["H75B"] = "N2";
            arrayConfigurations[4]["H75B"] = "W106";
            arrayConfigurations[5]["H75B"] = "W109";
            arrayConfigurations[6]["H75B"] = "W392";
            arrayConfigurations[1]["H75C"] = "W104";
            arrayConfigurations[2]["H75C"] = "N2";
            arrayConfigurations[3]["H75C"] = "N5";
            arrayConfigurations[4]["H75C"] = "W106";
            arrayConfigurations[5]["H75C"] = "W109";
            arrayConfigurations[6]["H75C"] = "W392";

            arrayConfigurations[1]["122C"] = "W98";
            arrayConfigurations[2]["122C"] = "W100";
            arrayConfigurations[3]["122C"] = "W102";
            arrayConfigurations[4]["122C"] = "W104";
            arrayConfigurations[5]["122C"] = "W106";
            arrayConfigurations[6]["122C"] = "W392";

            // OBSOLETE ARRAY CONFIGURATIONS included only for completion
            arrayConfigurations[1]["375"] = "W2";
            arrayConfigurations[2]["375"] = "W10";
            arrayConfigurations[3]["375"] = "W14";
            arrayConfigurations[4]["375"] = "W16";
            arrayConfigurations[5]["375"] = "W32";
            arrayConfigurations[6]["375"] = "W392";
            arrayConfigurations[1]["210"] = "W98";
            arrayConfigurations[2]["210"] = "W100";
            arrayConfigurations[3]["210"] = "W102";
            arrayConfigurations[4]["210"] = "W109";
            arrayConfigurations[5]["210"] = "W112";
            arrayConfigurations[6]["210"] = "W392";
            arrayConfigurations[1]["122A"] = "W0";
            arrayConfigurations[2]["122A"] = "W2";
            arrayConfigurations[3]["122A"] = "W4";
            arrayConfigurations[4]["122A"] = "W6";
            arrayConfigurations[5]["122A"] = "W8";
            arrayConfigurations[6]["122A"] = "W392";
            arrayConfigurations[1]["122B"] = "W8";
            arrayConfigurations[2]["122B"] = "W10";
            arrayConfigurations[3]["122B"] = "W12";
            arrayConfigurations[4]["122B"] = "W14";
            arrayConfigurations[5]["122B"] = "W16";
            arrayConfigurations[6]["122B"] = "W392";

        }

    }
}