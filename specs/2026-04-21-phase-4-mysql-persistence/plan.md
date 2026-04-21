# Persistencia MySQL Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use `superpowers:subagent-driven-development` (recommended) or `superpowers:executing-plans` to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Persistir `Order` e o catalogo em MySQL com `DbContext`, mapeamentos, migration inicial, seed de demonstracao e servico de migration no Docker.

**Architecture:** A persistencia deve ficar inteiramente na camada `Infrastructure`, com repositórios orientados a interface para `Order` e para leitura/escrita do catalogo. O dominio continua como fonte de verdade das regras e a infraestrutura passa a materializar o estado em tabelas e dados seeded.

**Tech Stack:** .NET 10, C# 14, EF Core, Pomelo MySQL provider, MySQL 8.4, Docker Compose, xUnit, FluentAssertions.

---

### Task 1: Definir contratos e fronteiras da persistencia

**Files:**
- Modify: `src/GoodHamburguer.Application/Orders/Services/IOrderDraftingService.cs`
- Create: `src/GoodHamburguer.Application/Orders/Abstractions/IOrderRepository.cs`
- Create: `src/GoodHamburguer.Application/Menu/Abstractions/IMenuCatalogRepository.cs`
- Create: `src/GoodHamburguer.Application/Menu/Abstractions/IMenuCatalogSeeder.cs` if needed

- [ ] Definir a interface de repositorio para `Order`
- [ ] Definir a interface de persistencia/consulta do catalogo preparada para substituir a fonte estatica atual
- [ ] Manter contratos livres de `DbContext`, `DbSet` e detalhes de EF Core
- [ ] Revisar se as abstrações continuam coerentes com a Clean Architecture adotada

### Task 2: Introduzir o `DbContext` da aplicacao

**Files:**
- Create: `src/GoodHamburguer.Infrastructure/Persistence/GoodHamburguerDbContext.cs`
- Modify: `src/GoodHamburguer.Infrastructure/DependencyInjection.cs`
- Modify: `src/GoodHamburguer.Infrastructure/GoodHamburguer.Infrastructure.csproj`

- [ ] Criar o `DbContext` com `DbSet` para pedidos e itens de catalogo
- [ ] Registrar o provider MySQL com `Pomelo.EntityFrameworkCore.MySql`
- [ ] Ler a connection string a partir da configuracao/variaveis de ambiente existentes
- [ ] Garantir que o `DbContext` permaneça restrito a `Infrastructure`

### Task 3: Mapear o catalogo para o banco

**Files:**
- Create: `src/GoodHamburguer.Infrastructure/Persistence/Entities/MenuItemEntity.cs`
- Create: `src/GoodHamburguer.Infrastructure/Persistence/Configurations/MenuItemEntityConfiguration.cs`
- Modify: `src/GoodHamburguer.Infrastructure/Menu/StaticMenuQueryService.cs`
- Create: `src/GoodHamburguer.Infrastructure/Menu/DbMenuCatalogRepository.cs`

- [ ] Criar a entidade de persistencia do catalogo com colunas claras para nome, categoria e preco
- [ ] Configurar tabela, chaves, tamanhos e precisao decimal
- [ ] Substituir a fonte estatica atual por um repositorio persistido
- [ ] Garantir que a leitura do menu continue retornando o catalogo do dominio como fonte de verdade do fluxo

### Task 4: Mapear pedidos para o banco

**Files:**
- Create: `src/GoodHamburguer.Infrastructure/Persistence/Entities/OrderEntity.cs`
- Create: `src/GoodHamburguer.Infrastructure/Persistence/Configurations/OrderEntityConfiguration.cs`
- Create: `src/GoodHamburguer.Infrastructure/Orders/DbOrderRepository.cs`

- [ ] Criar a entidade de persistencia de `Order`
- [ ] Decidir o formato de armazenamento dos slots `sandwich`, `side` e `drink` de forma clara e evolutiva
- [ ] Mapear identificador, datas e os dados necessarios dos itens selecionados
- [ ] Implementar o repositorio de pedidos usando o `DbContext`

### Task 5: Criar a migration inicial

**Files:**
- Create: `src/GoodHamburguer.Infrastructure/Persistence/Migrations/*`
- Modify: `src/GoodHamburguer.Infrastructure/GoodHamburguer.Infrastructure.csproj` if tooling config is needed

- [ ] Gerar a migration inicial com as tabelas do catalogo e de pedidos
- [ ] Revisar nomes de tabelas, colunas, chaves e precisao de valores monetarios
- [ ] Garantir que a migration represente o estado atual do dominio sem antecipar regras das fases seguintes

### Task 6: Implementar o seed inicial de demonstracao

**Files:**
- Create: `src/GoodHamburguer.Infrastructure/Persistence/Seed/DatabaseSeeder.cs`
- Create: `src/GoodHamburguer.Infrastructure/Persistence/Seed/DatabaseSeedOptions.cs` if useful

- [ ] Popular o catalogo com os itens fixos do desafio
- [ ] Inserir alguns pedidos de exemplo coerentes com a modelagem atual
- [ ] Tornar o seed idempotente para nao duplicar dados em reexecucoes

### Task 7: Adicionar o servico de migration no Docker

**Files:**
- Modify: `docker/docker-compose.yml`
- Create or Modify: `docker/api/Dockerfile`
- Create: a migration entrypoint or command configuration under `docker/` if needed
- Modify: `README.md` if execution changes become relevant in this phase

- [ ] Adicionar um servico dedicado de migration no `docker-compose`
- [ ] Reutilizar a imagem da aplicacao para executar `database update` e seed
- [ ] Garantir dependencia adequada entre `mysql`, migration e API
- [ ] Manter o fluxo alinhado ao objetivo de subir o ambiente com poucos comandos

### Task 8: Cobrir a persistencia com testes

**Files:**
- Create: `tests/GoodHamburguer.IntegrationTests/Persistence/MenuPersistenceTests.cs`
- Create: `tests/GoodHamburguer.IntegrationTests/Persistence/OrderPersistenceTests.cs`
- Modify: `tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj`

- [ ] Adicionar testes de integracao para leitura persistida do catalogo
- [ ] Adicionar testes de integracao para salvar e recuperar pedidos
- [ ] Preferir banco real efemero com a estrategia de testes ja definida para o projeto

### Task 9: Validar a fase

**Files:**
- Review: `specs/2026-04-21-phase-4-mysql-persistence/requirements.md`
- Review: `specs/2026-04-21-phase-4-mysql-persistence/validation.md`
- Review: `specs/roadmap.md`

- [ ] Rodar `restore`, `build` e os testes relevantes
- [ ] Validar a aplicacao da migration inicial
- [ ] Validar o seed do catalogo e dos pedidos de exemplo
- [ ] Confirmar que a API continua lendo o menu a partir da persistencia real
- [ ] Revisar se a fase ficou pronta para `Fase 5 - Regras de Validacao`
