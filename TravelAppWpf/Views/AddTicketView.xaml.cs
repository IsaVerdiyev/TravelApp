﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TravelAppWpf.Views
{
    /// <summary>
    /// Interaction logic for AddTicketView.xaml
    /// </summary>
    public partial class AddTicketView : UserControl
    {
        public AddTicketView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".pdf";
            fileDialog.Filter = "Pdf filed (*.pdf)|*.pdf";
            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                string filename = fileDialog.FileName;
                PathToFileTextBox.Text = filename;
            }
        }
    }
}
