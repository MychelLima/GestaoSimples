using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GestaoSimples.Data;

namespace GestaoSimples.Forms
{
    public partial class ClientesControl : UserControl
    {
        private DataGridView grid = new DataGridView();
        private TextBox txtNome = new TextBox();
        private TextBox txtEmail = new TextBox();
        private TextBox txtTelefone = new TextBox();
        private Button btnNovo = new Button();
        private Button btnSalvar = new Button();
        private Button btnExcluir = new Button();
        private int? idSelecionado = null;

        public ClientesControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            MontarLayout();
            CarregarDados();
        }

        private void MontarLayout()
        {
            BackColor = Tema.FundoPrincipal;
            var lblTitulo = new Label { Text = "Clientes", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Tema.TextoPrimario, AutoSize = true, Location = new Point(0, 0) };

            var lblNome = new Label { Text = "Nome:", ForeColor = Tema.TextoSecundario, Location = new Point(0, 40), AutoSize = true };
            txtNome.Location = new Point(70, 37);
            txtNome.Width = 200;
            txtNome.BackColor = Tema.FundoInput;
            txtNome.ForeColor = Tema.TextoPrimario;
            txtNome.BorderStyle = BorderStyle.FixedSingle;

            var lblEmail = new Label { Text = "Email:", ForeColor = Tema.TextoSecundario, Location = new Point(290, 40), AutoSize = true };
            txtEmail.Location = new Point(340, 37);
            txtEmail.Width = 200;
            txtEmail.BackColor = Tema.FundoInput;
            txtEmail.ForeColor = Tema.TextoPrimario;
            txtEmail.BorderStyle = BorderStyle.FixedSingle;

            var lblTelefone = new Label { Text = "Telefone:", ForeColor = Tema.TextoSecundario, Location = new Point(0, 75), AutoSize = true };
            txtTelefone.Location = new Point(70, 72);
            txtTelefone.Width = 200;
            txtTelefone.BackColor = Tema.FundoInput;
            txtTelefone.ForeColor = Tema.TextoPrimario;
            txtTelefone.BorderStyle = BorderStyle.FixedSingle;

            btnNovo.Text = "Novo";
            btnNovo.Location = new Point(340, 72);
            btnNovo.Width = 90;
            btnNovo.Click += (s, e) => LimparCampos();
            btnNovo.FlatStyle = FlatStyle.Flat;
            btnNovo.FlatAppearance.BorderColor = Tema.TextoSecundario;
            btnNovo.BackColor = Tema.FundoPrincipal;
            btnNovo.ForeColor = Tema.TextoPrimario;
            btnNovo.Cursor = Cursors.Hand;

            btnSalvar.Text = "Salvar";
            btnSalvar.Location = new Point(440, 72);
            btnSalvar.Width = 90;
            btnSalvar.Click += BtnSalvar_Click;
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.BackColor = Tema.Verde;
            btnSalvar.ForeColor = Color.Black;
            btnSalvar.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnSalvar.Cursor = Cursors.Hand;
            btnSalvar.MouseEnter += (s, e) => btnSalvar.BackColor = Tema.VerdeHover;
            btnSalvar.MouseLeave += (s, e) => btnSalvar.BackColor = Tema.Verde;

            btnExcluir.Text = "Excluir";
            btnExcluir.Location = new Point(540, 72);
            btnExcluir.Width = 90;
            btnExcluir.Click += BtnExcluir_Click;
            btnExcluir.FlatStyle = FlatStyle.Flat;
            btnExcluir.FlatAppearance.BorderColor = Color.IndianRed;
            btnExcluir.BackColor = Tema.FundoPrincipal;
            btnExcluir.ForeColor = Color.IndianRed;
            btnExcluir.Cursor = Cursors.Hand;

            grid.Location = new Point(0, 120);
            grid.Width = 630;
            grid.Height = 380;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionChanged += Grid_SelectionChanged;
            Tema.EstilizarGrid(grid);

            Controls.Add(lblTitulo);
            Controls.Add(lblNome);
            Controls.Add(txtNome);
            Controls.Add(lblEmail);
            Controls.Add(txtEmail);
            Controls.Add(lblTelefone);
            Controls.Add(txtTelefone);
            Controls.Add(btnNovo);
            Controls.Add(btnSalvar);
            Controls.Add(btnExcluir);
            Controls.Add(grid);
        }

        private void CarregarDados()
        {
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Email, Telefone FROM Clientes ORDER BY Nome";

            var tabela = new DataTable();
            using var reader = cmd.ExecuteReader();
            tabela.Load(reader);
            grid.DataSource = tabela;
        }

        private void Grid_SelectionChanged(object? sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;

            idSelecionado = Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
            txtNome.Text = grid.CurrentRow.Cells["Nome"].Value?.ToString();
            txtEmail.Text = grid.CurrentRow.Cells["Email"].Value?.ToString();
            txtTelefone.Text = grid.CurrentRow.Cells["Telefone"].Value?.ToString();
        }

        private void LimparCampos()
        {
            idSelecionado = null;
            txtNome.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
            grid.ClearSelection();
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do cliente.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();

            if (idSelecionado == null)
            {
                cmd.CommandText = "INSERT INTO Clientes (Nome, Email, Telefone) VALUES ($nome, $email, $telefone)";
            }
            else
            {
                cmd.CommandText = "UPDATE Clientes SET Nome=$nome, Email=$email, Telefone=$telefone WHERE Id=$id";
                cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            }

            cmd.Parameters.AddWithValue("$nome", txtNome.Text.Trim());
            cmd.Parameters.AddWithValue("$email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("$telefone", txtTelefone.Text.Trim());
            cmd.ExecuteNonQuery();

            LimparCampos();
            CarregarDados();
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um cliente na lista.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Deseja realmente excluir este cliente?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Clientes WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            cmd.ExecuteNonQuery();

            LimparCampos();
            CarregarDados();
        }
    }
}