using DevExpress.Mvvm;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Media.Imaging;

namespace AgendaViewDemo {
    public class CustomObjects : ViewModelBase {
        Random RandomInstance = new Random();
        BindingList<CustomResource> _resourcesList = new BindingList<CustomResource>();
        BindingList<CustomAppointment> _appointmentsList = new BindingList<CustomAppointment>();

        public BindingList<CustomResource> Resources {
            get {
                return _resourcesList;
            }
            set {
                if (_resourcesList == null)
                    return;
                _resourcesList = value;
                RaisePropertyChanged(() => Resources);
            }
        }
        public BindingList<CustomAppointment> Appointments {
            get {
                return _appointmentsList;
            }
            set {
                if (_appointmentsList == null)
                    return;
                _appointmentsList = value;
                RaisePropertyChanged(() => Appointments);
            }
        }

        public CustomObjects() {
            InitResources();
            InitAppointments();
        }

        void InitResources() {
            _resourcesList.Add(CreateCustomResource(1, "Max Fowler", System.Drawing.Color.PowderBlue, AgendaViewDemo.Properties.Resources.MaxFowlerPhoto));
            _resourcesList.Add(CreateCustomResource(2, "Nancy Drewmore", System.Drawing.Color.PaleVioletRed, AgendaViewDemo.Properties.Resources.NancyDrewmorePhoto));
            _resourcesList.Add(CreateCustomResource(3, "Pak Jang", System.Drawing.Color.PeachPuff, null));
        }
        CustomResource CreateCustomResource(int res_id, string caption, System.Drawing.Color ResColor, Image image) {
            CustomResource cr = new CustomResource();
            cr.ResID = res_id;
            cr.Name = caption;
            if (image != null) {
                cr.ResImage = image;
            }
            return cr;
        }
        void InitAppointments() {
            int count = _resourcesList.Count;
            for (int i = 0; i < count; i++) {
                CustomResource resource = _resourcesList[i];
                string subjPrefix = resource.Name + "'s ";
                _appointmentsList.Add(CreateEvent(subjPrefix + "meeting", "meeting", resource.ResID, 2, 5, 0, "office"));
                _appointmentsList.Add(CreateEvent(subjPrefix + "travel", "travel", resource.ResID, 3, 6, 0, "out of the city"));
                _appointmentsList.Add(CreateEvent(subjPrefix + "phone call", "phone call", resource.ResID, 0, 10, 0, "office"));
                _appointmentsList.Add(CreateEvent(subjPrefix + "business trip", "business trip", resource.ResID, 3, 6, 3, "San-Francisco"));
                _appointmentsList.Add(CreateEvent(subjPrefix + "double personal day", "double personal day", resource.ResID, 0, 10, 2, "out of the city"));
                _appointmentsList.Add(CreateEvent(subjPrefix + "personal day", "personal day", resource.ResID, 0, 10, 1, "out of the city"));
            }
        }
        CustomAppointment CreateEvent(string description, string subject, object resourceId, int status, int label, int days, string location) {
            CustomAppointment apt = new CustomAppointment();
            apt.Subject = subject;
            apt.Description = description;
            apt.OwnerId = resourceId;
            Random rnd = RandomInstance;
            int rangeInMinutes = 60 * 24;
            if (days == 2) {
                apt.StartTime = DateTime.Today;
                apt.EndTime = DateTime.Today.AddDays(2);
            }
            else if (days == 1) {
                apt.StartTime = DateTime.Today;
                apt.EndTime = DateTime.Today.AddDays(1);
            }
            else {
                apt.StartTime = DateTime.Today + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes));
                apt.EndTime = apt.StartTime.AddDays(days) + TimeSpan.FromMinutes(rnd.Next(0, rangeInMinutes / 4));
            }
            apt.Location = location;
            apt.Status = status;
            apt.Label = label;
            return apt;
        }
    }

    #region #customappointment
    public class CustomAppointment {
        private DateTime m_Start;
        private DateTime m_End;
        private string m_Subject;
        private int m_Status;
        private string m_Description;
        private int m_Label;
        private string m_Location;
        private bool m_Allday;
        private int m_EventType;
        private string m_RecurrenceInfo;
        private string m_ReminderInfo;
        private object m_OwnerId;


        public DateTime StartTime { get { return m_Start; } set { m_Start = value; } }
        public DateTime EndTime { get { return m_End; } set { m_End = value; } }
        public string Subject { get { return m_Subject; } set { m_Subject = value; } }
        public int Status { get { return m_Status; } set { m_Status = value; } }
        public string Description { get { return m_Description; } set { m_Description = value; } }
        public int Label { get { return m_Label; } set { m_Label = value; } }
        public string Location { get { return m_Location; } set { m_Location = value; } }
        public bool AllDay { get { return m_Allday; } set { m_Allday = value; } }
        public int EventType { get { return m_EventType; } set { m_EventType = value; } }
        public string RecurrenceInfo { get { return m_RecurrenceInfo; } set { m_RecurrenceInfo = value; } }
        public string ReminderInfo { get { return m_ReminderInfo; } set { m_ReminderInfo = value; } }
        public object OwnerId { get { return m_OwnerId; } set { m_OwnerId = value; } }

        public CustomAppointment() {
        }
    }
    #endregion  #customappointment

    #region #customresource
    public class CustomResource {
        private string m_name;
        private int m_res_id;
        private Image m_res_image;

        public string Name { get { return m_name; } set { m_name = value; } }
        public int ResID { get { return m_res_id; } set { m_res_id = value; } }
        public Image ResImage { get { return m_res_image; } set { m_res_image = value; } }

        public CustomResource() {
        }
    }
    #endregion #customresource
}