# AppTask — versão reorganizada

Esta versão remove as duplicidades de controllers, views e contextos que existiam no projeto anterior. O projeto utiliza um único `DbTasksContext`, um `FuncionarioController` e um `DepartamentoController`, com rotas singulares:

```text
/Funcionario
/Departamento
/Tarefa
/Incidentes
```

O relacionamento entre `Funcionario` e `Departamento` está mapeado no Entity Framework e o cadastro/edição de funcionários possui um select de departamento. A exclusão de um departamento com funcionários vinculados é bloqueada com uma mensagem amigável. O mesmo comportamento é aplicado à exclusão de funcionários com tarefas vinculadas.

## Banco de dados

O script `DataBase/DbEmpresa_Inicial.sql` recria as tabelas do laboratório no banco `dbEmpresa`:

```text
Departamento
Funcionario
Tarefa
Incidente
```

O script remove as tabelas existentes antes de recriá-las. Portanto, ele deve ser usado somente quando for aceitável apagar os dados atuais do laboratório. Execute o script no SQL Server e depois confirme que a connection string em `AppTask/appsettings.json` aponta para `dbEmpresa`.

## Execução

Abra `SLNTask.sln` no Visual Studio, restaure os pacotes, compile a solução e execute o projeto. As opções de navegação ficam no menu principal. O projeto não contém `bin` e `obj` na versão distribuída; esses diretórios serão gerados automaticamente pelo Visual Studio.
