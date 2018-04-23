using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Scheduler.UI;


namespace AgendaViewDemo {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow {
        public MainWindow() {
            InitializeComponent();
        }
        void schedulerControl_QueryWorkTime(object sender, DevExpress.XtraScheduler.QueryWorkTimeEventArgs e) {
            if (e.Resource.Id == DevExpress.XtraScheduler.Native.EmptyResource.Empty) return;
            int resID = (int)e.Resource.Id;
            if (resID == 1 && e.Interval.Start.Day == 19) {
                e.WorkTimes.Add(new DevExpress.XtraScheduler.TimeOfDayInterval(new TimeSpan(9, 00, 00), new TimeSpan(13, 00, 00)));
                e.WorkTimes.Add(new DevExpress.XtraScheduler.TimeOfDayInterval(new TimeSpan(14, 00, 00), new TimeSpan(16, 00, 00)));
                e.WorkTimes.Add(new DevExpress.XtraScheduler.TimeOfDayInterval(new TimeSpan(17, 00, 00), new TimeSpan(18, 00, 00)));
            }
        }
        private void schedulerControl_PopupMenuShowing(object sender, DevExpress.Xpf.Scheduler.SchedulerMenuEventArgs e) {
            BarSubItem subItem = e.Menu.Items.Where(x => x is BarSubItem && ((BarSubItem)x).Name == SchedulerMenuItemName.SwitchViewMenu).Cast<BarSubItem>().FirstOrDefault();
            if (subItem == null)
                return;
            BarButtonItem newItem = new BarButtonItem { Content = "Agenda View", Glyph = new BitmapImage(new Uri("pack://application:,,,/Resources/Report.png", UriKind.Absolute)) };
            newItem.ItemClick += OnSwitchToAgendaViewItemClick;
            subItem.Items.Add(newItem);
        }
        void OnSwitchToAgendaViewItemClick(object sender, ItemClickEventArgs e) {
            SwitchToAgendaView();
        }
        void SwitchToAgendaView() {
            flipView1.SelectedIndex = 0;
        }
    }
}