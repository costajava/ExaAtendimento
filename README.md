# Sistema de Atendimento - Projeto Completo

## Visão Geral

Sistema completo de gerenciamento de atendimentos desenvolvido com:
- **Backend:** ASP.NET Core 8.0 Web API
- **Frontend:** Angular 20.1.0 com Material Design

## Estrutura do Projeto

```
ExaAtendimento/
├── ExaAtendimento.API/              # API REST
├── ExaAtendimento.Application/      # Camada de Aplicação (Services, DTOs)
├── ExaAtendimento.Domain/           # Camada de Domínio (Entities)
├── ExaAtendimento.InfraData/        # Camada de Infraestrutura (Repositories, EF Core)
└── AtendimentoApp/                  # Frontend Angular
```

## Backend (.NET)

### Tecnologias
- ASP.NET Core 8.0
- Entity Framework Core
- FluentValidation
- Mapster (AutoMapper)
- SQL Server

### Entidades Implementadas
1. **Assunto** - Assuntos de atendimento
2. **Atendimento** - Registro de atendimentos
3. **CA** - Controle de Alterações
4. **Cliente** - Cadastro de clientes
5. **Módulo** - Módulos do sistema
6. **Sugestão** - Sugestões de melhorias
7. **TipoAtendimento** - Tipos de atendimento
8. **Usuário** - Usuários do sistema

### Arquitetura
- **Clean Architecture** com separação de camadas
- **Repository Pattern** para acesso a dados
- **DTO Pattern** para transferência de dados
- **Validation** com FluentValidation
- **Mapping** com Mapster

### Endpoints da API

Todos os controllers seguem o padrão REST:

```
GET    /api/{entity}           # Listar todos
GET    /api/{entity}/{id}      # Obter por ID
POST   /api/{entity}           # Criar
PUT    /api/{entity}/{id}      # Atualizar
DELETE /api/{entity}/{id}      # Deletar
POST   /api/{entity}/listar    # Listar com paginação/filtros
```

### Como Executar o Backend

1. Configure a connection string em `appsettings.json`:
   Ajustando nome do banco, usuario e senha

   Projeto preperado para rodar com banco MySql, podendo ser adaptado para
   rodar com SqlSrver, ou outro de preferência.

2. Execute as migrations:
   ```bash
   cd ExaAtendimento.InfraData
   dotnet ef database update
   ```
3. Execute a API:
   ```bash
   cd ExaAtendimento.API
   dotnet run
   ```

A API estará disponível em: `https://localhost:7001`


## Recursos Implementados

### Backend
- Controllers REST completos
- Services com lógica de negócio
- Repositories com EF Core
- DTOs para todas as entidades
- Validações com FluentValidation
- Mapeamento com Mapster
- Paginação e filtros
- Tratamento de erros
- Autenticação JWT

## Padrões e Boas Práticas Aplicadas

- Clean Architecture
- Repository Pattern
- DTO Pattern
- Dependency Injection
- Separation of Concerns
- SOLID Principles

## Usuário admin:
- Ao rodar o projeto pela primeira se não existir usuário admin, ele será criado
  com as credenciais informadas em: AdminSettings

## Licença

Este projeto foi desenvolvido para fins educacionais e de demonstração.

**Desenvolvido com:** ASP.NET Core 8.0 + Angular 20.1.0 + Material Design + Tailwind CSS

