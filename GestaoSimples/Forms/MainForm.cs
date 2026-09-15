using System;
using System.Drawing;
using System.Windows.Forms;
using GestaoSimples.Data;

namespace GestaoSimples.Forms
{
    public partial class MainForm : Form
    {
        private readonly string _usuario;
        private readonly string _perfil;

        private Panel pnlMenu = new Panel();
        private Panel pnlConteudo = new Panel();
        private Label lblUsuarioLogado = new Label();

        private Button btnClientes = new Button();
        private Button btnProdutos = new Button();
        private Button btnPedidos = new Button();
        private Button btnUsuarios = new Button();

        public MainForm(string usuario, string perfil)
        {
            InitializeComponent();
            _usuario = usuario;
            _perfil = perfil;
            MontarTela();
            AbrirAba(btnClientes, "Clientes");
        }

        private void MontarTela()
        {
            Text = "Sistema de Gestão";
            Width = 900;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Tema.FundoPrincipal;

            // Menu lateral
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Width = 180;
            pnlMenu.BackColor = Tema.FundoMenu;

            lblUsuarioLogado.Text = $"Olá, {_usuario}\n({_perfil})";
            lblUsuarioLogado.Location = new Point(15, 15);
            lblUsuarioLogado.AutoSize = true;
            lblUsuarioLogado.ForeColor = Tema.TextoPrimario;

            ConfigurarBotaoMenu(btnClientes, "Clientes", 70);
            ConfigurarBotaoMenu(btnProdutos, "Produtos", 115);
            ConfigurarBotaoMenu(btnPedidos, "Pedidos", 160);
            ConfigurarBotaoMenu(btnUsuarios, "Usuários", 205);

            // Regra de acesso: só Administrador vê a aba Usuários
            btnUsuarios.Visible = (_perfil == "Administrador");

            btnClientes.Click += (s, e) => AbrirAba(btnClientes, "Clientes");
            btnProdutos.Click += (s, e) => AbrirAba(btnProdutos, "Produtos");
            btnPedidos.Click += (s, e) => AbrirAba(btnPedidos, "Pedidos");
            btnUsuarios.Click += (s, e) => AbrirAba(btnUsuarios, "Usuários");

            pnlMenu.Controls.Add(lblUsuarioLogado);
            pnlMenu.Controls.Add(btnClientes);
            pnlMenu.Controls.Add(btnProdutos);
            pnlMenu.Controls.Add(btnPedidos);
            pnlMenu.Controls.Add(btnUsuarios);

            // Área de conteúdo (onde cada aba vai aparecer)
            pnlConteudo.Dock = DockStyle.Fill;
            pnlConteudo.Padding = new Padding(20);
            pnlConteudo.BackColor = Tema.FundoPrincipal;

            Controls.Add(pnlConteudo);
            Controls.Add(pnlMenu);
        }

        private void ConfigurarBotaoMenu(Button btn, string texto, int top)
        {
            btn.Text = "   " + texto;
            btn.Location = new Point(15, top);
            btn.Width = 150;
            btn.Height = 38;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Font = new Font("Segoe UI", 9.5f);
            btn.Cursor = Cursors.Hand;
            btn.ForeColor = Tema.TextoSecundario;
            btn.BackColor = Tema.FundoMenu;
        }

        private void AbrirAba(Button botaoClicado, string nomeAba)
        {
            // Reseta o visual de todos os botões do menu
            foreach (var btn in new[] { btnClientes, btnProdutos, btnPedidos, btnUsuarios })
            {
                btn.BackColor = Tema.FundoMenu;
                btn.ForeColor = Tema.TextoSecundario;
                btn.Font = new Font("Segoe UI", 9.5f);
            }

            // Destaca o botão da aba atual
            botaoClicado.BackColor = Tema.FundoSelecionado;
            botaoClicado.ForeColor = Tema.TextoPrimario;
            botaoClicado.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);

            pnlConteudo.Controls.Clear();

            if (nomeAba == "Clientes")
            {
                pnlConteudo.Controls.Add(new ClientesControl());
            }
            else if (nomeAba == "Produtos")
            {
                pnlConteudo.Controls.Add(new ProdutosControl());
            }
            else if (nomeAba == "Pedidos")
            {
                pnlConteudo.Controls.Add(new PedidosControl());
            }
            else if (nomeAba == "Usuários")
            {
                pnlConteudo.Controls.Add(new UsuariosControl());
            }
        }
    }
}