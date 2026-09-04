# LabUseCase01 - Desenvolvimento ASP.NET Core .NET 8 (Database First)

Seja bem-vindo ao **LabUseCase01**! Neste laboratório prático, você trabalhará com um projeto ASP.NET Core .NET 8 MVC pré-configurado utilizando a abordagem **Database First** do Entity Framework Core.

O projeto já possui a estrutura base pronta, a conexão com o SQL Server configurada no `appsettings.json` e o **CRUD de Tarefas** 100% funcional. Sua missão será testar o sistema atual, criar o **CRUD de Funcionários** e implementar o módulo de **Incidentes**.

---

## 📋 Modelo de Dados Inicial

O banco de dados do sistema é o `dbTasks`. Ele possui duas tabelas relacionadas (**1 Funcionário : N Tarefas**):

### Tabela `Funcionario`
* **`Codigo`**: `INT` (Primary Key, Identity)
* **`Nome`**: `VARCHAR(100)` (Not Null)
* **`Cargo`**: `VARCHAR(50)` (Not Null)

### Tabela `Tarefa`
* **`Codigo`**: `INT` (Primary Key, Identity)
* **`Descricao`**: `VARCHAR(200)` (Not Null)
* **`DataPlanejada`**: `DATETIME` (Not Null)
* **`DataIniciada`**: `DATETIME` (Nullable)
* **`DataFinalizada`**: `DATETIME` (Nullable)
* **`DataCancelada`**: `DATETIME` (Nullable)
* **`StatusTarefa`**: `VARCHAR(30)` (Not Null)
* **`Prazo`**: `VARCHAR(20)` (Not Null)
* **`CodigoFuncionario`**: `INT` (Foreign Key -> `Funcionario.Codigo`)

---

## 🚀 Passo 1: Preparação do Banco de Dados Inicial

1. Abra o **SQL Server Management Studio (SSMS)** ou o **Azure Data Studio**.
2. Conecte-se à sua instância local do SQL Server.
3. Execute o script SQL abaixo para criar o banco de dados `dbTasks` e as tabelas iniciais:

```sql
CREATE DATABASE dbTasks;
GO

USE dbTasks;
GO

-- Tabela Funcionario
CREATE TABLE Funcionario (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Nome VARCHAR(100) NOT NULL,
    Cargo VARCHAR(50) NOT NULL
);
GO

-- Tabela Tarefa
CREATE TABLE Tarefa (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Descricao VARCHAR(200) NOT NULL,
    DataPlanejada DATETIME NOT NULL,
    DataIniciada DATETIME NULL,
    DataFinalizada DATETIME NULL,
    DataCancelada DATETIME NULL,
    StatusTarefa VARCHAR(30) NOT NULL,
    Prazo VARCHAR(20) NOT NULL,
    CodigoFuncionario INT NOT NULL,
    CONSTRAINT FK_Tarefa_Funcionario FOREIGN KEY (CodigoFuncionario) 
        REFERENCES Funcionario(Codigo)
);
GO

-- Dados Iniciais para Teste
INSERT INTO Funcionario (Nome, Cargo) VALUES 
('Carlos Silva', 'Desenvolvedor Senior'),
('Ana Oliveira', 'Analista de QA'),
('Roberto Santos', 'Gerente de Projetos');

INSERT INTO Tarefa (Descricao, DataPlanejada, DataIniciada, DataFinalizada, DataCancelada, StatusTarefa, Prazo, CodigoFuncionario) VALUES 
('Criar tela de Login', '2026-08-10', '2026-08-01', NULL, NULL, 'Em Andamento', 'Em dia', 1),
('Homologar Release 1.0', '2026-08-05', NULL, NULL, NULL, 'Pendente', 'Em atraso', 2);
GO
```

---

## 🛠️ Passo 2: Verificação da Conexão e Teste do CRUD de Tarefa

