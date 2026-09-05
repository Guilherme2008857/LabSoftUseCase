# AppTask — versão corrigida

## 1. Banco de dados

1. Abra o SQL Server Management Studio.
2. Conecte na instância `.`\`SQLEXPRESS` usando Autenticação do Windows.
3. Abra `DataBase/DbEmpresa_Inicial.sql`.
4. Execute o script inteiro.

> O script recria `dbEmpresa`/as tabelas e inclui `CodigoGerente` em `Funcionario`. Se você já tiver dados importantes no banco, faça backup antes, porque o script apaga as tabelas do laboratório.

## 2. Aplicação

O `appsettings.json` já está configurado para:

`Server=.\SQLEXPRESS;Database=dbEmpresa;Trusted_Connection=True;TrustServerCertificate=True;`

Se sua instância do SQL Server tiver outro nome, altere apenas essa conexão.

## 3. Executar

Abra `AppTask/AppTask.csproj` no Visual Studio e pressione F5.

A barra lateral usa rotas MVC normais:
- `/Home/Index`
- `/Tarefa/Index`
- `/Funcionario/Index`
- `/Departamento/Index`
- `/Incidentes/Index`

Se a Home abrir e as demais páginas derem erro 500, quase sempre é conexão com o SQL Server ou banco com esquema diferente do script.
