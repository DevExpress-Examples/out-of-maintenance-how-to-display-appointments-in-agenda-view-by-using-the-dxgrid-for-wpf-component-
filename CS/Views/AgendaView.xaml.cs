using DevExpress.Utils;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Scheduler;
using DevExpress.Xpf.WindowsUI.Navigation;
using DevExpress.XtraScheduler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Mvvm;
using System.ComponentModel;
using DevExpress.Xpf.WindowsUI;

namespace AgendaViewDemo.Views {
    /// <summary>
    /// Interaction logic for AgendaViewControl.xaml
    /// </summary>
    public partial class AgendaView : UserControl, INotifyPropertyChanged {
        Resource selectedResource = null;
        AgendaAppointment selectedAppointment = null;
        ImageCollection appointmentImages;

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        public static readonly DependencyProperty OwnerSchedulerProperty = DependencyProperty.Register("OwnerScheduler", typeof(DevExpress.Xpf.Scheduler.SchedulerControl), typeof(AgendaView), new UIPropertyMetadata(null));
        public DevExpress.Xpf.Scheduler.SchedulerControl OwnerScheduler {
            get {
                return this.GetValue(OwnerSchedulerProperty) as DevExpress.Xpf.Scheduler.SchedulerControl;
            }
            set {
                this.SetValue(OwnerSchedulerProperty, value);
            }
        }
        public bool IsAppointmentSelected {
            get {
                return SelectedAppointment == null ? false : true;
            }
        }
        public Resource SelectedResource {
            get { return selectedResource; }
            set {
                if (selectedResource != value) {
                    selectedResource = value;
                }
            }
        }
        public AgendaAppointment SelectedAppointment {
            get { return selectedAppointment; }
            set {
                if (selectedAppointment != value) {
                    selectedAppointment = value;
                    NotifyPropertyChanged("IsAppointmentSelected");
                }
            }
        }
        #endregion

        public AgendaView() {
            DataContext = this;
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            if (OwnerScheduler != null) {
                DateTime selectedIntervalStart = OwnerScheduler.ActiveView.GetVisibleIntervals().Start;
                DateTime intervalStart = new DateTime(selectedIntervalStart.Year, selectedIntervalStart.Month, 1);
                AgendaViewDataGenerator.SelectedInterval = new TimeInterval(intervalStart, intervalStart.AddMonths(1));
                InitializeResources();
                InitializeAppointments();
                appointmentImages = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionCore(Properties.Resources.AppointmentImages, new System.Drawing.Size(15, 15), System.Drawing.Color.Transparent);
            }
        }

