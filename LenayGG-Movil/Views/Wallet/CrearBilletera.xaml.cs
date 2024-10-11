using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace LenayGG_Movil.Views.Wallet
{
    public partial class CrearBilletera : ContentPage
    {
        public CrearBilletera()
        {
            InitializeComponent();
            CardOption.Content = new OpcionesOtrasCuentas();
        }
    }
}

