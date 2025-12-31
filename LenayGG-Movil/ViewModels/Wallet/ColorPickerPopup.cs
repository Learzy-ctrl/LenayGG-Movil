using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil.ViewModels.Wallet
{
    public class ColorPickerPopup : Popup
    {
        private readonly ColorPaletteDrawable _drawable;

        public ColorPickerPopup()
        {
            _drawable = new ColorPaletteDrawable();

            var layout = new VerticalStackLayout
            {
                Padding = 10,
                Spacing = 10,
                BackgroundColor = Colors.White,
                WidthRequest = 300,
                HeightRequest = 400,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var colorPicker = new GraphicsView
            {
                Drawable = _drawable,
                HeightRequest = 300,
                WidthRequest = 300
            };

            // Detectar toque en el GraphicsView
            colorPicker.StartInteraction += (sender, e) =>
            {
                var x = (float)e.Touches.First().X;
                var y = (float)e.Touches.First().Y;

                // Establecer la posición de la marca
                _drawable.SetMarkerPosition(x, y);

                // Actualizar el GraphicsView
                colorPicker.Invalidate();

                // Opcional: Previsualizar el color seleccionado
                var selectedColor = ColorPaletteDrawable.GetColorFromPosition(x, y, colorPicker.Width, colorPicker.Height);
                Console.WriteLine($"Color seleccionado: {selectedColor.ToHex()}");
            };

            var selectButton = new Button
            {
                Text = "Seleccionar Color",
                BackgroundColor = Colors.Blue,
                TextColor = Colors.White,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            selectButton.Clicked += (sender, args) =>
            {
                // Obtener el color seleccionado en la posición de la marca
                if (_drawable != null)
                {
                    var selectedColor = ColorPaletteDrawable.GetColorFromPosition(
                        _drawable.MarkerX,
                        _drawable.MarkerY,
                        colorPicker.Width,
                        colorPicker.Height
                    );

                    Close(selectedColor.ToHex());
                }
                else
                {
                    Close(null); // No se seleccionó color
                }
            };

            layout.Add(colorPicker);
            layout.Add(selectButton);

            Content = layout;
        }
    }
}
