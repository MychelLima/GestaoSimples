using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GestaoSimples.Data;

namespace GestaoSimples.Forms
{
    public partial class UsuariosControl : UserControl
    {
        private DataGridView grid = new DataGridView();
        private TextBox txtUsuario = new TextBox();
        private TextBox txtSenha = new TextBox();
        private ComboBox cboPerfil = new ComboBox();
        private Button btnNovo = new Button();
        private Button btnSalvar = new Button();
        private Button btnExcluir = new Button();
        private int? idSelecionado = null;

        public UsuariosControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            MontarLayout();
            CarregarDados();
        }

        private void MontarLayout()
        {
            BackColor = Tema.FundoPrincipal;

            var lblTitulo = new Label { Text = "Usuários", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Tema.TextoPrimario, AutoSize = true, Location = new Point(0, 0) };

            var lblUsuario = new Label { Text = "Usuário:", ForeColor = Tema.TextoSecundario, Location = new Point(0, 40), AutoSize = true };
            txtUsuario.Location = new Point(70, 37);
            txtUsuario.Width = 160;
            txtUsuario.BackColor = Tema.FundoInput;
            txtUsuario.ForeColor = Tema.TextoPrimario;
            txtUsuario.BorderStyle = BorderStyle.FixedSingle;

            var lblSenha = new Label { Text = "Senha:", ForeColor = Tema.TextoSecundario, Location = new Point(250, 40), AutoSize = true };
            txtSenha.Location = new Point(300, 37);
            txtSenha.Width = 120;
            txtSenha.PasswordChar = '•';
            txtSenha.BackColor = Tema.FundoInput;
            txtSenha.ForeColor = Tema.TextoPrimario;
            txtSenha.BorderStyle = BorderStyle.FixedSingle;

            var lblPerfil = new Label { Text = "Perfil:", ForeColor = Tema.TextoSecundario, Location = new Point(0, 75), AutoSize = true };
            cboPerfil.Location = new Point(70, 72);
            cboPerfil.Width = 160;
            cboPerfil.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPerfil.Items.Add("Administrador");
            cboPerfil.Items.Add("Vendedor");
            cboPerfil.BackColor = Tema.FundoInput;
            cboPerfil.ForeColor = Tema.TextoPrimario;
            cboPerfil.FlatStyle = FlatStyle.Flat;

            btnNovo.Text = "Novo";
            btnNovo.Location = new Point(300, 72);
            btnNovo.Width = 90;
            btnNovo.Click += (s, e) => LimparCampos();
            btnNovo.FlatStyle = FlatStyle.Flat;
            btnNovo.FlatAppearance.BorderColor = Tema.TextoSecundario;
            btnNovo.BackColor = Tema.FundoPrincipal;
            btnNovo.ForeColor = Tema.TextoPrimario;
            btnNovo.Cursor = Cursors.Hand;

            btnSalvar.Text = "Salvar";
            btnSalvar.Location = new Point(400, 72);
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
            btnExcluir.Location = new Point(500, 72);
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
            Controls.Add(lblUsuario);
            Controls.Add(txtUsuario);
            Controls.Add(lblSenha);
            Controls.Add(txtSenha);
            Controls.Add(lblPerfil);
            Controls.Add(cboPerfil);
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
            cmd.CommandText = "SELECT Id, NomeUsuario, Perfil FROM Usuarios ORDER BY NomeUsuario";

            var tabela = new DataTable();
            using var reader = cmd.ExecuteReader();
            tabela.Load(reader);
            grid.DataSource = tabela;
            // Repare que não trazemos a coluna Senha pra listagem — nunca mostramos senha salva
        }

        private void Grid_SelectionChanged(object? sender, EventArgs e)
        {
            if (grid.CurrentRow == null) return;

            idSelecionado = Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value);
            txtUsuario.Text = grid.CurrentRow.Cells["NomeUsuario"].Value?.ToString();
            cboPerfil.SelectedItem = grid.CurrentRow.Cells["Perfil"].Value?.ToString();
            txtSenha.Text = ""; // senha fica em branco ao editar; só muda se digitar algo novo
        }

        private void LimparCampos()
        {
            idSelecionado = null;
            txtUsuario.Clear();
            txtSenha.Clear();
            cboPerfil.SelectedIndex = -1;
            grid.ClearSelection();
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || cboPerfil.SelectedItem == null)
            {
                MessageBox.Show("Preencha usuário e perfil.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (idSelecionado == null && string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                MessageBox.Show("Informe uma senha para o novo usuário.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();

            if (idSelecionado == null)
            {
                cmd.CommandText = "INSERT INTO Usuarios (NomeUsuario, Senha, Perfil) VALUES ($usuario, $senha, $perfil)";
                cmd.Parameters.AddWithValue("$senha", txtSenha.Text.Trim());
            }
            else if (!string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                // Editando e digitou uma senha nova -> atualiza a senha também
                cmd.CommandText = "UPDATE Usuarios SET NomeUsuario=$usuario, Senha=$senha, Perfil=$perfil WHERE Id=$id";
                cmd.Parameters.AddWithValue("$senha", txtSenha.Text.Trim());
                cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            }
            else
            {
                // Editando e deixou a senha em branco -> mantém a senha antiga
                cmd.CommandText = "UPDATE Usuarios SET NomeUsuario=$usuario, Perfil=$perfil WHERE Id=$id";
                cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            }

            cmd.Parameters.AddWithValue("$usuario", txtUsuario.Text.Trim());
            cmd.Parameters.AddWithValue("$perfil", cboPerfil.SelectedItem!.ToString()!);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                MessageBox.Show("Já existe um usuário com esse nome.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LimparCampos();
            CarregarDados();
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um usuário na lista.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Deseja realmente excluir este usuário?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Usuarios WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            cmd.ExecuteNonQuery();

            LimparCampos();
            CarregarDados();
        }
    }
}