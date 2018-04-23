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
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Windows
Imports DevExpress.Xpf.Core

Namespace AgendaViewDemo
    ''' <summary>
    ''' Interaction logic for App.xaml
    ''' </summary>
    Partial Public Class App
        Inherits Application

        Protected Overrides Sub OnStartup(ByVal e As StartupEventArgs)
            MyBase.OnStartup(e)
           ' DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName();
        End Sub

        Private Sub OnAppStartup_UpdateThemeName(sender As Object, e As StartupEventArgs)

            DevExpress.Xpf.Core.ApplicationThemeHelper.UpdateApplicationThemeName()
        End Sub
    End Class
End Namespace
