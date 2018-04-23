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


Namespace AgendaViewDemo
    ''' <summary>
    ''' Interaction logic for GoToDateDialog.xaml
    ''' </summary>
    Partial Public Class GoToDateDialog
        Inherits DXWindow

        Public Property SelectedDate() As Date
            Get
                Return dateEditGoToDate.DateTime
            End Get
            Set(ByVal value As Date)
                dateEditGoToDate.EditValue = value
            End Set
        End Property
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Me.DialogResult = True
        End Sub
    End Class
End Namespace
