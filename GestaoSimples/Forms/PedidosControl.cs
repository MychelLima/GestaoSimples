using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GestaoSimples.Data;


namespace GestaoSimples.Forms
{
    public partial class PedidosControl : UserControl
    {
        private ComboBox cboCliente = new ComboBox();
        private ComboBox cboProduto = new ComboBox();
        private TextBox txtQuantidade = new TextBox();
        private Label lblTotalValor = new Label();
        private Button btnAdicionar = new Button();
        private Button btnExcluir = new Button();
        private DataGridView grid = new DataGridView();

        private int? idSelecionado = null;

        public PedidosControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            MontarLayout();
            CarregarCombos();
            CarregarDados();
        }

        private void MontarLayout()
        {
            BackColor = Tema.FundoPrincipal;

            var lblTitulo = new Label { Text = "Pedidos", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Tema.TextoPrimario, AutoSize = true, Location = new Point(0, 0) };

            var lblCliente = new Label { Text = "Cliente:", ForeColor = Tema.TextoSecundario, Location = new Point(0, 40), AutoSize = true };
            cboCliente.Location = new Point(70, 37);
            cboCliente.Width = 180;
            cboCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCliente.BackColor = Tema.FundoInput;
            cboCliente.ForeColor = Tema.TextoPrimario;
            cboCliente.FlatStyle = FlatStyle.Flat;

            var lblProduto = new Label { Text = "Produto:", ForeColor = Tema.TextoSecundario, Location = new Point(270, 40), AutoSize = true };
            cboProduto.Location = new Point(335, 37);
            cboProduto.Width = 180;
            cboProduto.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProduto.SelectedIndexChanged += (s, e) => AtualizarTotal();
            cboProduto.BackColor = Tema.FundoInput;
            cboProduto.ForeColor = Tema.TextoPrimario;
            cboProduto.FlatStyle = FlatStyle.Flat;

            var lblQtd = new Label { Text = "Qtd:", ForeColor = Tema.TextoSecundario, Location = new Point(0, 75), AutoSize = true };
            txtQuantidade.Location = new Point(70, 72);
            txtQuantidade.Width = 60;
            txtQuantidade.TextChanged += (s, e) => AtualizarTotal();
            txtQuantidade.BackColor = Tema.FundoInput;
            txtQuantidade.ForeColor = Tema.TextoPrimario;
            txtQuantidade.BorderStyle = BorderStyle.FixedSingle;

            var lblTotal = new Label { Text = "Total:", ForeColor = Tema.TextoSecundario, Location = new Point(150, 75), AutoSize = true };
            lblTotalValor.Text = "R$ 0,00";
            lblTotalValor.Location = new Point(200, 75);
            lblTotalValor.AutoSize = true;
            lblTotalValor.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTotalValor.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTotalValor.ForeColor = Tema.Verde;

            btnAdicionar.Text = "Adicionar pedido";
            btnAdicionar.Location = new Point(335, 72);
            btnAdicionar.Width = 130;
            btnAdicionar.Click += BtnAdicionar_Click;
            btnAdicionar.FlatStyle = FlatStyle.Flat;
            btnAdicionar.FlatAppearance.BorderSize = 0;
            btnAdicionar.BackColor = Tema.Verde;
            btnAdicionar.ForeColor = Color.Black;
            btnAdicionar.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnAdicionar.Cursor = Cursors.Hand;
            btnAdicionar.MouseEnter += (s, e) => btnAdicionar.BackColor = Tema.VerdeHover;
            btnAdicionar.MouseLeave += (s, e) => btnAdicionar.BackColor = Tema.Verde;

            btnExcluir.Text = "Excluir selecionado";
            btnExcluir.Location = new Point(475, 72);
            btnExcluir.Width = 140;
            btnExcluir.Click += BtnExcluir_Click;
            btnExcluir.FlatStyle = FlatStyle.Flat;
            btnExcluir.FlatAppearance.BorderColor = Color.IndianRed;
            btnExcluir.BackColor = Tema.FundoPrincipal;
            btnExcluir.ForeColor = Color.IndianRed;
            btnExcluir.Cursor = Cursors.Hand;

            grid.Location = new Point(0, 120);
            grid.Width = 650;
            grid.Height = 380;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.SelectionChanged += (s, e) =>
            {
                idSelecionado = grid.CurrentRow != null
                    ? Convert.ToInt32(grid.CurrentRow.Cells["Id"].Value)
                    : null;
            };
            Tema.EstilizarGrid(grid);

            Controls.Add(lblTitulo);
            Controls.Add(lblCliente);
            Controls.Add(cboCliente);
            Controls.Add(lblProduto);
            Controls.Add(cboProduto);
            Controls.Add(lblQtd);
            Controls.Add(txtQuantidade);
            Controls.Add(lblTotal);
            Controls.Add(lblTotalValor);
            Controls.Add(btnAdicionar);
            Controls.Add(btnExcluir);
            Controls.Add(grid);
        }

