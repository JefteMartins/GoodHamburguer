# Plan

## 1. Estruturar a Solution

- criar a solution `.NET 10`
- criar os projetos definidos para `src/` e `tests/`
- adicionar os projetos a solution

## 2. Definir a Arquitetura Base

- organizar a estrutura inicial em `Api`, `Application`, `Domain`, `Infrastructure`, `Blazor` e `tests`
- configurar as referencias entre camadas de forma aderente a Clean Architecture com DDD pragmatico
- garantir que a base arquitetural do frontend exista, mesmo sem implementacao funcional nesta fase

## 3. Preparar a API para Evolucao

- configurar `Controllers`
- configurar versionamento de API em `v1`
- configurar Swagger versionado
- configurar `ProblemDetails` basico

## 4. Configurar Ambientes

- definir configuracoes iniciais por ambiente
- preparar valores e convencoes necessarios para API, Blazor e futura persistencia
- garantir que a configuracao esteja pronta para evolucao sem acoplamento prematuro

## 5. Preparar a Base de Docker

- estruturar a pasta `docker/`
- criar os `Dockerfiles` da API e do Blazor
- criar o `docker/docker-compose.yml`
- incluir servicos base para `API`, `Blazor` e `MySQL`
- deixar o compose preparado para receber o servico de migration em fase posterior, sem implementa-lo ainda

## 6. Documentar a Fundacao

- criar ou atualizar o README inicial
- documentar visao do projeto
- documentar arquitetura e estrutura de pastas
- documentar como rodar com Docker
- incluir um resumo do roadmap

## 7. Validar a Fase

- garantir que a solution compile integralmente
- validar que os projetos estejam referenciados corretamente
- validar que Swagger esteja configurado para subir
- revisar se a estrutura criada esta aderente aos documentos em `specs/`
