using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Telescope
{
    [System.Serializable]
    public class TelescopePointData
    {
        public List<PointData> pointData;
        public TelescopePointData(List<PointData> pointData)
        {
            this.pointData = pointData;
        }
    }
    
    [System.Serializable]
    public class PointData
    {
        public bool errorState;
        public string value;
        public string label;
        public string time;
        public string pointName;

        public PointData() : this(false, "", "", "", "")
        {

        }

        public PointData(bool errorState, string value, string label, string time, string pointName)
        {
            this.errorState = errorState;
            this.value = value;
            this.label = label;
            this.time = time;
            this.pointName = pointName;
        }
    }
}