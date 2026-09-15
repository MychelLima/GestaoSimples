# Sistema de Gestão Comercial

Sistema desktop desenvolvido em C# (WinForms) para gerenciamento de clientes, produtos, pedidos e usuários de uma pequena empresa comercial. Projeto criado com fins de aprendizado, cobrindo autenticação, CRUD completo, relacionamento entre tabelas e upload de imagens.

## Funcionalidades

- **Login** com autenticação de usuário e senha
- **Controle de acesso por perfil** — a aba "Usuários" só é visível para administradores
- **Clientes** — cadastro, listagem, edição e exclusão
- **Produtos** — cadastro com upload de imagem, exibidos em cards; controle de estoque com destaque visual para itens em baixa quantidade
- **Pedidos** — vinculação de cliente + produto, cálculo automático do total
- **Usuários** — gerenciamento de contas e perfis de acesso (Administrador / Vendedor)

## Tecnologias

- **C# / .NET 8**
- **Windows Forms** (interface desktop)
- **SQLite** via `Microsoft.Data.Sqlite` (banco de dados local em arquivo)

## Estrutura do projeto

```
GestaoSimples/
├── Data/
│   ├── Database.cs      # Criação e conexão com o banco SQLite
│   └── Tema.cs           # Cores e estilos visuais reutilizáveis
├── Models/
│   ├── Cliente.cs
│   ├── Produto.cs
│   ├── Pedido.cs
│   └── Usuario.cs
├── Forms/
│   ├── LoginForm.cs
│   ├── MainForm.cs       # Tela principal com menu lateral
│   ├── ClientesControl.cs
│   ├── ProdutosControl.cs
│   ├── PedidosControl.cs
│   └── UsuariosControl.cs
└── Program.cs
```

## Como executar

### Pré-requisitos

- [Visual Studio 2022 ou superior](https://visualstudio.microsoft.com/) com a carga de trabalho **Desenvolvimento para desktop com .NET**
- .NET 8.0 SDK (geralmente já incluso no Visual Studio)

### Passos

1. Clone o repositório:
   ```
   git clone https://github.com/SEU-USUARIO/GestaoSimples.git
   ```
2. Abra o arquivo `GestaoSimples.sln` no Visual Studio
3. Aguarde a restauração automática dos pacotes NuGet (ou clique com o botão direito na Solução → **Restaurar Pacotes NuGet**)
4. Pressione **F5** para compilar e executar

Na primeira execução, o banco de dados (`gestao.db`) e a pasta de imagens são criados automaticamente na pasta do executável.

### Login de teste

Um usuário administrador é criado automaticamente na primeira execução:

| Usuário | Senha  |
|---------|--------|
| admin   | 123456 |

> ⚠️ Este projeto tem fins educacionais. A senha é armazenada em texto puro no banco — em um sistema de produção, ela deveria ser armazenada com hash (ex: BCrypt).

## Status do projeto

Projeto em desenvolvimento como estudo de C# / WinForms / SQLite. Próximas melhorias possíveis: busca e filtros nas listagens, relatórios, senha com hash.
