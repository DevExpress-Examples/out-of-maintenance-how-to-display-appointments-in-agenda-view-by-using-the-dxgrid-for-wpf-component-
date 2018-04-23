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