        #region Popup menu events
        private void OnOpenItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            OwnerScheduler.Storage.AppointmentsChanged += OnAppointmentsChanged;
            OwnerScheduler.ShowEditAppointmentForm(SelectedAppointment.SourceAppointment, SelectedAppointment.SourceAppointment.IsRecurring);
        }
        private void OnDeleteItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            OwnerScheduler.Storage.AppointmentStorage.Remove(SelectedAppointment.SourceAppointment);
            InitializeAppointments();
        }
        private void OnNextMonthItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            AgendaViewDataGenerator.SelectedInterval = new TimeInterval(AgendaViewDataGenerator.SelectedInterval.End, AgendaViewDataGenerator.SelectedInterval.End.AddMonths(1));
            InitializeAppointments();
        }
        private void OnPrevMonthItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            AgendaViewDataGenerator.SelectedInterval = new TimeInterval(AgendaViewDataGenerator.SelectedInterval.Start.AddMonths(-1), AgendaViewDataGenerator.SelectedInterval.Start);
            InitializeAppointments();
        }
        private void OnGotoDateItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            GoToDateDialog dateDialog = new GoToDateDialog();
            dateDialog.SelectedDate = AgendaViewDataGenerator.SelectedInterval.Start;
            if (dateDialog.ShowDialog() == true) {
                DateTime intervalStart = new DateTime(dateDialog.SelectedDate.Year, dateDialog.SelectedDate.Month, 1);
                AgendaViewDataGenerator.SelectedInterval = new TimeInterval(intervalStart, intervalStart.AddMonths(1));
                InitializeAppointments();
            }
        }
        private void OnDayViewItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            OwnerScheduler.ActiveViewType = SchedulerViewType.Day;
            SwitchToMainvView();
        }
        private void OnWeekViewItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            OwnerScheduler.ActiveViewType = SchedulerViewType.Week;
            SwitchToMainvView();
        }
        private void OnWorkWeekViewItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            OwnerScheduler.ActiveViewType = SchedulerViewType.WorkWeek;
            SwitchToMainvView();

        }
        private void bbiMonthView_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            OwnerScheduler.ActiveViewType = SchedulerViewType.Month;
            SwitchToMainvView();
        }
        private void OnTimelineViewItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            OwnerScheduler.ActiveViewType = SchedulerViewType.Timeline;
            SwitchToMainvView();
        }
        #endregion

        #region Methods
        void SwitchToMainvView() {
            (this.Parent as FlipView).SelectedIndex = 1;
        }
        void InitializeResources() {
            lbResources.ItemsSource = AgendaViewDataGenerator.GenerateResourcesCollection(OwnerScheduler.Storage);
        }
        public void InitializeAppointments() {
            gridControlAppointments.ItemsSource = AgendaViewDataGenerator.GenerateAgendaAppointmentCollection(OwnerScheduler.Storage);
        }
        void NotifyPropertyChanged(String propertyName) {
            if (this.PropertyChanged != null) {
                this.UpdateLayout();
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void OnCustomRowFilter(object sender, RowFilterEventArgs e) {
            if (SelectedResource != null) {
                Appointment sourceAppointment = ((sender as GridControl).GetRow(e.ListSourceRowIndex) as AgendaAppointment).SourceAppointment;
                e.Visible = sourceAppointment.ResourceIds.Contains(SelectedResource.Id);
                e.Handled = true;
            }
        }
        private void OnCustomUnboundColumnData(object sender, GridColumnDataEventArgs e) {
            Appointment currentApt = (gridControlAppointments.GetRowByListIndex(e.ListSourceRowIndex) as AgendaAppointment).SourceAppointment;
            if (e.Column.FieldName == "gridColumnRecurring" && e.IsGetData && currentApt.IsRecurring) {
                e.Value = ImageToBitmapImage.Convert(appointmentImages.Images[2]);
            }

            if (e.Column.FieldName == "gridColumnReminder" && e.IsGetData && currentApt.HasReminder) {
                e.Value = ImageToBitmapImage.Convert(appointmentImages.Images[4]);
            }
        }
        private void OnListBoxSelectedIndexChanged(object sender, RoutedEventArgs e) {
            SelectedResource = (Resource)lbResources.SelectedItem;
            gridControlAppointments.RefreshData();
        }
        private void OnFocusedRowChanged(object sender, FocusedRowChangedEventArgs e) {
            if (gridControlAppointments.IsGroupRowHandle(gridViewAppointments.FocusedRowHandle))
                SelectedAppointment = null;
            else
                SelectedAppointment = (AgendaAppointment)gridControlAppointments.GetFocusedRow();
        }
        void OnAppointmentsChanged(object sender, PersistentObjectsEventArgs e) {
            InitializeAppointments();
            OwnerScheduler.Storage.AppointmentsChanged -= OnAppointmentsChanged;
        }
        private void gridViewAppointments_RowDoubleClick(object sender, RowDoubleClickEventArgs e) {
            OwnerScheduler.Storage.AppointmentsChanged -= OnAppointmentsChanged;
            OwnerScheduler.Storage.AppointmentsChanged += OnAppointmentsChanged;
            OwnerScheduler.ShowEditAppointmentForm(SelectedAppointment.SourceAppointment, SelectedAppointment.SourceAppointment.IsRecurring);
        }
    }

    public class ImageToBitmapImageConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            BitmapImage bitmapImage = new BitmapImage();
            System.Drawing.Image bitmap;
            if (value != null)
                return value;
            else {
                bitmap = AgendaViewDemo.Properties.Resources.no_photo_icon;
                return ImageToBitmapImage.Convert(bitmap);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value;
        }
    }

    public class ColorToBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            System.Windows.Media.SolidColorBrush brush;
            System.Windows.Media.Color color = System.Windows.Media.Colors.White;
            if (value != null)
                color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
            brush = new SolidColorBrush(color);
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value;
        }
    }

    public static class ImageToBitmapImage {
        // "Bad metadata" exception prevents image from appearing on a BitmapImage object
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/0f037b9c-779d-45ad-b797-01c25999491b/bad-metadata-exception-prevents-image-from-appearing-on-a-bitmapimage-object?forum=wpf
        public static BitmapImage Convert(System.Drawing.Image image) {
            System.Drawing.Bitmap badMetadataImage = (System.Drawing.Bitmap)image;
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            // get an ImageCodecInfo object that represents the JPEG codec
            myImageCodecInfo = GetEncoderInfo("image/png");
            // Create an Encoder object based on the GUID for the Quality parameter category
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            // Create an EncoderParameters object
            // An EncoderParameters object has an array of EncoderParameter objects.
            // In this case, there is only one EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);
            // Save the image as a JPEG file with quality level 75.
            myEncoderParameter = new EncoderParameter(myEncoder, 75L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            MemoryStream memory = new MemoryStream();
            badMetadataImage.Save(memory, myImageCodecInfo, myEncoderParameters);
            // Create the source to use as the tb source
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = memory;
            bi.EndInit();
            return bi;
        }

        static ImageCodecInfo GetEncoderInfo(String mimeType) {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j) {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
    public class IntToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return value is int && (int)value == 0 ? "No items" : string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}