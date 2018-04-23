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