1. Abra o arquivo `appsettings.json` no projeto e ajuste a `ConnectionString` conforme as credenciais do seu banco SQL Server:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "ConexaoSqlServer": "Server=LOCALHOST;Database=dbTasks;User Id=sa;Password=SUA_SENHA_AQUI;TrustServerCertificate=True;"
  }
}
```

2. Execute o projeto pressionando `F5` ou clicando em **Run/Start** no Visual Studio.
3. Acesse a rota `/Tarefa` e teste as operações CRUD completas (Criar, Listar, Editar, Detalhar e Excluir) que já vêm nativas no projeto.

---

## 🎯 Passo 3: Criando o CRUD de Funcionário via Scaffold

Sua primeira missão prática será gerar o CRUD para a entidade `Funcionario` utilizando as ferramentas automáticas do Visual Studio (Scaffold):

1. Na janela do **Solution Explorer**, clique com o botão direito sobre a pasta **Controllers**.
2. Selecione **Adicionar** > **Novo Item Scaffolded...** (ou *New Scaffolded Item...*).
3. Escolha a opção **Controlador MVC com exibições, usando o Entity Framework** (*MVC Controller with views, using Entity Framework*).
4. Preencha a caixa de diálogo com as seguintes opções:
   * **Classe de Modelo (Model class):** `Funcionario (AppTask.Models)`
   * **Classe do contexto de dados (Data context class):** `DbTasksContext (AppTask.Models)`
   * **Nome do Controlador:** `FuncionarioController`
5. Clique em **Adicionar**.
6. Abra o arquivo `Views/Shared/_Layout.cshtml` e adicione um link para acessar a Controller no menu de navegação:

```html
<li class="nav-item">
    <a class="nav-link text-dark" asp-area="" asp-controller="Funcionario" asp-action="Index">Funcionários</a>
</li>
```

---

## ⚡ Passo 4: Adicionando a Tabela e Módulo de Incidentes

Agora você irá expandir o banco de dados e atualizar o mapeamento no projeto.

### 4.1. Executar o Script SQL no Banco

Execute o script abaixo no SQL Server para criar a nova tabela `Incidente`:

```sql
USE dbTasks;
GO

CREATE TABLE Incidente (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    DescricaoProblema VARCHAR(250) NOT NULL,
    DataIncidente DATETIME NOT NULL,
    Solucao VARCHAR(250) NULL,
    Resolvido VARCHAR(3) NOT NULL -- 'sim' ou 'nao'
);
GO
```

### 4.2. Atualizar o Mapeamento via Scaffold (CLI)

Para atualizar o seu `DbTasksContext` e gerar a Model `Incidente` automaticamente sem perder suas configurações, abra o **Package Manager Console** (no Visual Studio em *Ferramentas > Gerenciador de Pacotes NuGet > Console do Gerenciador de Pacotes*) ou o **Terminal** na raiz do projeto e execute:

**Pelo Package Manager Console:**
```powershell
Scaffold-DbContext "Server=LOCALHOST;Database=dbTasks;User Id=sa;Password=SUA_SENHA_AQUI;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables Incidente -Force
```

**Pelo Terminal / CLI (.NET Core):**
```bash
dotnet ef dbcontext scaffold "Server=LOCALHOST;Database=dbTasks;User Id=sa;Password=SUA_SENHA_AQUI;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --table Incidente --force
```

> **Nota:** Certifique-se de substituir `SUA_SENHA_AQUI` pela senha configurada no seu SQL Server.

### 4.3. Criar o CRUD de Incidente via Scaffold

Com a nova Model `Incidente.cs` criada na pasta `Models`:

1. Clique com o botão direito na pasta **Controllers** > **Adicionar** > **Novo Item Scaffolded...**
2. Selecione **Controlador MVC com exibições, usando o Entity Framework**.
3. Defina as opções:
   * **Classe de Modelo:** `Incidente (AppTask.Models)`
   * **Classe do contexto de dados:** `DbTasksContext (AppTask.Models)`
   * **Nome do Controlador:** `IncidenteController`
4. Clique em **Adicionar**.

🎉 **Teste o funcionamento completo do novo CRUD executando o projeto e navegando até `/Incidente`!**
