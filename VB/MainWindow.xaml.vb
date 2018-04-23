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
        End Sub
        Private Sub schedulerControl_QueryWorkTime(ByVal sender As Object, ByVal e As DevExpress.XtraScheduler.QueryWorkTimeEventArgs)
            If e.Resource.Id = DevExpress.XtraScheduler.ResourceEmpty.Id Then
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
            Dim subItem As BarSubItem = e.Menu.Items.Where(Function(x) TypeOf x Is BarSubItem AndAlso CType(x, BarSubItem).Name = SchedulerMenuItemName.SwitchViewMenu).Cast(Of BarSubItem)().FirstOrDefault()
            If subItem Is Nothing Then
                Return
            End If
            Dim newItem As BarButtonItem = New BarButtonItem With {.Content = "Agenda View", .Glyph = New BitmapImage(New Uri("pack://application:,,,/Resources/Report.png", UriKind.Absolute))}
            AddHandler newItem.ItemClick, AddressOf OnSwitchToAgendaViewItemClick
            subItem.Items.Add(newItem)
        End Sub
        Private Sub OnSwitchToAgendaViewItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            SwitchToAgendaView()
        End Sub
        Private Sub SwitchToAgendaView()
            flipView1.SelectedIndex = 0
        End Sub
    End Class
End Namespace