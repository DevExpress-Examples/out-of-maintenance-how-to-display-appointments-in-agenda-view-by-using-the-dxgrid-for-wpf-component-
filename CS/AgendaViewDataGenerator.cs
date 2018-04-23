using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraScheduler;
using DevExpress.Xpf.Scheduler;
using System.ComponentModel;
using System.Windows.Media;

namespace AgendaViewDemo {
    public static class AgendaViewDataGenerator {
        public static TimeInterval SelectedInterval { get; set; }

        public static object GenerateResourcesCollection(DevExpress.Xpf.Scheduler.SchedulerStorage storage) {
            return storage.ResourceStorage.Items;
        }
        public static object GenerateAgendaAppointmentCollection(DevExpress.Xpf.Scheduler.SchedulerStorage storage) {
            AppointmentBaseCollection sourceAppointments = storage.GetAppointments(SelectedInterval);
            BindingList<AgendaAppointment> agendaAppointments = new BindingList<AgendaAppointment>();
            foreach (Appointment appointment in sourceAppointments) {
                TimeInterval currentDayInterval = new TimeInterval(appointment.Start.Date, appointment.Start.Date.AddDays(1));
                string startTime = "";
                string endTime = "";
                if (currentDayInterval.Contains(appointment.End)) {
                    startTime = currentDayInterval.Start == appointment.Start ? "" : appointment.Start.TimeOfDay.ToString(@"hh\:mm");
                    endTime = currentDayInterval.End == appointment.End ? "" : appointment.End.TimeOfDay.ToString(@"hh\:mm");
                    agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, startTime, endTime));
                }
                else {
                    startTime = currentDayInterval.Start == appointment.Start ? "" : appointment.Start.TimeOfDay.ToString(@"hh\:mm");
                    agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, startTime, ""));
                    while (true) {
                        currentDayInterval = new TimeInterval(currentDayInterval.End, currentDayInterval.End.AddDays(1));
                        if (currentDayInterval.Contains(appointment.End)) {
                            endTime = currentDayInterval.End == appointment.End ? "" : appointment.End.TimeOfDay.ToString(@"hh\:mm");
                            agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, "", endTime));
                            break;
                        }
                        else {
                            agendaAppointments.Add(CreateAgendaAppointment(storage, appointment, currentDayInterval.Start, "", ""));
                        }
                    }
                }
            }
            return agendaAppointments;
        }
        static AgendaAppointment CreateAgendaAppointment(DevExpress.Xpf.Scheduler.SchedulerStorage storage, Appointment sourceAppointment, DateTime startDate, string startTime, string endTime) {
            AgendaAppointment agendaAppointment = new AgendaAppointment();
            agendaAppointment.AgendaDate = startDate;
            agendaAppointment.AgendaDescription = sourceAppointment.Description;
            agendaAppointment.AgendaSubject = sourceAppointment.Subject;
            if (startTime == "" && endTime == "") {
                agendaAppointment.AgendaDuration = "All Day";
            }
            else if (startTime == "" && endTime != "") {
                agendaAppointment.AgendaDuration = "Till: " + endTime;
            }
            else if (startTime != "" && endTime == "") {
                agendaAppointment.AgendaDuration = "From: " + startTime;
            }
            else {
                agendaAppointment.AgendaDuration = String.Format("{0} - {1}", startTime, endTime);
            }
            agendaAppointment.AgendaLocation = sourceAppointment.Location;
            agendaAppointment.AgendaStatus = storage.AppointmentStorage.Statuses[sourceAppointment.StatusId];
            agendaAppointment.AgendaLabel = storage.GetLabelColor(sourceAppointment.LabelId);
            agendaAppointment.SourceAppointment = sourceAppointment;
            return agendaAppointment;
        }
    }

    // Agenda appointment
    public class AgendaAppointment {
        public DevExpress.Xpf.Scheduler.AppointmentStatus AgendaStatus { get; set; }
        public string AgendaSubject { get; set; }
        public string AgendaDescription { get; set; }
        public string AgendaDuration { get; set; }
        public string AgendaLocation { get; set; }
        public DateTime AgendaDate { get; set; }
        public Color AgendaLabel { get; set; }
        public Appointment SourceAppointment { get; set; }
    }
}