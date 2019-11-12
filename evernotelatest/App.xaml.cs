using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EverNoteLatest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string UserId;
        public static MobileServiceClient MobileServiceClient = new MobileServiceClient("https://evernoteappsrv.azurewebsites.net");
    }
}
