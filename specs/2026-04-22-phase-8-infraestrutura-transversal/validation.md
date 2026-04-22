# Validation

## Criterios de Sucesso

A `Fase 8 - Infraestrutura Transversal` sera considerada bem-sucedida quando:

- a API emitir logs estruturados durante a execucao
- a aplicacao registrar traces basicos com `OpenTelemetry`
- existir um endpoint de `liveness` respondendo com sucesso enquanto o processo estiver saudavel
- existir um endpoint de `readiness` validando a dependencia real de MySQL
- existir tratamento global e centralizado de excecoes HTTP com `ExceptionFilter`
- excecoes conhecidas serem traduzidas de forma consistente para `ProblemDetails`
- os endpoints de saude estiverem cobertos por testes de integracao com persistencia real
- existir workflow de GitHub Actions executando restore, build, testes unitarios e testes de integracao

## Validacao Automatizada

Deve haver evidencias automatizadas em tres niveis.

### Composicao da API

- build da API com os novos pacotes operacionais
- inicializacao da aplicacao sem excecoes de configuracao ligadas a logging, tracing ou health checks
- inicializacao da aplicacao com o filtro global de excecoes registrado
- confirmacao de que o host de testes continua funcionando com a nova composicao

### Integracao HTTP

- teste para `GET /health/live` retornando sucesso
- teste para `GET /health/ready` retornando sucesso com MySQL pronto
- teste confirmando que os endpoints de saude ficam disponiveis no host real da API e nao apenas em um stub
- teste para uma excecao conhecida retornando o `ProblemDetails` esperado
- teste para excecao nao mapeada retornando `500`

### CI

- workflow `ci.yml` valido no repositorio
- workflow configurado para rodar em `push`
- workflow configurado para rodar em `pull_request`
- etapas cobrindo restore, build, testes unitarios e testes de integracao

## Validacao Manual

Manual checks esperados antes do merge:

- subir a API localmente
- consultar `GET /health/live` e `GET /health/ready`
- confirmar que `readiness` nao esta apenas retornando sucesso fixo
- observar logs estruturados no console durante a subida ou durante uma requisicao HTTP

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- `dotnet build GoodHamburguer.slnx -v minimal` passando
- testes unitarios passando
- testes de integracao passando
- endpoints de saude operacionais
- readiness verificada contra a dependencia real de MySQL
- filtro global de excecoes operando de forma consistente
- workflow de CI versionado no repositorio
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- `liveness` e `readiness` forem o mesmo endpoint com nomes diferentes
- `readiness` nao consultar a dependencia real
- nao existir caminho deterministico para validar o filtro global por integracao HTTP
- o tratamento de excecoes continuar espalhado em controllers
- excecoes de dominio passarem a carregar `status code` HTTP ou `ProblemDetails`
- a configuracao de logging ou tracing quebrar a subida da API em ambiente local ou de testes
- os testes de integracao deixarem de usar MySQL real
- o workflow de CI rodar apenas validacoes parciais ou nao executar testes
- a implementacao aumentar significativamente a complexidade operacional sem beneficio claro para a avaliacao tecnica
