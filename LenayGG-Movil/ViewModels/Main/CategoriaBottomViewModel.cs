using LenayGG_Movil.Models.TransactionModel;
using LenayGG_Movil.Views.Main.Transacciones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LenayGG_Movil.ViewModels.Main
{
    public class CategoriaBottomViewModel : BaseViewModel
    {
        public ObservableCollection<ColorItem> Colores { get; set; }
        public CategoriaBottomSheet bottomSheet {  get; set; }
        public ICommand SelectColorCommand => new Command<ColorItem>(colorItem =>
        {
            MessagingCenter.Send(this, "ColorItemSelected", colorItem);
            bottomSheet.DismissAsync();
        });
        public CategoriaBottomViewModel()
        {
            Colores = new ObservableCollection<ColorItem>
            {
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                new ColorItem { Color = Colors.Red, Nombre = "Rojo" },
                new ColorItem { Color = Colors.Blue, Nombre = "Azul" },
                new ColorItem { Color = Colors.Green, Nombre = "Verde" },
                // Agrega más colores según sea necesario
            };
        }
    }
}
