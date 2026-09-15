using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GestaoSimples.Data;
using GestaoSimples.Forms;


namespace GestaoSimples.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsuario = new TextBox();
        private TextBox txtSenha = new TextBox();
        private Button btnEntrar = new Button();
        private Label lblErro = new Label();

        public LoginForm()
        {
            InitializeComponent();
            MontarTela();
        }

        private void MontarTela()
        {
            Text = "Sistema de Gestão - Login";
            Width = 380;
            Height = 320;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            BackColor = Tema.FundoPrincipal;

            var lblTitulo = new Label
            {
                Text = "Sistema de Gestão",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Tema.TextoPrimario,
                AutoSize = true,
                Location = new Point(90, 25)
            };

            var lblUsuario = new Label { Text = "Usuário:", ForeColor = Tema.TextoSecundario, Location = new Point(40, 90), AutoSize = true };
            txtUsuario.Location = new Point(120, 87);
            txtUsuario.Width = 180;
            txtUsuario.BackColor = Tema.FundoInput;
            txtUsuario.ForeColor = Tema.TextoPrimario;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;

            var lblSenha = new Label { Text = "Senha:", ForeColor = Tema.TextoSecundario, Location = new Point(40, 130), AutoSize = true };
            txtSenha.Location = new Point(120, 127);
            txtSenha.Width = 180;
            txtSenha.PasswordChar = '•';
            txtSenha.BackColor = Tema.FundoInput;
            txtSenha.ForeColor = Tema.TextoPrimario;
            txtSenha.BorderStyle = BorderStyle.FixedSingle;

            btnEntrar.Text = "Entrar";
            btnEntrar.Location = new Point(120, 170);
            btnEntrar.Width = 180;
            btnEntrar.Height = 35;
            btnEntrar.Click += BtnEntrar_Click;
            btnEntrar.BackColor = Tema.Verde;
            btnEntrar.ForeColor = Color.Black;
            btnEntrar.FlatStyle = FlatStyle.Flat;
            btnEntrar.FlatAppearance.BorderSize = 0;
            btnEntrar.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnEntrar.Cursor = Cursors.Hand;
            btnEntrar.MouseEnter += (s, e) => btnEntrar.BackColor = Tema.VerdeHover;
            btnEntrar.MouseLeave += (s, e) => btnEntrar.BackColor = Tema.Verde;

            lblErro.Location = new Point(40, 220);
            lblErro.Width = 300;
            lblErro.ForeColor = Color.Red;
            lblErro.AutoSize = false;

            Controls.Add(lblTitulo);
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblSenha);
            Controls.Add(txtSenha);
            Controls.Add(btnEntrar);
            Controls.Add(lblErro);
        }

        private void BtnEntrar_Click(object? sender, EventArgs e)
        {
            lblErro.Text = "";

            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                lblErro.Text = "Preencha usuário e senha.";
                return;
            }

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();

            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Perfil FROM Usuarios WHERE NomeUsuario = $usuario AND Senha = $senha";
            cmd.Parameters.AddWithValue("$usuario", txtUsuario.Text.Trim());
            cmd.Parameters.AddWithValue("$senha", txtSenha.Text.Trim());

            var resultado = cmd.ExecuteScalar();

            if (resultado == null)
            {
                lblErro.Text = "Usuário ou senha inválidos.";
                return;
            }

            string perfil = resultado.ToString() ?? "Vendedor";

            Hide();
            var main = new MainForm(txtUsuario.Text.Trim(), perfil);
            main.FormClosed += (s, args) => Close();
            main.Show();
        }
    }
}