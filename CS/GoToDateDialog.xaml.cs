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


namespace AgendaViewDemo {
    /// <summary>
    /// Interaction logic for GoToDateDialog.xaml
    /// </summary>
    public partial class GoToDateDialog : DXWindow {

        public DateTime SelectedDate {
            get { return dateEditGoToDate.DateTime; }
            set { dateEditGoToDate.EditValue = value; }
        }
        public GoToDateDialog() {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
    }
}
