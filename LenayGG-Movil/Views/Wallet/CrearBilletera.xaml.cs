using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace LenayGG_Movil.Views.Wallet
{
    public partial class CrearBilletera : ContentPage
    {
        public List<string> AccountTypes { get; set; }

        public CrearBilletera()
        {
            InitializeComponent(); // Debe estar aqu� para inicializar el componente
            AccountTypes = new List<string> { "Cr�dito", "N�mina", "D�bito", "Efectivo" };
            BindingContext = this;
        }

        private void OnAccountTypeSelected(object sender, EventArgs e)
        {
            var selectedType = accountTypePicker.SelectedItem?.ToString();
            dynamicFields.Children.Clear(); // Limpiar los campos din�micos existentes

            if (selectedType == "Cr�dito")
            {
                dynamicFields.Children.Add(CreateEntryField("L�mite de Cr�dito", "Gray"));
                dynamicFields.Children.Add(CreateEntryField("Fecha de Pago", "Gray"));
                dynamicFields.Children.Add(CreateEntryField("Fecha de Corte", "Gray"));
                dynamicFields.Children.Add(CreateEntryField("Tasa de Inter�s", "Gray"));
            }
            else if (selectedType == "N�mina" || selectedType == "D�bito")
            {
                dynamicFields.Children.Add(CreateEntryField("Saldo Inicial", "Gray"));
            }
            else if (selectedType == "Efectivo")
            {
                dynamicFields.Children.Add(CreateEntryField("Saldo Inicial", "Gray"));
            }
        }

        private StackLayout CreateEntryField(string placeholder, string placeholderColor)
        {
            return new StackLayout
            {
                Margin = new Thickness(0, 10, 0, 10), // Agregar un poco de margen
                Children =
                {
                    new Label { Text = placeholder, TextColor = Colors.Gray },
                    new Entry { Placeholder = placeholder, PlaceholderColor = Color.FromHex(placeholderColor) }
                }
            };
        }

        private void OnAddWalletButtonClicked(object sender, EventArgs e)
        {
            // L�gica para agregar billetera
        }
    }
}

