' Developer Express Code Central Example:
' How to display appointments in Agenda View by using the DXGrid for WPF component
' 
' The Agenda view is a list of upcoming events grouped by the appointment's date.
' This list can be displayed in the GridControl component
' (https://documentation.devexpress.com/#WPF/CustomDocument6294).
' 
' This example
' demonstrates how to implement this behavior.
' 
' 
' Please see "Implementation
' Details" (click the corresponding link below this text) to learn more about
' technical aspects of this approach implementation.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=T239055

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Imaging
Imports System.Windows.Shapes
Imports DevExpress.Xpf.Core
Imports DevExpress.Xpf.Bars
Imports DevExpress.Xpf.Scheduler.UI


Namespace AgendaViewDemo
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits DXWindow

        Public Sub New()
            InitializeComponent()
            schedulerControl.Start = Date.Now
            schedulerControl.GroupType = DevExpress.XtraScheduler.SchedulerGroupType.Resource
            AddHandler schedulerControl.PopupMenuShowing, AddressOf schedulerControl_PopupMenuShowing
            AddHandler schedulerControl.QueryWorkTime, AddressOf schedulerControl_QueryWorkTime
        End Sub

        Private Sub schedulerControl_QueryWorkTime(ByVal sender As Object, ByVal e As DevExpress.XtraScheduler.QueryWorkTimeEventArgs)
            If e.Resource.Id Is DevExpress.XtraScheduler.Native.EmptyResource.Empty Then
                Return
            End If
            Dim resID As Integer = CInt((e.Resource.Id))
            If resID = 1 AndAlso e.Interval.Start.Day = 19 Then
                e.WorkTimes.Add(New DevExpress.XtraScheduler.TimeOfDayInterval(New TimeSpan(9, 00, 00), New TimeSpan(13, 00, 00)))
                e.WorkTimes.Add(New DevExpress.XtraScheduler.TimeOfDayInterval(New TimeSpan(14, 00, 00), New TimeSpan(16, 00, 00)))
                e.WorkTimes.Add(New DevExpress.XtraScheduler.TimeOfDayInterval(New TimeSpan(17, 00, 00), New TimeSpan(18, 00, 00)))
            End If
        End Sub

        Private Sub schedulerControl_PopupMenuShowing(ByVal sender As Object, ByVal e As DevExpress.Xpf.Scheduler.SchedulerMenuEventArgs)
            For i As Integer = 0 To e.Menu.Items.Count - 1
                Dim item As Object = e.Menu.Items(i)
                If TypeOf item Is BarItemSeparator Then
                    Continue For
                End If
                If TypeOf item Is BarSubItem AndAlso DirectCast(item, BarSubItem).Name = SchedulerMenuItemName.SwitchViewMenu Then
                    Dim newItem As New BarButtonItem()
                    newItem.Content = "Agenda View"
                    newItem.Glyph = New BitmapImage(New Uri("pack://application:,,,/Resources/Report.png", UriKind.Absolute))
                    AddHandler newItem.ItemClick, AddressOf OnSwitchToAgendaView
                    DirectCast(item, BarSubItem).Items.Add(newItem)
                End If
            Next i
        End Sub

        Private Sub OnSwitchToAgendaView(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            SwitchToAgendaView()
        End Sub

        Private Sub SwitchToAgendaView()
            flipView1.SelectedIndex = 0
        End Sub
    End Class
End Namespace
