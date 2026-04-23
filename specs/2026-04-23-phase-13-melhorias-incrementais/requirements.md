# Requirements

## Escopo

Esta feature cobre a `Fase 13 - Melhorias Incrementais`, concebida como uma fase de hardening pos-entrega para aumentar o valor tecnico e visual do projeto sem reabrir a arquitetura principal.

A fase foi decomposta em quatro frentes independentes:

- testes de borda e regressao
- refinamentos visuais do Blazor
- verificacoes adicionais de CI
- aprofundamento de metricas e traces

O objetivo nao e reiniciar o projeto, mas melhorar a experiencia de avaliacao e a capacidade de diagnosticar regressao com mudancas pequenas e reviewables.

## O que Esta Incluido

- ampliacao seletiva de testes em cenarios de borda ainda pouco cobertos
- melhorias de UX e apresentacao nas telas principais de pedido do Blazor
- fortalecimento do CI com checks adicionais de qualidade e apresentacao
- enriquecimento de observabilidade nos fluxos de pedido
- documentacao do recorte da fase e da estrategia de execucao incremental

## O que Fica Fora Desta Fase

- reestruturacao ampla de Clean Architecture
- troca de stack de frontend ou backend
- adocao de plataforma externa obrigatoria para observabilidade
- browser E2E completos como requisito necessario
- redesign total das telas do Blazor
- automatizacoes de CI sem retorno claro para avaliacao tecnica

## Decisoes Confirmadas

### A fase e incremental de verdade

A Fase 13 continua sendo uma unica fase de roadmap, mas a implementacao deve ser dividida em lotes autonomos. O principal risco aqui seria transformar melhorias opcionais em um pacote grande e difuso; esta spec evita isso explicitamente.

### Testes devem proteger regressao real

Depois da Fase 11, o projeto ja atingiu um baseline de confianca e cobertura. O foco agora e ampliar a protecao onde ainda ha cenarios de borda com potencial de fuga, em especial validacoes, atualizacoes de pedido e contratos de erro.

### UX do Blazor deve melhorar sem quebrar identidade

As telas principais do fluxo de pedido ja existem e refletem a referencia de Stitch adotada na Fase 10. A meta desta fase nao e refaze-las, mas torna-las mais claras, mais legiveis e mais convincentes para demonstracao.

### CI adicional deve reforcar apresentacao da entrega

O pipeline atual ja cobre build, testes e cobertura. O ganho adicional desta fase deve aumentar confianca e legibilidade para avaliacao, nao apenas adicionar tempo de execucao.

### Observabilidade deve continuar leve

Ja existe instrumentacao basica em `src/GoodHamburguer.Api/Program.cs`. A fase deve aprofundar essa base com sinais mais uteis sobre operacoes de pedido, mas sem obrigar o projeto a depender de stack externa para rodar localmente.

## Frentes e Alvos Minimos

### Frente 1: Testes de Borda

Alvos minimos:

- `tests/GoodHamburguer.UnitTests/OrderTests.cs`
- `tests/GoodHamburguer.UnitTests/OrderRequestValidatorTests.cs`
- `tests/GoodHamburguer.IntegrationTests/OrdersEndpointTests.cs`
- `tests/GoodHamburguer.IntegrationTests/ExceptionHandlingTests.cs`

Tipos de cenario esperados:

- pedidos com combinacoes limite ou campos opcionais omitidos
- atualizacoes invalidas que hoje podem escapar sem cobertura suficiente
- respostas de erro coerentes em falhas menos triviais
- comportamento serializado e persistido em limites relevantes

### Frente 2: Refinamento Visual do Blazor

Alvos minimos:

- `src/GoodHamburguer.Blazor/Components/Pages/NewOrder.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/OrderDetails.razor`
- `src/GoodHamburguer.Blazor/Components/Pages/OrdersDashboard.razor`

Aspectos obrigatorios:

- hierarquia visual mais forte
- estados de loading, erro, vazio e sucesso mais expressivos
- melhor distribuicao da informacao operacional
- manutencao do design local ja consolidado

### Frente 3: CI Adicional

Alvos minimos:

- `.github/workflows/ci.yml`
- `README.md`

Resultados esperados:

- checks adicionais que validem melhor a entrega
- artefatos e saidas mais claros para quem avalia o repositorio
- pipeline ainda enxuta o bastante para nao comprometer feedback rapido

### Frente 4: Metricas e Traces

Alvos minimos:

- `src/GoodHamburguer.Api/Program.cs`
- `src/GoodHamburguer.Application/Orders/Services/OrderAppService.cs`
- testes cobrindo a configuracao ou o comportamento esperado da telemetria

Sinais obrigatorios:

- spans enriquecidos em operacoes de pedido
- metricas de volume e resultado dessas operacoes
- permanencia de execucao simples no ambiente local

## Contexto e Restricoes

O projeto ja entrega:

- API versionada em `v1`
- frontend Blazor funcional
- MySQL em Docker
- pipeline de CI com build, testes e cobertura
- observabilidade basica com OpenTelemetry

As melhorias desta fase devem respeitar essa base e evitar acoplamentos novos que compliquem a execucao local.

## Padroes a Seguir

- manter alinhamento com `specs/mission.md`, `specs/tech-stack.md` e `specs/roadmap.md`
- quebrar a implementacao em frentes pequenas e revisaveis
- preferir ganho claro de confianca, legibilidade ou diagnostico
- seguir os arquivos e convencoes ja existentes antes de criar novas abstracoes
- atualizar documentacao quando a mudanca alterar a forma de validar ou demonstrar o projeto
