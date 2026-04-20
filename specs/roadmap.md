# Roadmap

## Estrategia

A implementacao deve avancar em fases pequenas, cada uma deixando o sistema em um estado executavel e demonstravel.

## Fase 1 - Fundacao do Projeto

Objetivo: preparar a base da solucao.

- criar a solution .NET 10
- criar os projetos principais e referencias entre camadas
- estruturar a solucao em Clean Architecture com orientacao a DDD
- configurar Swagger
- configurar versionamento de API
- definir estrutura de pastas
- preparar a base arquitetural para o frontend em Blazor, sem implementa-lo ainda
- estruturar a pasta `docker/`
- adicionar Dockerfiles da API e do Blazor
- adicionar `docker-compose.yml` com API, Blazor, MySQL e servico de migration
- definir configuracao inicial por variaveis de ambiente
- adicionar README inicial com objetivo e instrucoes basicas

## Fase 2 - Dominio do Cardapio

Objetivo: representar o cardapio e os tipos de item.

- modelar categorias e itens do cardapio
- registrar os precos fixos do desafio
- criar servico ou fonte de dados para consulta do cardapio
- adicionar testes unitarios iniciais do dominio do cardapio

## Fase 3 - Modelagem de Pedido

Objetivo: criar a representacao correta do pedido.

- definir entidade de pedido
- definir contrato de criacao e atualizacao
- representar as restricoes de um item por categoria
- incluir identificador e datas basicas, se fizer sentido
- adicionar testes unitarios iniciais da modelagem de pedido

## Fase 4 - Persistencia MySQL

Objetivo: introduzir a persistencia real logo apos a consolidacao do dominio.

- criar interfaces de repositorio para os agregados principais
- configurar `DbContext` na camada `Infrastructure`
- mapear entidades e tabelas
- criar migracao inicial
- implementar servico de migration e seed para uso no Docker
- garantir seed inicial com cardapio e alguns pedidos de exemplo

## Fase 5 - Regras de Validacao

Objetivo: impedir entradas invalidas logo no inicio.

- validar item obrigatoriamente pertencente ao cardapio
- bloquear duplicidade por categoria
- retornar mensagens claras para combinacoes invalidas
- padronizar respostas de erro com `ProblemDetails`
- adicionar testes cobrindo cenarios invalidos relevantes

## Fase 6 - Calculo de Valores

Objetivo: aplicar corretamente as regras do desafio.

- calcular subtotal
- calcular desconto por combinacao
- calcular total final
- cobrir com testes unitarios cenarios de happy path
- cobrir com testes unitarios edge cases
- cobrir com testes unitarios cenarios invalidos relevantes

## Fase 7 - CRUD de Pedidos

Objetivo: entregar os endpoints principais.

- criar pedido
- listar pedidos
- consultar pedido por identificador
- atualizar pedido
- remover pedido
- implementar o CRUD ja sobre a persistencia real
- expor os endpoints em `v1`

## Fase 8 - Infraestrutura Transversal

Objetivo: adicionar capacidades transversais importantes para operacao e avaliacao tecnica.

- configurar logging estruturado
- configurar observabilidade basica com traces
- adicionar health checks com `liveness` e `readiness`
- validar `readiness` com dependencia real do MySQL
- adicionar testes de integracao end-to-end com persistencia real
- configurar GitHub Actions com restore, build, testes unitarios e testes de integracao em `push` e `pull request`

## Fase 9 - Integracao via Docker

Objetivo: facilitar execucao e avaliacao do projeto.

- validar subida completa com Docker
- garantir conexao entre API, Blazor e MySQL
- validar execucao do servico dedicado de migration e seed
- documentar comandos de build e run
- revisar funcionamento de health checks e readiness no ambiente containerizado

## Fase 10 - Frontend Blazor

Objetivo: entregar a demonstracao visual integrada apos a consolidacao do back-end.

- implementar o frontend em Blazor Web App
- consumir a API versionada
- cobrir os fluxos principais de consulta de cardapio e operacao de pedidos
- validar integracao funcional entre frontend e API

## Fase 11 - Testes e Cobertura

Objetivo: consolidar a confianca tecnica da entrega.

- ampliar testes unitarios restantes
- ampliar testes de integracao principais
- perseguir cobertura minima de 70%
- revisar cenarios de happy path, edge cases e falhas relevantes

## Fase 12 - Qualidade Operacional e Documentacao

Objetivo: deixar a entrega mais robusta e apresentavel.

- revisar nomes e contratos
- revisar codigos HTTP
- revisar mensagens de erro padronizadas
- complementar observabilidade basica, se necessario
- complementar README com decisoes de arquitetura e limites da solucao
- revisar e refinar a pipeline de CI, se necessario

## Fase 13 - Melhorias Incrementais

Objetivo: aumentar o valor da entrega se houver tempo.

- elevar cobertura de testes em cenarios de borda
- enriquecer a experiencia visual do Blazor
- expandir a pipeline de CI com verificacoes adicionais
- aprofundar metricas e traces

## Definicao de Pronto da Primeira Entrega

A primeira entrega esta pronta quando:

- a API, o Blazor e o MySQL sobem por Docker com poucos comandos
- o servico de migration e seed funciona corretamente no ambiente local
- o cardapio pode ser consultado
- o CRUD de pedidos funciona
- os descontos sao calculados corretamente
- a API esta exposta em `v1`
- o Swagger esta funcional e organizado para exploracao da API versionada
- erros de validacao sao retornados com `ProblemDetails`
- os health checks indicam o estado da aplicacao
- os testes automatizados sustentam a confianca da entrega
- a pipeline de CI executa build e testes automaticamente
- existe README suficiente para avaliacao tecnica