        private void CarregarCombos()
        {
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();

            var tabelaClientes = new DataTable();
            var cmdClientes = conn.CreateCommand();
            cmdClientes.CommandText = "SELECT Id, Nome FROM Clientes ORDER BY Nome";
            using (var reader = cmdClientes.ExecuteReader()) tabelaClientes.Load(reader);
            cboCliente.DataSource = tabelaClientes;
            cboCliente.DisplayMember = "Nome";
            cboCliente.ValueMember = "Id";

            var tabelaProdutos = new DataTable();
            var cmdProdutos = conn.CreateCommand();
            cmdProdutos.CommandText = "SELECT Id, Nome, Preco FROM Produtos ORDER BY Nome";
            using (var reader = cmdProdutos.ExecuteReader()) tabelaProdutos.Load(reader);
            cboProduto.DataSource = tabelaProdutos;
            cboProduto.DisplayMember = "Nome";
            cboProduto.ValueMember = "Id";
        }

        private void AtualizarTotal()
        {
            if (cboProduto.SelectedItem is DataRowView linha && int.TryParse(txtQuantidade.Text, out int qtd))
            {
                double preco = Convert.ToDouble(linha["Preco"]);
                lblTotalValor.Text = $"R$ {(preco * qtd):F2}";
            }
            else
            {
                lblTotalValor.Text = "R$ 0,00";
            }
        }

        private void CarregarDados()
        {
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT p.Id, c.Nome AS Cliente, pr.Nome AS Produto, p.Quantidade, p.Total, p.Data
                FROM Pedidos p
                JOIN Clientes c ON p.ClienteId = c.Id
                JOIN Produtos pr ON p.ProdutoId = pr.Id
                ORDER BY p.Data DESC";

            var tabela = new DataTable();
            using var reader = cmd.ExecuteReader();
            tabela.Load(reader);
            grid.DataSource = tabela;
        }

        private void BtnAdicionar_Click(object? sender, EventArgs e)
        {
            if (cboCliente.SelectedValue == null || cboProduto.SelectedValue == null)
            {
                MessageBox.Show("Selecione um cliente e um produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Informe uma quantidade válida.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var linhaProduto = (DataRowView)cboProduto.SelectedItem;
            double preco = Convert.ToDouble(linhaProduto["Preco"]);
            double total = preco * quantidade;

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Pedidos (ClienteId, ProdutoId, Quantidade, Total, Data)
                VALUES ($clienteId, $produtoId, $quantidade, $total, $data)";
            cmd.Parameters.AddWithValue("$clienteId", Convert.ToInt32(cboCliente.SelectedValue));
            cmd.Parameters.AddWithValue("$produtoId", Convert.ToInt32(cboProduto.SelectedValue));
            cmd.Parameters.AddWithValue("$quantidade", quantidade);
            cmd.Parameters.AddWithValue("$total", total);
            cmd.Parameters.AddWithValue("$data", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            cmd.ExecuteNonQuery();

            txtQuantidade.Clear();
            lblTotalValor.Text = "R$ 0,00";
            CarregarDados();
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um pedido na lista.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Deseja realmente excluir este pedido?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Pedidos WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            cmd.ExecuteNonQuery();

            idSelecionado = null;
            CarregarDados();
        }
    }
}
