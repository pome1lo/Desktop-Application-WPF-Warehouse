using app.Models;
using app.Views.Pages;
using app.Views.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace app.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static Frame MainFrame = new();
        protected static User CurrentUser { get; set; } = new();
        
        protected static void ShowMainWindow()
        {
            MainView view = new MainView();
            MainFrame = view.MainFrame;
            ShowPage(new HomeView());
            view.Show();
        }

        protected static void ShowPage(Page page) => MainFrame.Content = page;
        protected static void SendToModalWindow(string content) => new ModalWindow(content).ShowDialog();
    }
}
