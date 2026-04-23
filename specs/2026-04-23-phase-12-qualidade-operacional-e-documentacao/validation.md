# Validation

## Criterios de Sucesso

A `Fase 12 - Qualidade Operacional e Documentacao` sera considerada bem-sucedida quando:

- os contratos HTTP principais estiverem mais consistentes e previsiveis
- os erros padronizados estiverem mais completos para integracao e troubleshooting
- os endpoints operacionais expuserem payloads mais uteis
- o `README.md` estiver mais forte para avaliacao tecnica
- a pipeline de CI produzir sinais operacionais mais claros sem perder o gate de qualidade
- tudo isso ocorrer sem introduzir regressao funcional nas suites existentes

## Validacao Automatizada

Deve haver evidencias automatizadas em quatro niveis.

### Build da Solucao

- `dotnet build GoodHamburguer.slnx -v minimal` sem erros

### Contratos e Integracao da API

- testes de integracao cobrindo os contratos de `orders`
- testes de integracao cobrindo `ProblemDetails` e `ValidationProblemDetails`
- testes de integracao cobrindo `/health/live`, `/health/ready` e `/api/v1/system/info`

### Suites Principais

- `tests/GoodHamburguer.UnitTests` passando
- `tests/GoodHamburguer.IntegrationTests` passando
- `tests/GoodHamburguer.Blazor.Tests` passando

### Pipeline

- a workflow principal continua executando build, testes e cobertura
- a workflow passa a produzir sinais operacionais mais claros para avaliacao

## Validacao Manual

Checks manuais esperados:

- revisar o `README.md` renderizado e confirmar que ele explica a solucao em leitura rapida
- revisar a workflow de CI e confirmar que a pipeline ficou mais compreensivel
- revisar o contrato HTTP mais sensivel da fase, especialmente criacao de pedidos
- revisar payloads de erro e health para confirmar utilidade real, nao apenas mudanca cosmetica
- conferir que a fase continua alinhada ao escopo da primeira entrega

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- testes unitarios passando
- testes de integracao passando
- testes Blazor passando
- contratos HTTP refinados e cobertos por testes
- payloads de erro e health refinados e cobertos por testes
- `README.md` atualizado para avaliacao tecnica
- CI atualizada com sinais operacionais mais claros
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- houver alteracao de contrato HTTP sem cobertura correspondente
- os erros continuarem inconsistentes ou pobres em contexto relevante
- os health endpoints melhorarem apenas superficialmente sem ganho util
- o `README.md` continuar fraco para explicar arquitetura, contratos e limites
- a CI ficar mais complexa sem produzir clareza adicional
- a fase escorregar para melhorias incrementais tipicas da Fase 13

## Comandos de Verificacao Base

Executar ao menos:

- `dotnet build GoodHamburguer.slnx -v minimal`
- `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.Blazor.Tests/GoodHamburguer.Blazor.Tests.csproj -v minimal`

Quando a fase tocar Docker guidance ou health operacional, tambem validar:

- `docker compose --env-file .env -f docker/docker-compose.yml config`

## Prova de Alinhamento

Ao final da fase, deve ser possivel explicar claramente:

- o que foi refinado na superficie externa da API
- como isso melhora a avaliacao tecnica da entrega
- por que a fase ainda pertence a `Qualidade Operacional e Documentacao`
- o que permanece para a `Fase 13 - Melhorias Incrementais`
