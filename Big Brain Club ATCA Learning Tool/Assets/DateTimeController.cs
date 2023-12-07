using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace Controller
{
    public class DateTimeController : MonoBehaviour
    {
        private DateTime dateTime;
        private TimeZoneInfo nswTimeZone;
        private bool realTimeEnabled = true;

        public int updateIntervalSecs = 1;

        void Start()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                nswTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            else
                nswTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Australia/NSW");
            dateTime = DateTime.Now;
            EventManager.UserTimeEvent += UserTimeSetup;
            EventManager.RealTimeEvent += RealTimeSetup;
        }

        void Update()
        {
            DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, nswTimeZone);
            if (realTimeEnabled && currentTime >= dateTime.AddSeconds(updateIntervalSecs))
                SetDateTime(DateTime.Now);
        }

        public void SetDateTime(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Unspecified)
                dateTime = TimeZoneInfo.ConvertTime(dateTime, nswTimeZone);
            this.dateTime = dateTime;
            // UpdateUI();
        }

        public void SetDateFromString(string dateString)
        {
            try
            {
                DateTime newDateTime = DateTime.Parse(dateString);
                newDateTime = newDateTime.Date + dateTime.TimeOfDay;
                SetDateTime(newDateTime);
            }
            catch
            {
                SetDateTime(dateTime);
            }
        }

        public void SetTimeFromString(string timeString)
        {
            try
            {
                DateTime newDateTime = DateTime.Parse(timeString);
                newDateTime = dateTime.Date + newDateTime.TimeOfDay;
                SetDateTime(newDateTime);
            }
            catch
            {
                SetDateTime(dateTime);
            }
        }

        public DateTime GetDateTime()
        {
            return dateTime;
        }

        private void UserTimeSetup()
        {
            realTimeEnabled = false;
            SetDateTime(dateTime);
        }

        private void RealTimeSetup()
        {
            realTimeEnabled = true;
            SetDateTime(DateTime.Now);
        }
    }
}
