# Validation

## Criterios de Sucesso

A `Fase 13 - Melhorias Incrementais` sera considerada bem-sucedida quando:

- pelo menos uma melhoria concreta e verificavel existir em cada uma das quatro frentes definidas
- os ganhos forem pequenos, claros e cumulativos, sem transformar a fase em um redesign
- os testes seguirem protegendo comportamento importante em vez de apenas inflar cobertura
- o Blazor ficar visualmente mais legivel e convincente para demonstracao
- o CI passar a oferecer uma leitura melhor da qualidade da entrega
- a telemetria ficar mais util para diagnosticar operacoes de pedido

## Validacao Automatizada

As evidencias automatizadas da fase devem cobrir quatro niveis.

### Build e Suites

- `dotnet build GoodHamburguer.slnx -v minimal`
- `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.Blazor.Tests/GoodHamburguer.Blazor.Tests.csproj -v minimal`

### Testes de Borda

Deve haver novas evidencias automatizadas cobrindo pelo menos:

- validacao de pedidos em cenarios limite
- operacoes HTTP de pedido com falhas menos triviais
- estados ou fluxos principais do Blazor afetados pelos refinamentos

### Pipeline

O workflow principal deve continuar verde com os checks adicionais definidos para a fase e produzir saidas compreensiveis para analise humana.

### Telemetria

Deve existir pelo menos uma evidencia automatizada de que a configuracao ou os sinais instrumentados esperados estao presentes, seja por testes de unidade, testes de integracao ou ambos.

## Validacao Manual

Checks manuais esperados:

- revisar visualmente `/orders/new`, `/orders/{id}` e `/dashboard/orders`
- confirmar que loading, erro, vazio e sucesso ficaram mais claros
- inspecionar a pipeline e os artefatos para garantir que a leitura do resultado melhorou
- validar que a telemetria adicional nao exige infraestrutura nova para subir o projeto localmente

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- suites de teste principais passando
- novos testes cobrindo bordas relevantes
- telas principais do Blazor refinadas sem regressao funcional
- workflow de CI atualizado e compreensivel
- telemetria enriquecida nos fluxos de pedido
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- uma das quatro frentes ficar sem entrega concreta
- as melhorias de UX introduzirem regressao de fluxo
- o CI ficar mais lento ou opaco sem ganho claro
- a telemetria nova depender de stack externa nao prevista
- os testes novos forem predominantemente artificiais ou redundantes
- a fase se transformar em refatoracao ampla sem relacao direta com o valor incremental prometido
