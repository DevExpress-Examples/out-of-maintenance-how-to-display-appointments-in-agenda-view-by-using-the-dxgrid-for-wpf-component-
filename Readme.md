<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128657175/15.2.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T239055)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [AgendaViewDataGenerator.cs](./CS/AgendaViewDataGenerator.cs) (VB: [AgendaViewDataGenerator.vb](./VB/AgendaViewDataGenerator.vb))
* [CustomObjectsProvider.cs](./CS/CustomObjectsProvider.cs) (VB: [CustomObjectsProvider.vb](./VB/CustomObjectsProvider.vb))
* [GoToDateDialog.xaml](./CS/GoToDateDialog.xaml) (VB: [GoToDateDialog.xaml](./VB/GoToDateDialog.xaml))
* [GoToDateDialog.xaml.cs](./CS/GoToDateDialog.xaml.cs) (VB: [GoToDateDialog.xaml.vb](./VB/GoToDateDialog.xaml.vb))
* [MainWindow.xaml](./CS/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/MainWindow.xaml.cs) (VB: [MainWindow.xaml.vb](./VB/MainWindow.xaml.vb))
* [AgendaView.xaml](./CS/Views/AgendaView.xaml) (VB: [AgendaView.xaml](./VB/Views/AgendaView.xaml))
* [AgendaView.xaml.cs](./CS/Views/AgendaView.xaml.cs) (VB: [AgendaView.xaml.vb](./VB/Views/AgendaView.xaml.vb))
<!-- default file list end -->
# How to display appointments in Agenda View by using the DXGrid for WPF component


<p>The Agenda view is a list of upcoming events grouped by the appointment's date. This list can be displayed in the <a href="https://documentation.devexpress.com/#WPF/CustomDocument6294">GridControl component</a>.</p>
<p>This example demonstrates how to implement this behavior.<br /><br /></p>
<p><strong>Please see "Implementation Details" (click the corresponding link below this text) to learn more about technical aspects of this approach implementation.</strong></p>


<h3>Description</h3>

<p>Since multi-day appointments should be displayed as several GridView rows (such appointments should be displayed in each "Day" group in accordance with an appointment's duration), we used a separate AgendaAppointment class to store the appointment's data.</p>
<p>To get existing appointments from the SchedulerStorage and generate a corresponding collection of AgendaAppointment instances, the&nbsp;<a href="https://documentation.devexpress.com/#CoreLibraries/DevExpressXtraSchedulerSchedulerStorageBase_GetAppointmentstopic1830">SchedulerStorage.GetAppointments</a>&nbsp;method is used.</p>
<p>Switching between the SchedulerControl's view and a GridControl instance (used as an AgendaView) is implemented by using the&nbsp;<a href="https://documentation.devexpress.com/#WPF/CustomDocument15021">FlipView</a>&nbsp;control.</p>
<p>To generate a list of AgendaAppointment instances depending on existing appointments, the&nbsp;<strong>AgendaViewDataGenerator</strong>&nbsp;is used.</p>
<p>The GridControl is used to display the AgendaView located within the "<strong>AgendaViewControl</strong>" UserControl.</p>

<br/>


