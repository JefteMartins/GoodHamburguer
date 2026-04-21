# Plan

## 1. Estruturar o Dominio do Cardapio

- modelar o cardapio com estrutura generica preparada para leitura futura do banco
- representar categorias como `value object`
- representar itens com nome, categoria e preco em `decimal`
- manter o dominio independente de HTTP e de persistencia concreta

## 2. Definir a Abstracao de Consulta

- introduzir uma abstracao de leitura preparada para persistencia futura, como `IMenuQueryService`
- definir contratos de aplicacao para consulta do cardapio
- garantir que a consulta atual possa evoluir para MySQL sem quebrar o contrato da aplicacao

## 3. Implementar a Fonte Inicial de Dados

- fornecer uma implementacao inicial para o catalogo do desafio
- registrar os precos fixos definidos no desafio tecnico
- manter a fonte inicial compatível com futura substituicao por persistencia real

## 4. Expor o Endpoint Versionado

- adicionar o endpoint `GET /api/v1/menu`
- retornar uma estrutura agrupada por categoria
- manter o contrato amigavel para Swagger e para o futuro frontend em Blazor

## 5. Cobrir com Testes

- adicionar testes unitarios do dominio do cardapio
- adicionar testes da camada de aplicacao para a consulta do cardapio
- adicionar teste de integracao do endpoint HTTP de consulta

## 6. Validar a Fase

- verificar o contrato agrupado do endpoint no Swagger
- revisar a aderencia da implementacao a `specs/mission.md`, `specs/tech-stack.md` e `specs/roadmap.md`
- garantir que a fase esteja pronta para evoluir para modelagem de pedido e persistencia real
