﻿using CefSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CefSharpTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var settings = new CefSettings
            {
                BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe"
            };

            Cef.Initialize(settings);
        }
    }
}