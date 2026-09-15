using System.Drawing;
using System.Windows.Forms;

namespace GestaoSimples.Data
{
    public static class Tema
    {
        public static readonly Color FundoPrincipal = ColorTranslator.FromHtml("#121212");
        public static readonly Color FundoMenu = ColorTranslator.FromHtml("#000000");
        public static readonly Color FundoCard = ColorTranslator.FromHtml("#181818");
        public static readonly Color FundoInput = ColorTranslator.FromHtml("#2A2A2A");
        public static readonly Color FundoSelecionado = ColorTranslator.FromHtml("#282828");

        public static readonly Color Verde = ColorTranslator.FromHtml("#1DB954");
        public static readonly Color VerdeHover = ColorTranslator.FromHtml("#1ED760");

        public static readonly Color TextoPrimario = Color.White;
        public static readonly Color TextoSecundario = ColorTranslator.FromHtml("#B3B3B3");

        public static void EstilizarGrid(DataGridView grid)
        {
            grid.BackgroundColor = FundoPrincipal;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;

            grid.ColumnHeadersDefaultCellStyle.BackColor = FundoMenu;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = TextoPrimario;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.DefaultCellStyle.BackColor = FundoCard;
            grid.DefaultCellStyle.ForeColor = TextoPrimario;
            grid.DefaultCellStyle.SelectionBackColor = FundoSelecionado;
            grid.DefaultCellStyle.SelectionForeColor = Verde;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5f);
            grid.RowTemplate.Height = 32;

            grid.AlternatingRowsDefaultCellStyle.BackColor = FundoPrincipal;

            grid.GridColor = FundoPrincipal;
            grid.RowHeadersVisible = false;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }
    }

}