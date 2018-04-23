Imports DevExpress.Utils
Imports DevExpress.Xpf.Grid
Imports DevExpress.Xpf.Scheduler
Imports DevExpress.Xpf.WindowsUI.Navigation
Imports DevExpress.XtraScheduler
Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports DevExpress.Mvvm
Imports System.ComponentModel
Imports DevExpress.Xpf.WindowsUI

Namespace AgendaViewDemo.Views
    ''' <summary>
    ''' Interaction logic for AgendaViewControl.xaml
    ''' </summary>
    Partial Public Class AgendaView
        Inherits UserControl
        Implements INotifyPropertyChanged


        Private selectedResource_Renamed As Resource = Nothing

        Private selectedAppointment_Renamed As AgendaAppointment = Nothing
        Private appointmentImages As ImageCollection

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        #Region "Properties"
        Public Shared ReadOnly OwnerSchedulerProperty As DependencyProperty = DependencyProperty.Register("OwnerScheduler", GetType(DevExpress.Xpf.Scheduler.SchedulerControl), GetType(AgendaView), New UIPropertyMetadata(Nothing))
        Public Property OwnerScheduler() As DevExpress.Xpf.Scheduler.SchedulerControl
            Get
                Return TryCast(Me.GetValue(OwnerSchedulerProperty), DevExpress.Xpf.Scheduler.SchedulerControl)
            End Get
            Set(ByVal value As DevExpress.Xpf.Scheduler.SchedulerControl)
                Me.SetValue(OwnerSchedulerProperty, value)
            End Set
        End Property
        Public ReadOnly Property IsAppointmentSelected() As Boolean
            Get
                Return If(SelectedAppointment Is Nothing, False, True)
            End Get
        End Property
        Public Property SelectedResource() As Resource
            Get
                Return selectedResource_Renamed
            End Get
            Set(ByVal value As Resource)
                If selectedResource_Renamed IsNot value Then
                    selectedResource_Renamed = value
                End If
            End Set
        End Property
        Public Property SelectedAppointment() As AgendaAppointment
            Get
                Return selectedAppointment_Renamed
            End Get
            Set(ByVal value As AgendaAppointment)
                If selectedAppointment_Renamed IsNot value Then
                    selectedAppointment_Renamed = value
                    NotifyPropertyChanged("IsAppointmentSelected")
                End If
            End Set
        End Property
        #End Region

        Public Sub New()
            DataContext = Me
            InitializeComponent()
        End Sub

        Private Sub OnLoaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If OwnerScheduler IsNot Nothing Then
                Dim selectedIntervalStart As Date = OwnerScheduler.ActiveView.GetVisibleIntervals().Start
                Dim intervalStart As New Date(selectedIntervalStart.Year, selectedIntervalStart.Month, 1)
                AgendaViewDataGenerator.SelectedInterval = New TimeInterval(intervalStart, intervalStart.AddMonths(1))
                InitializeResources()
                InitializeAppointments()
                appointmentImages = DevExpress.Utils.Controls.ImageHelper.CreateImageCollectionCore(My.Resources.AppointmentImages, New System.Drawing.Size(15, 15), System.Drawing.Color.Transparent)
            End If
        End Sub

        #Region "Popup menu events"
        Private Sub OnOpenItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            AddHandler OwnerScheduler.Storage.AppointmentsChanged, AddressOf OnAppointmentsChanged
            OwnerScheduler.ShowEditAppointmentForm(SelectedAppointment.SourceAppointment, SelectedAppointment.SourceAppointment.IsRecurring)
        End Sub
        Private Sub OnDeleteItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            OwnerScheduler.Storage.AppointmentStorage.Remove(SelectedAppointment.SourceAppointment)
            InitializeAppointments()
        End Sub
        Private Sub OnNextMonthItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            AgendaViewDataGenerator.SelectedInterval = New TimeInterval(AgendaViewDataGenerator.SelectedInterval.End, AgendaViewDataGenerator.SelectedInterval.End.AddMonths(1))
            InitializeAppointments()
        End Sub
        Private Sub OnPrevMonthItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            AgendaViewDataGenerator.SelectedInterval = New TimeInterval(AgendaViewDataGenerator.SelectedInterval.Start.AddMonths(-1), AgendaViewDataGenerator.SelectedInterval.Start)
            InitializeAppointments()
        End Sub
        Private Sub OnGotoDateItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            Dim dateDialog As New GoToDateDialog()
            dateDialog.SelectedDate = AgendaViewDataGenerator.SelectedInterval.Start
            If dateDialog.ShowDialog() = True Then
                Dim intervalStart As New Date(dateDialog.SelectedDate.Year, dateDialog.SelectedDate.Month, 1)
                AgendaViewDataGenerator.SelectedInterval = New TimeInterval(intervalStart, intervalStart.AddMonths(1))
                InitializeAppointments()
            End If
        End Sub
        Private Sub OnDayViewItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            OwnerScheduler.ActiveViewType = SchedulerViewType.Day
            SwitchToMainvView()
        End Sub
        Private Sub OnWeekViewItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            OwnerScheduler.ActiveViewType = SchedulerViewType.Week
            SwitchToMainvView()
        End Sub
        Private Sub OnWorkWeekViewItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            OwnerScheduler.ActiveViewType = SchedulerViewType.WorkWeek
            SwitchToMainvView()

        End Sub
        Private Sub bbiMonthView_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            OwnerScheduler.ActiveViewType = SchedulerViewType.Month
            SwitchToMainvView()
        End Sub
        Private Sub OnTimelineViewItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
            OwnerScheduler.ActiveViewType = SchedulerViewType.Timeline
            SwitchToMainvView()
        End Sub
        #End Region

        #Region "Methods"
        Private Sub SwitchToMainvView()
            TryCast(Me.Parent, FlipView).SelectedIndex = 1
        End Sub
        Private Sub InitializeResources()
            lbResources.ItemsSource = AgendaViewDataGenerator.GenerateResourcesCollection(OwnerScheduler.Storage)
        End Sub
        Public Sub InitializeAppointments()
            gridControlAppointments.ItemsSource = AgendaViewDataGenerator.GenerateAgendaAppointmentCollection(OwnerScheduler.Storage)
        End Sub
        Private Sub NotifyPropertyChanged(ByVal propertyName As String)
            If Me.PropertyChangedEvent IsNot Nothing Then
                Me.UpdateLayout()
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub
        #End Region

        Private Sub OnCustomRowFilter(ByVal sender As Object, ByVal e As RowFilterEventArgs)
            If SelectedResource IsNot Nothing Then
                Dim sourceAppointment As Appointment = (TryCast((TryCast(sender, GridControl)).GetRow(e.ListSourceRowIndex), AgendaAppointment)).SourceAppointment
                e.Visible = sourceAppointment.ResourceIds.Contains(SelectedResource.Id)
                e.Handled = True
            End If
        End Sub
        Private Sub OnCustomUnboundColumnData(ByVal sender As Object, ByVal e As GridColumnDataEventArgs)
            Dim currentApt As Appointment = (TryCast(gridControlAppointments.GetRowByListIndex(e.ListSourceRowIndex), AgendaAppointment)).SourceAppointment
            If e.Column.FieldName = "gridColumnRecurring" AndAlso e.IsGetData AndAlso currentApt.IsRecurring Then
                e.Value = ImageToBitmapImage.Convert(appointmentImages.Images(2))
            End If

            If e.Column.FieldName = "gridColumnReminder" AndAlso e.IsGetData AndAlso currentApt.HasReminder Then
                e.Value = ImageToBitmapImage.Convert(appointmentImages.Images(4))
            End If
        End Sub
        Private Sub OnListBoxSelectedIndexChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
            SelectedResource = CType(lbResources.SelectedItem, Resource)
            gridControlAppointments.RefreshData()
        End Sub
        Private Sub OnFocusedRowChanged(ByVal sender As Object, ByVal e As FocusedRowChangedEventArgs)
            If gridControlAppointments.IsGroupRowHandle(gridViewAppointments.FocusedRowHandle) Then
                SelectedAppointment = Nothing
            Else
                SelectedAppointment = CType(gridControlAppointments.GetFocusedRow(), AgendaAppointment)
            End If
        End Sub
        Private Sub OnAppointmentsChanged(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
            InitializeAppointments()
            RemoveHandler OwnerScheduler.Storage.AppointmentsChanged, AddressOf OnAppointmentsChanged
        End Sub
        Private Sub gridViewAppointments_RowDoubleClick(ByVal sender As Object, ByVal e As RowDoubleClickEventArgs)
            RemoveHandler OwnerScheduler.Storage.AppointmentsChanged, AddressOf OnAppointmentsChanged
            AddHandler OwnerScheduler.Storage.AppointmentsChanged, AddressOf OnAppointmentsChanged
            OwnerScheduler.ShowEditAppointmentForm(SelectedAppointment.SourceAppointment, SelectedAppointment.SourceAppointment.IsRecurring)
        End Sub
    End Class

    Public Class ImageToBitmapImageConverter
        Implements IValueConverter

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Dim bitmapImage As New BitmapImage()
            Dim bitmap As System.Drawing.Image
            If value IsNot Nothing Then
                Return value
            Else
                bitmap = My.Resources.no_photo_icon
                Return ImageToBitmapImage.Convert(bitmap)
            End If
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return value
        End Function
    End Class

    Public Class ColorToBrushConverter
        Implements IValueConverter

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Dim brush As System.Windows.Media.SolidColorBrush
            Dim color As System.Windows.Media.Color = System.Windows.Media.Colors.White
            If value IsNot Nothing Then
                color = System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B)
            End If
            brush = New SolidColorBrush(color)
            Return brush
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return value
        End Function
    End Class

    Public NotInheritable Class ImageToBitmapImage

        Private Sub New()
        End Sub

        ' "Bad metadata" exception prevents image from appearing on a BitmapImage object
        ' https://social.msdn.microsoft.com/Forums/vstudio/en-US/0f037b9c-779d-45ad-b797-01c25999491b/bad-metadata-exception-prevents-image-from-appearing-on-a-bitmapimage-object?forum=wpf
        Public Shared Function Convert(ByVal image As System.Drawing.Image) As BitmapImage
            Dim badMetadataImage As System.Drawing.Bitmap = CType(image, System.Drawing.Bitmap)
            Dim myImageCodecInfo As ImageCodecInfo
            Dim myEncoder As System.Drawing.Imaging.Encoder
            Dim myEncoderParameter As EncoderParameter
            Dim myEncoderParameters As EncoderParameters
            ' get an ImageCodecInfo object that represents the JPEG codec
            myImageCodecInfo = GetEncoderInfo("image/png")
            ' Create an Encoder object based on the GUID for the Quality parameter category
            myEncoder = System.Drawing.Imaging.Encoder.Quality
            ' Create an EncoderParameters object
            ' An EncoderParameters object has an array of EncoderParameter objects.
            ' In this case, there is only one EncoderParameter object in the array.
            myEncoderParameters = New EncoderParameters(1)
            ' Save the image as a JPEG file with quality level 75.
            myEncoderParameter = New EncoderParameter(myEncoder, 75L)
            myEncoderParameters.Param(0) = myEncoderParameter
            Dim memory As New MemoryStream()
            badMetadataImage.Save(memory, myImageCodecInfo, myEncoderParameters)
            ' Create the source to use as the tb source
            Dim bi As New BitmapImage()
            bi.BeginInit()
            bi.StreamSource = memory
            bi.EndInit()
            Return bi
        End Function

        Private Shared Function GetEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
            Dim j As Integer
            Dim encoders() As ImageCodecInfo
            encoders = ImageCodecInfo.GetImageEncoders()
            For j = 0 To encoders.Length - 1
                If encoders(j).MimeType = mimeType Then
                    Return encoders(j)
                End If
            Next j
            Return Nothing
        End Function
    End Class
    Public Class IntToStringConverter
        Implements IValueConverter

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
            Return If(TypeOf value Is Integer AndAlso DirectCast(value, Integer) = 0, "No items", String.Empty)
        End Function
        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace