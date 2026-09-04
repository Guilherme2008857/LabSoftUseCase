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
    FuncionarioId INT NOT NULL,
    CONSTRAINT FK_Tarefa_Funcionario FOREIGN KEY (FuncionarioId) 
        REFERENCES Funcionario(Codigo)
);
GO

-- Inserindo Dados Iniciais para Teste
INSERT INTO Funcionario (Nome, Cargo) VALUES 
('Carlos Silva', 'Desenvolvedor Senior'),
('Ana Oliveira', 'Analista de QA'),
('Roberto Santos', 'Gerente de Projetos');

INSERT INTO Tarefa (Descricao, DataPlanejada, DataIniciada, DataFinalizada, DataCancelada, StatusTarefa, Prazo, FuncionarioId) VALUES 
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

Use esse comando para adicionar novas tabelas, se seu projeto já contém a classe context configurada 
```powershell
Scaffold-DbContext "Name=ConexaoSqlServer" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
```

Use somente esse se deseja recriar toda estrutura ou quando um projeto novo.
```powershell
Scaffold-DbContext "Server=LOCALHOST;Database=dbTasks;User Id=sa;Password=SUA_SENHA_AQUI;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Tables Incidente -Force
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


---

## 🛠️ Revisão Dev A: Banco de Dados & SQL

Para trabalhar com a abordagem **Database First**, é essencial dominar os comandos de manipulação e as estruturas de dados no SQL Server. Abaixo estão os conceitos e comandos fundamentais utilizados neste laboratório:

| Conceito / Sintaxe | Tipo | O que faz / Explicação | Exemplo de Uso |
| :--- | :--- | :--- | :--- |
| **`INT`** | Tipo de Dado | Armazena números inteiros (positivos ou negativos) sem casas decimais. | `Codigo INT` |
| **`VARCHAR(N)`** | Tipo de Dado | Armazena texto/caracteres de tamanho variável até o limite $N$ especificado. | `Nome VARCHAR(100)` |
| **`PRIMARY KEY`** | Restrição | Identificador único da tabela. Garante que não existam registros duplicados e que o valor nunca seja nulo. | `Codigo INT PRIMARY KEY` |
| **`FOREIGN KEY`** | Restrição | Cria um relacionamento entre duas tabelas, garantindo a integridade referencial com a chave primária de outra tabela. | `FOREIGN KEY (CodigoFuncionario) REFERENCES Funcionario(Codigo)` |
| **`SELECT`** | Comando DML | Consulta e recupera dados de uma ou mais tabelas do banco de dados. | `SELECT * FROM Funcionario;` |
| **`INSERT`** | Comando DML | Insere novos registros/linhas em uma tabela. | `INSERT INTO Funcionario (Nome, Cargo) VALUES ('Ana', 'QA');` |

---

## 🏗️ Revisão Dev B: Arquitetura ASP.NET Core & Entity Framework

Para que a conexão e o gerenciamento de dados funcionem corretamente em uma aplicação ASP.NET Core MVC com Entity Framework Core, é necessário compreender os seguintes componentes e conceitos centrais:

| Componente / Conceito | Descrição e Papel na Aplicação |
| :--- | :--- |
| **`appsettings.json`** | Arquivo de configuração global da aplicação. É nele que armazenamos a **ConnectionString** (string de conexão), contendo o servidor, nome do banco, usuário e senha para conectar ao SQL Server. |
| **`DbContext`** | Classe do Entity Framework que representa uma sessão com o banco de dados. Ela mapeia as tabelas em coleções `DbSet<T>` e traduz operações C# em comandos SQL equivalentes no banco. |
| **CRUD** | Acrônimo para as 4 operações básicas de armazenamento persistente: <br>• **C**reate (Criar/Inserir - `HTTP POST`) <br>• **R**ead (Ler/Consultar - `HTTP GET`) <br>• **U**pdate (Atualizar/Editar - `HTTP POST/PUT`) <br>• **D**elete (Deletar/Excluir - `HTTP POST/DELETE`) |

---

## 💻 Revisão Dev C: Conceitos de Orientação a Objetos (C#)

As classes geradas pelo Entity Framework baseiam-se nos pilares da Orientação a Objetos. Veja como esses conceitos se aplicam ao código C# do nosso projeto:

| Conceito | Explicação | Exemplo do Código (`Tarefa.cs` / `DbContext`) |
| :--- | :--- | :--- |
| **Classe** | Estrutura/molde que define as características e comportamentos de um objeto no sistema. | `public class Tarefa { ... }` |
| **Atributos (Propriedades)** | Características ou dados que a classe armazena (no EF, representam as colunas da tabela). | `public string Descricao { get; set; }` |
| **Método** | Bloco de código dentro de uma classe que executa uma ação ou comportamento específico. | `public async Task<IActionResult> Index() { ... }` |
| **Parâmetros** | Valores/informações de entrada passados para um método executar sua lógica. | `public IActionResult Details(int? id)` *(onde `id` é o parâmetro)* |
| **Return** | Instrução que encerra a execução de um método e devolve um resultado para quem o chamou. | `return View(tarefa);` |

---

## 💼 Você na Entrevista de Emprego

Testes práticos e simulações de entrevistas técnicas frequentemente abordam a integração entre SQL, C# e ASP.NET Core. Responda às 10 questões abaixo para avaliar o seu domínio sobre o conteúdo:

---

### 🗄️ Questões de Banco de Dados & SQL

#### ❓ Questão 1: Tipos de Dados e Restrições
**Cenário:** Durante a modelagem do banco de dados `dbTasks`, você precisa definir uma coluna para armazenar o código identificador principal da tabela `Incidente`. Esse código deve ser gerado automaticamente pelo SQL Server a cada novo registro e não pode se repetir.

Qual combinação de tipos e restrições SQL deve ser utilizada?

- [ ] A) `Codigo VARCHAR(50) NOT NULL`
- [ ] B) `Codigo INT IDENTITY(1,1) PRIMARY KEY`
- [ ] C) `Codigo INT FOREIGN KEY`
- [ ] D) `Codigo TEXT UNIQUE`
- [ ] E) `Codigo INT NULL`

---

#### ❓ Questão 2: Relacionamento entre Tabelas
**Cenário:** A tabela `Tarefa` possui a coluna `CodigoFuncionario`, que faz referência à coluna `Codigo` da tabela `Funcionario`. Essa configuração garante que uma tarefa não seja vinculada a um funcionário inexistente.

Como chamamos essa regra de integridade no banco de dados relational?

- [ ] A) Primary Key (Chave Primária)
- [ ] B) Identity Constraint
- [ ] C) Foreign Key (Chave Estrangeira)
- [ ] D) Index Clustered
- [ ] E) Database First Constraint

---

#### ❓ Questão 3: Manipulação de Dados (DML)
**Cenário:** O sistema precisa registrar um novo incidente no banco de dados via script manual. A tabela `Incidente` possui as colunas `DescricaoProblema`, `DataIncidente` e `Resolvido`.

Qual instrução SQL realiza a inserção desse novo registro corretamente?

- [ ] A) `SELECT INTO Incidente VALUES ('Lentidão no sistema', GETDATE(), 'nao');`
- [ ] B) `UPDATE Incidente SET DescricaoProblema = 'Lentidão no sistema';`
- [ ] C) `CREATE TABLE Incidente ('Lentidão no sistema', GETDATE(), 'nao');`
- [ ] D) `INSERT INTO Incidente (DescricaoProblema, DataIncidente, Resolvido) VALUES ('Lentidão no sistema', GETDATE(), 'nao');`
- [ ] E) `ADD REGISTRO TO Incidente VALUES ('Lentidão no sistema', GETDATE(), 'nao');`

---

### ⚙️ Questões de ASP.NET Core, EF Core & Orientação a Objetos

#### ❓ Questão 4: String de Conexão
**Cenário:** Ao publicar a aplicação ASP.NET Core em um novo ambiente de homologação, o sistema apresentou erro informando que não conseguiu conectar ao SQL Server. O desenvolvedor precisa ajustar a URL do servidor e as credenciais de acesso.

Em qual arquivo padrão do projeto ASP.NET Core a `ConnectionString` fica armazenada?

- [ ] A) `DbContext.cs`
- [ ] B) `Program.cs`
- [ ] C) `appsettings.json`
- [ ] D) `_Layout.cshtml`
- [ ] E) `FuncionarioController.cs`

---

#### ❓ Questão 5: O papel do DbContext
**Cenário:** Em uma entrevista de emprego, o entrevistador pergunta: *"Qual é a principal função da classe `DbTasksContext` que herdou de `DbContext` no nosso projeto?"*

Qual das respostas abaixo descreve corretamente o papel dessa classe?

- [ ] A) Renderizar as páginas HTML e gerenciar o CSS da aplicação.
- [ ] B) Atuar como a ponte entre o código C# e o banco de dados SQL Server, gerenciando a conexão e os conjuntos de dados (`DbSet`).
- [ ] C) Executar scripts de criação de tabelas automaticamente toda vez que a aplicação é iniciada.
- [ ] D) Armazenar as credenciais de login dos usuários de forma criptografada.
- [ ] E) Criar as rotas de navegação no menu principal da aplicação.

---

#### ❓ Questão 6: Conceito de CRUD
**Cenário:** Um analista de sistemas pediu para você criar o "CRUD de Incidentes". 

O que a sigla **CRUD** representa no ciclo de desenvolvimento de software?

- [ ] A) Class, Resource, User, Data
- [ ] B) Connect, Run, Undo, Disconnect
- [ ] C) Compile, Read, Update, Deploy
- [ ] D) Create, Read, Update, Delete
- [ ] E) Code, Refactor, Use, Debug

---
### 💻 Questões de Orientação a Objetos & Scaffold (C# e EF Core)

#### ❓ Questão 7: Abordagem Database First
**Cenário:** Durante a execução do laboratório, a tabela Incidente foi criada diretamente no SQL Server via script SQL. Em seguida, foi executado o comando de scaffold no terminal para atualizar o projeto em C#.

O que a abordagem Database First faz nesse processo?

- [ ] A) Apaga o banco de dados SQL Server e o recria a partir das classes C#.
- [ ] B) Lê a estrutura existente do banco de dados e gera automaticamente as classes de modelo (Models) e o DbContext no projeto.
- [ ] C) Converte o projeto ASP.NET Core em uma aplicação desktop executável.
- [ ] D) Impede qualquer alteração futura nas tabelas do banco de dados.
- [ ] E) Gera relatórios em PDF com base nos dados armazenados nas tabelas.

---

#### ❓ Questão 8: Propriedades e Atributos de Classe
**Cenário:** Na classe de modelo Tarefa, temos a declaração da propriedade Descricao com os acessores get e set.

Dentro dos conceitos da Programação Orientada a Objetos (POO) aplicados ao Entity Framework Core, o que essa estrutura representa?

- [ ] A) Um método responsável por salvar a descrição da tarefa diretamente no banco de dados.
- [ ] B) Uma propriedade (atributo) da classe Tarefa com métodos get e set acessores, que mapeia uma coluna de texto da tabela.
- [ ] C) Uma variável local que só existe enquanto o formulário HTML estiver aberto.
- [ ] D) Um parâmetro obrigatoriamente passado para o construtor da classe DbContext.
- [ ] E) Um comando SQL executado para alterar o tipo de dado da coluna.

---

#### ❓ Questão 9: Assinatura e Parâmetros de Métodos na Controller
**Cenário:** Ao analisar o código gerado pelo Scaffold na FuncionarioController, você encontra o método Details que recebe como entrada um parâmetro de ID inteiro opcional e retorna a View com os dados do funcionário.

Sobre os conceitos de Métodos, Parâmetros e Return, qual afirmação está correta?

- [ ] A) Details é a classe, o ID é o retorno do método e a View é o parâmetro de entrada.
- [ ] B) Details é o método, o ID é um parâmetro opcional (que aceita nulo) e a instrução return View devolve o resultado para ser renderizado na tela.
- [ ] C) A instrução return View apaga a instância do funcionário da memória do servidor.
- [ ] D) O tipo de retorno Task indica que o método não pode receber nenhum parâmetro de entrada.
- [ ] E) O parâmetro do ID indica que o método só aceita números inteiros negativos.

---

#### ❓ Questão 10: Roteamento e Ações do Controlador MVC
**Cenário:** Um usuário acessa o navegador e clica no link de navegação que aponta para a rota /Funcionario/Index.

O que acontece na arquitetura ASP.NET Core MVC para que a lista de funcionários apareça na tela?

- [ ] A) O navegador executa uma instrução SQL diretamente no banco de dados sem passar pelo servidor C#.
- [ ] B) O arquivo appsettings.json intercepta a requisição e renderiza o HTML diretamente para o usuário.
- [ ] C) A requisição atinge a FuncionarioController, que executa o método de ação Index, consulta o DbContext e passa os dados para a exibição (View).
- [ ] D) O Entity Framework ignora a requisição pois a ação Index é reservada apenas para o administrador do sistema.
- [ ] E) O arquivo _Layout.cshtml compila o código C# e gera um novo banco de dados temporário.

---

## 🔑 Gabarito Completo das 10 Questões de Entrevista

<details>
<summary><strong>Clique para expandir o Gabarito e Explicações</strong></summary>

### 🗄️ Banco de Dados & SQL

* **Questão 1: Resposta Correta: B**
  * **Explicação:** O tipo INT com a propriedade IDENTITY(1,1) garante o autoincremento automático a cada novo registro. A restrição PRIMARY KEY garante a unicidade da chave primária.
* **Questão 2: Resposta Correta: C**
  * **Explicação:** A FOREIGN KEY (Chave Estrangeira) é a restrição que estabelece o relacionamento entre tabelas e garante a integridade referencial.
* **Questão 3: Resposta Correta: D**
  * **Explicação:** O comando DML INSERT INTO NomeTabela (colunas) VALUES (valores) é a sintaxe padrão para inserção de dados no SQL Server.

### ⚙️ ASP.NET Core, EF Core & Orientação a Objetos

* **Questão 4: Resposta Correta: C**
  * **Explicação:** O arquivo appsettings.json é o local correto e padronizado para armazenar configurações de ambiente e strings de conexão no ASP.NET Core.
* **Questão 5: Resposta Correta: B**
  * **Explicação:** A classe derivada de DbContext gerencia a conexão, mapeia as tabelas (DbSet) e faz a ponte ORM entre os objetos C# e o banco de dados.
* **Questão 6: Resposta Correta: D**
  * **Explicação:** CRUD significa Create (Criar), Read (Ler), Update (Atualizar) e Delete (Excluir).
* **Questão 7: Resposta Correta: B**
  * **Explicação:** Na abordagem Database First, o banco de dados é a fonte da verdade: a estrutura das tabelas é convertida automaticamente em classes C# e mapeamentos no EF Core via engenharia reversa (scaffold).
* **Questão 8: Resposta Correta: B**
  * **Explicação:** Trata-se de uma propriedade C# autoimplementada com métodos get (leitura) e set (escrita), mapeada pelo Entity Framework para a coluna equivalente na tabela.
* **Questão 9: Resposta Correta: B**
  * **Explicação:** Details é o identificador do método; o ID indica que o método recebe um parâmetro de tipo inteiro anulável (nullable); e a instrução return View devolve o resultado para a camada de apresentação.
* **Questão 10: Resposta Correta: C**
  * **Explicação:** No padrão MVC (Model-View-Controller), o Controller recebe a requisição HTTP, interage com o Model/DbContext para buscar os dados e os repassa para a View correspondente ser renderizada.

</details>