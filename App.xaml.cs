﻿using Daidokoro.View;

namespace Daidokoro
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
