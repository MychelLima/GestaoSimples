using System;
using System.Windows.Forms;
using GestaoSimples.Data;
using GestaoSimples.Forms;

namespace GestaoSimples
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Database.Inicializar();

            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
        }
    }
}