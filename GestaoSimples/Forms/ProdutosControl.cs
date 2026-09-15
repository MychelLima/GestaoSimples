using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
using GestaoSimples.Data;

namespace GestaoSimples.Forms
{
    public partial class ProdutosControl : UserControl
    {
        private TextBox txtNome = new TextBox();
        private TextBox txtPreco = new TextBox();
        private TextBox txtEstoque = new TextBox();
        private PictureBox picPreview = new PictureBox();
        private Button btnEscolherImagem = new Button();
        private Button btnNovo = new Button();
        private Button btnSalvar = new Button();
        private Button btnExcluir = new Button();
        private FlowLayoutPanel flowCards = new FlowLayoutPanel();

        private int? idSelecionado = null;
        private string? caminhoImagemSelecionada = null;

        private static readonly string PastaImagens =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

        public ProdutosControl()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            Directory.CreateDirectory(PastaImagens);
            MontarLayout();
            CarregarCards();
        }

        private void MontarLayout()
        {
            BackColor = Tema.FundoPrincipal;

            var lblTitulo = new Label { Text = "Produtos", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Tema.TextoPrimario, AutoSize = true, Location = new Point(0, 0) };

            var lblNome = new Label { Text = "Nome:", ForeColor = Tema.TextoSecundario, Location = new Point(0, 40), AutoSize = true };
            txtNome.Location = new Point(70, 37);
            txtNome.Width = 150;
            txtNome.BackColor = Tema.FundoInput;
            txtNome.ForeColor = Tema.TextoPrimario;
            txtNome.BorderStyle = BorderStyle.FixedSingle;

            var lblPreco = new Label { Text = "Preço:", ForeColor = Tema.TextoSecundario, Location = new Point(230, 40), AutoSize = true };
            txtPreco.Location = new Point(280, 37);
            txtPreco.Width = 80;
            txtPreco.BackColor = Tema.FundoInput;
            txtPreco.ForeColor = Tema.TextoPrimario;
            txtPreco.BorderStyle = BorderStyle.FixedSingle;

            var lblEstoque = new Label { Text = "Estoque:", ForeColor = Tema.TextoSecundario, Location = new Point(370, 40), AutoSize = true };
            txtEstoque.Location = new Point(440, 37);
            txtEstoque.Width = 60;
            txtEstoque.BackColor = Tema.FundoInput;
            txtEstoque.ForeColor = Tema.TextoPrimario;
            txtEstoque.BorderStyle = BorderStyle.FixedSingle;

            picPreview.Location = new Point(0, 72);
            picPreview.Size = new Size(60, 60);
            picPreview.BorderStyle = BorderStyle.FixedSingle;
            picPreview.SizeMode = PictureBoxSizeMode.Zoom;
            picPreview.BackColor = Tema.FundoInput;

            btnEscolherImagem.Text = "Escolher imagem";
            btnEscolherImagem.Location = new Point(70, 92);
            btnEscolherImagem.Width = 120;
            btnEscolherImagem.Click += BtnEscolherImagem_Click;
            btnEscolherImagem.FlatStyle = FlatStyle.Flat;
            btnEscolherImagem.FlatAppearance.BorderColor = Tema.TextoSecundario;
            btnEscolherImagem.BackColor = Tema.FundoPrincipal;
            btnEscolherImagem.ForeColor = Tema.TextoPrimario;
            btnEscolherImagem.Cursor = Cursors.Hand;

            btnNovo.Text = "Novo";
            btnNovo.Location = new Point(370, 72);
            btnNovo.Width = 90;
            btnNovo.Click += (s, e) => LimparCampos();
            btnNovo.FlatStyle = FlatStyle.Flat;
            btnNovo.FlatAppearance.BorderColor = Tema.TextoSecundario;
            btnNovo.BackColor = Tema.FundoPrincipal;
            btnNovo.ForeColor = Tema.TextoPrimario;
            btnNovo.Cursor = Cursors.Hand;

            btnSalvar.Text = "Salvar";
            btnSalvar.Location = new Point(470, 72);
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
            btnExcluir.Location = new Point(570, 72);
            btnExcluir.Width = 90;
            btnExcluir.Click += BtnExcluir_Click;
            btnExcluir.FlatStyle = FlatStyle.Flat;
            btnExcluir.FlatAppearance.BorderColor = Color.IndianRed;
            btnExcluir.BackColor = Tema.FundoPrincipal;
            btnExcluir.ForeColor = Color.IndianRed;
            btnExcluir.Cursor = Cursors.Hand;

            flowCards.Location = new Point(0, 150);
            flowCards.Width = 660;
            flowCards.Height = 380;
            flowCards.AutoScroll = true;
            flowCards.FlowDirection = FlowDirection.LeftToRight;
            flowCards.WrapContents = true;
            flowCards.BackColor = Tema.FundoPrincipal;

            Controls.Add(lblTitulo);
            Controls.Add(lblNome);
            Controls.Add(txtNome);
            Controls.Add(lblPreco);
            Controls.Add(txtPreco);
            Controls.Add(lblEstoque);
            Controls.Add(txtEstoque);
            Controls.Add(picPreview);
            Controls.Add(btnEscolherImagem);
            Controls.Add(btnNovo);
            Controls.Add(btnSalvar);
            Controls.Add(btnExcluir);
            Controls.Add(flowCards);
        }

