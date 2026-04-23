# Validation

## Criterios de Sucesso

A `Fase 14 - Persistencia de Logs Operacionais` sera considerada bem-sucedida quando:

- houver tabela unica de logs operacionais no MySQL
- a coluna `type` distinguir corretamente `application` e `error`
- logs normais armazenarem payload estruturado de parametros de entrada
- logs de erro armazenarem dados suficientes para rastrear falhas
- existir consulta operacional por periodo e por tipo
- os testes provarem persistencia de logs normais e de erro sem regressao das suites principais

## Validacao Automatizada

As evidencias automatizadas da fase devem cobrir cinco niveis.

### Build e Suites Principais

- `dotnet build GoodHamburguer.slnx -v minimal`
- `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.Blazor.Tests/GoodHamburguer.Blazor.Tests.csproj -v minimal`

### Persistencia de Logs `application`

Deve existir evidencia automatizada de que:

- requisicoes normais geram registro com `type=application`
- `payload` e persistido com estrutura valida
- `route`, `method`, `createdAtUtc` e `correlationId` sao gravados

### Persistencia de Logs `error`

Deve existir evidencia automatizada de que:

- falhas tratadas geram registro com `type=error`
- `exceptionType` e `errorMessage` sao persistidos
- o registro preserva contexto de rota/correlacao para investigacao

### Consulta por Periodo e Tipo

Deve existir evidencia automatizada de que:

- filtros por intervalo de data/hora funcionam
- filtro por tipo (`application` e `error`) funciona
- ordenacao por data/hora desc e aplicada
- limite de itens retornados e respeitado

### Migracao de Banco

Deve existir evidencia de que:

- a migracao da fase cria a tabela e indices esperados
- o modelo EF Core permanece consistente com o schema gerado

## Validacao Manual

Checks manuais esperados:

- criar ao menos uma operacao valida de pedido e confirmar que log `application` foi persistido
- provocar ao menos uma falha controlada e confirmar persistencia de log `error`
- consultar logs por janela de tempo curta e por tipo para validar investigacao rapida
- revisar se a fase manteve execucao local simples sem stack adicional

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- suites principais de teste passando
- novos testes cobrindo log normal, log de erro e consulta por filtros
- migracao EF Core da fase aplicada sem inconsistencias
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- logs forem gravados sem diferenciar `application` e `error`
- payload de entrada for perdido ou gravado de forma nao estruturada
- faltar contexto minimo (rota, horario, correlacao) para rastreabilidade
- consulta por periodo/tipo nao estiver disponivel ou nao tiver cobertura
- falha de gravacao de log causar quebra do fluxo principal da API
- a fase introduzir dependencia externa obrigatoria para subir o ambiente local

## Comandos de Verificacao Base

Executar ao menos:

- `dotnet build GoodHamburguer.slnx -v minimal`
- `dotnet test tests/GoodHamburguer.UnitTests/GoodHamburguer.UnitTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.IntegrationTests/GoodHamburguer.IntegrationTests.csproj -v minimal`
- `dotnet test tests/GoodHamburguer.Blazor.Tests/GoodHamburguer.Blazor.Tests.csproj -v minimal`

Quando a fase incluir migracao de schema, tambem validar:

- `dotnet ef migrations list --project src/GoodHamburguer.Infrastructure --startup-project src/GoodHamburguer.Api`

Quando a fase tocar execucao containerizada, tambem validar:

- `docker compose --env-file .env -f docker/docker-compose.yml config`

## Prova de Alinhamento

Ao final da fase, deve ser possivel explicar claramente:

- como os logs operacionais passam a apoiar auditoria e investigacao
- quais dados minimos ficam registrados em logs normais e logs de erro
- como consultar rapidamente por periodo e por tipo
- por que a implementacao continua proporcional ao escopo do desafio
