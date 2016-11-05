﻿using System;
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
using System.Windows.Shapes;

namespace EventsScheduler
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = this.Owner as MainWindow;

            if (NameTextBox.Text == "")
            {
                MessageBox.Show("Invalid name!");
            }
            else if (EmailTextBox.Text == "")
            {
                MessageBox.Show("Invalid email!");
            }
            else if (LoginTextBox.Text == "")
            {
                MessageBox.Show("Invalid login");
            }
            else if (PasswordBox.Password == "")
            {
                MessageBox.Show("Invalid password!");
            }

            Close();
        }
    }
}