        private void BtnEscolherImagem_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.Filter = "Imagens (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                caminhoImagemSelecionada = dialog.FileName;
                picPreview.Image = Image.FromFile(caminhoImagemSelecionada);
            }
        }

        private void CarregarCards()
        {
            flowCards.Controls.Clear();

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Id, Nome, Preco, Estoque, CaminhoImagem FROM Produtos ORDER BY Nome";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nome = reader.GetString(1);
                double preco = reader.GetDouble(2);
                int estoque = reader.GetInt32(3);
                string? caminhoImagem = reader.IsDBNull(4) ? null : reader.GetString(4);

                flowCards.Controls.Add(CriarCard(id, nome, preco, estoque, caminhoImagem));
            }
        }

        private Panel CriarCard(int id, string nome, double preco, int estoque, string? caminhoImagem)
        {
            var card = new Panel
            {
                Width = 150,
                Height = 160,
                BackColor = Tema.FundoCard,
                Margin = new Padding(8),
                Tag = id,
                Cursor = Cursors.Hand
            };

            var pic = new PictureBox
            {
                Location = new Point(5, 5),
                Size = new Size(138, 80),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Tema.FundoInput
            };
            if (caminhoImagem != null && File.Exists(caminhoImagem))
            {
                pic.Image = Image.FromFile(caminhoImagem);
            }

            var lblNome = new Label { Text = nome, ForeColor = Tema.TextoPrimario, Location = new Point(5, 90), Width = 138, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            var lblPreco = new Label { Text = $"R$ {preco:F2}", ForeColor = Tema.Verde, Location = new Point(5, 110), Width = 138, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            var lblEstoque = new Label
            {
                Text = $"{estoque} un.",
                Location = new Point(5, 128),
                Width = 138,
                ForeColor = estoque <= 5 ? Color.IndianRed : Tema.TextoSecundario
            };

            card.Controls.Add(pic);
            card.Controls.Add(lblNome);
            card.Controls.Add(lblPreco);
            card.Controls.Add(lblEstoque);

            EventHandler clicarCard = (s, e) => SelecionarProduto(id);
            card.Click += clicarCard;
            pic.Click += clicarCard;
            lblNome.Click += clicarCard;
            lblPreco.Click += clicarCard;
            lblEstoque.Click += clicarCard;

            return card;
        }

        private void SelecionarProduto(int id)
        {
            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Nome, Preco, Estoque, CaminhoImagem FROM Produtos WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                idSelecionado = id;
                txtNome.Text = reader.GetString(0);
                txtPreco.Text = reader.GetDouble(1).ToString();
                txtEstoque.Text = reader.GetInt32(2).ToString();
                caminhoImagemSelecionada = reader.IsDBNull(3) ? null : reader.GetString(3);

                picPreview.Image = (caminhoImagemSelecionada != null && File.Exists(caminhoImagemSelecionada))
                    ? Image.FromFile(caminhoImagemSelecionada)
                    : null;
            }
        }

        private void LimparCampos()
        {
            idSelecionado = null;
            txtNome.Clear();
            txtPreco.Clear();
            txtEstoque.Clear();
            caminhoImagemSelecionada = null;
            picPreview.Image = null;
        }

        private void BtnSalvar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Informe o nome do produto.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtPreco.Text, out double preco))
            {
                MessageBox.Show("Informe um preço válido (ex: 99.90).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtEstoque.Text, out int estoque))
            {
                MessageBox.Show("Informe um estoque válido (número inteiro).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string? caminhoFinal = caminhoImagemSelecionada;
            if (caminhoFinal != null && !caminhoFinal.StartsWith(PastaImagens))
            {
                string novoNome = $"{Guid.NewGuid()}{Path.GetExtension(caminhoFinal)}";
                string destino = Path.Combine(PastaImagens, novoNome);
                File.Copy(caminhoFinal, destino, true);
                caminhoFinal = destino;
            }

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();

            if (idSelecionado == null)
            {
                cmd.CommandText = "INSERT INTO Produtos (Nome, Preco, Estoque, CaminhoImagem) VALUES ($nome, $preco, $estoque, $imagem)";
            }
            else
            {
                cmd.CommandText = "UPDATE Produtos SET Nome=$nome, Preco=$preco, Estoque=$estoque, CaminhoImagem=$imagem WHERE Id=$id";
                cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            }

            cmd.Parameters.AddWithValue("$nome", txtNome.Text.Trim());
            cmd.Parameters.AddWithValue("$preco", preco);
            cmd.Parameters.AddWithValue("$estoque", estoque);
            cmd.Parameters.AddWithValue("$imagem", (object?)caminhoFinal ?? DBNull.Value);
            cmd.ExecuteNonQuery();

            LimparCampos();
            CarregarCards();
        }

        private void BtnExcluir_Click(object? sender, EventArgs e)
        {
            if (idSelecionado == null)
            {
                MessageBox.Show("Selecione um produto na lista.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Deseja realmente excluir este produto?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using var conn = new SqliteConnection(Database.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM Produtos WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", idSelecionado.Value);
            cmd.ExecuteNonQuery();

            LimparCampos();
            CarregarCards();
        }
    }
}
