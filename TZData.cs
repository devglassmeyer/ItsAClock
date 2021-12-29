using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace ItsAClock
{
    [Serializable]
    public class tzCustomName
    {
        private string _custom_name = string.Empty;
        private bool _is_custom_name_on = true;

        public virtual string CustomName { get { return _custom_name; } set { _custom_name = value; } }
        public virtual bool HasCustomName()
        {
            return !string.IsNullOrWhiteSpace(_custom_name);
        }
        /// <summary>
        /// Boolean flag to allow the user to turn on/off the custom name display
        /// </summary>
        public virtual bool UseCustomName { get { return _is_custom_name_on; } set { _is_custom_name_on = value; } }

        /// <summary>
        /// Returns true if their is a custom name and it is turned on
        /// </summary>
        /// <returns></returns>
        public virtual bool IsCustomNameOn()
        {
            return HasCustomName() && _is_custom_name_on;
        }

        public tzCustomName() { }
        public tzCustomName(tzCustomName copyMe)
        {
            this.CustomName = copyMe.CustomName;
            this.UseCustomName = copyMe.UseCustomName;
        }
    }

    [Serializable]
    public class TZUserData : tzCustomName
    {
        private string _timezoneID = string.Empty;
        private bool _is_visible = false;
        private bool _is_included = false;

        public virtual string TimeZoneID { get { return _timezoneID; } set { _timezoneID = value; } }

        /// <summary>
        /// True if we show the time for this time zone. False we do not show the time for this time zone
        /// </summary>
        public virtual bool ShowTimeForTimeZone { get { return _is_visible; } set { _is_visible = value; } }
        /// <summary>
        /// Include this time zone in list of time zones of interest
        /// </summary>
        public virtual bool IncludeInList { get { return _is_included; } set { _is_included = value; } }


        /// <summary>
        /// Set all the values in this instance to be the same as another instance
        /// </summary>
        /// <param name="tu"></param>
        public void SetMyValues(TZUserData tu)
        {
            if (tu != null)
            {
                this.IncludeInList = tu.IncludeInList;
                this.ShowTimeForTimeZone = tu.ShowTimeForTimeZone;
                this.CustomName = tu.CustomName;
                this.UseCustomName = tu.UseCustomName;
            }
        }

        public TZUserData() { }
        public TZUserData(string time_zone_id)
        {
            _timezoneID = time_zone_id;
        }
        public TZUserData(TZUserData copyMe) : base(copyMe)
        {
            _timezoneID = copyMe.TimeZoneID;
            _is_visible = copyMe.ShowTimeForTimeZone;
            _is_included = copyMe.IncludeInList;
        }
    }
    [Serializable]
    public class TZData : TZUserData
    {
        private string _timezone_display_name = string.Empty;
        private bool _is_localtz = false;
        public string DisplayName { get { return _timezone_display_name; } set { _timezone_display_name = value; } }

        public override string CustomName { get => base.CustomName; set => base.CustomName = value; }

        public override bool ShowTimeForTimeZone
        {
            get
            {
                if (_is_localtz) { return true; }
                return base.ShowTimeForTimeZone;
            }
            set
            {
                if (!_is_localtz) { base.ShowTimeForTimeZone = value; }
            }
        }

        public override bool IncludeInList
        {
            get
            {
                if (_is_localtz) { return true; }
                return base.IncludeInList;
            }
            set
            {
                if (!_is_localtz) { base.IncludeInList = value; }
            }
        }

        public void SetLocal(string local_time_zone_id)
        {
            _is_localtz = (local_time_zone_id == base.TimeZoneID);
        }

        public bool IsLocalTimezone()
        {
            return _is_localtz;
        }

        public TZData(TZUserData from_Me) : base(from_Me) { }

        public TZData() { }
        public TZData(string time_zone_id, string time_zone_display_name) : base(time_zone_id)
        {
            _timezone_display_name = time_zone_display_name;
        }
    }

    [Serializable]
    [XmlInclude(typeof(tzCustomName))]
    [XmlInclude(typeof(TZUserData))]
    [XmlInclude(typeof(TZData))]
    public class TZDataList: tzCustomName
    {
        List<TZData> _tzlist = new List<TZData>();
        private bool _show_seconds = true;

        public bool IsSecondsOn { get { return _show_seconds; } set { _show_seconds = value; } }

        /// <summary>
        /// Look! A nice friendly List.
        /// How cool is that? Answer, very cool.
        /// </summary>
        [XmlIgnore]
        public List<TZData> TimeZoneData
        {
            get { return _tzlist; }
            set { _tzlist = value; }
        }

        /// <summary>
        /// Used for serialization
        /// </summary>
        [XmlElement("TZ")]
        public TZUserData[] UserData
        {
            get
            {
                List<TZUserData> ret = new List<TZUserData>();
                foreach (var tz in _tzlist)
                {
                    ret.Add(new TZUserData(tz));
                }
                return ret.ToArray();
            }
            set
            {
                _tzlist = new List<TZData>();
                if (value != null)
                {
                    foreach (var tz in value)
                    {
                        _tzlist.Add(new TZData(tz));
                    }
                }
            }
        }

        public void SetLocalTimezone(string time_zone_id)
        {
            // var tz = _tzlist.FirstOrDefault(x => x.TimeZoneID == time_zone_id);
            Parallel.ForEach(_tzlist, a_tz =>
            {
                a_tz.SetLocal(time_zone_id);
            });
        }
        public void Add_TimeZone(string time_zone_id, string time_zone_displayname, bool is_local)
        {
            if (_tzlist == null) { _tzlist = new List<TZData>(); }

            var tz = _tzlist.FirstOrDefault(x => x.TimeZoneID == time_zone_id);
            if (tz == null)
            {
                _tzlist.Add(new TZData(time_zone_id, time_zone_displayname));
            }
            else
            {
                tz.DisplayName = time_zone_displayname;
            }
            if (is_local)
            {
                SetLocalTimezone(time_zone_id);
            }
        }

        public void SetCurrentData(TZDataList currentTZ)
        {
            if (currentTZ != null)
            {
                foreach (var tz in currentTZ.TimeZoneData)
                {
                    var workTZ = _tzlist.FirstOrDefault(x => x.TimeZoneID == tz.TimeZoneID);

                    if (workTZ != null)
                    {
                        workTZ.SetMyValues(tz);
                    }
                }
            }
        }

        public TZDataList GetIncludedData()
        {
            TZDataList retVal = new TZDataList();

            retVal.TimeZoneData.AddRange(this.TimeZoneData.FindAll(x => x.IncludeInList && x.IsLocalTimezone() == false));
            retVal.IsSecondsOn = this.IsSecondsOn;

            var local_data = this.TimeZoneData.FirstOrDefault(x => x.IsLocalTimezone() == true);
            if (local_data != null)
            {
                retVal.CustomName = local_data.CustomName;
                retVal.UseCustomName = local_data.UseCustomName;
            }
            return retVal;
        }

        public TZDataList() { }
        public TZDataList(TZDataList a_list_of_tz)
        {
            _tzlist = new List<TZData>(a_list_of_tz.TimeZoneData);
        }
    }
}
