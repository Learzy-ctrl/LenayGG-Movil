using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LenayGG_Movil
{
    public class ColorPaletteDrawable : IDrawable
    {
        private float _markerX = -1; // Coordenadas de la marca (-1 para no dibujar)
        private float _markerY = -1;

        // Propiedades públicas para acceder a la posición de la marca
        public float MarkerX => _markerX;
        public float MarkerY => _markerY;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Dibujar la paleta de colores (igual que antes)
            for (int y = 0; y < dirtyRect.Height; y++)
            {
                float verticalFactor = (float)y / (float)dirtyRect.Height;

                for (int x = 0; x < dirtyRect.Width; x++)
                {
                    float horizontalFactor = (float)x / (float)dirtyRect.Width;

                    var color = Color.FromRgb(
                        (int)(255 * horizontalFactor),
                        (int)(255 * verticalFactor),
                        (int)(255 * (1 - horizontalFactor))
                    );

                    canvas.FillColor = color;
                    canvas.FillRectangle(x, y, 1, 1);
                }
            }

            // Dibujar la marca si se seleccionó un color
            if (_markerX >= 0 && _markerY >= 0)
            {
                canvas.StrokeColor = Colors.Black; // Contorno negro
                canvas.StrokeSize = 2;
                canvas.FillColor = Colors.Transparent; // Interior transparente

                // Dibujar un círculo en la posición seleccionada
                canvas.DrawEllipse(_markerX - 10, _markerY - 10, 20, 20);
            }
        }

        public void SetMarkerPosition(float x, float y)
        {
            _markerX = x;
            _markerY = y;
        }

        public static Color GetColorFromPosition(double x, double y, double width, double height)
        {
            double normalizedX = x / width;
            double normalizedY = y / height;

            return Color.FromRgb(
                (int)(255 * normalizedX),
                (int)(255 * normalizedY),
                (int)(255 * (1 - normalizedX))
            );
        }
    }
}
