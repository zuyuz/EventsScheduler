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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventsScheduler
{
    public static class DataManager
    {
        private static string staticLogin;

        public static string StaticLogin
        {
            get
            {
                return staticLogin;
            }

            set
            {
                staticLogin = value;
            }
        }

        public static string OutputGreeting()
        {
            return ("Hello " + staticLogin + "!");
        }
    }
}