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
            InitializeComponent(); // Debe estar aquí para inicializar el componente
            AccountTypes = new List<string> { "Crédito", "Nómina", "Débito", "Efectivo" };
            BindingContext = this;
        }

        private void OnAccountTypeSelected(object sender, EventArgs e)
        {
            var selectedType = accountTypePicker.SelectedItem?.ToString();
            dynamicFields.Children.Clear(); // Limpiar los campos dinámicos existentes

            if (selectedType == "Crédito")
            {
                dynamicFields.Children.Add(CreateEntryField("Límite de Crédito", "Gray"));
                dynamicFields.Children.Add(CreateEntryField("Fecha de Pago", "Gray"));
                dynamicFields.Children.Add(CreateEntryField("Fecha de Corte", "Gray"));
                dynamicFields.Children.Add(CreateEntryField("Tasa de Interés", "Gray"));
            }
            else if (selectedType == "Nómina" || selectedType == "Débito")
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
            // Lógica para agregar billetera
        }
    }
}

