# Validation

## Criterios de Sucesso

A `Fase 9 - Integracao via Docker` sera considerada bem-sucedida quando:

- a stack sobe com `docker compose --env-file .env -f docker/docker-compose.yml up --build`
- `mysql` fica `healthy`
- `migrations` conclui com sucesso antes da `api`
- `api` fica operacional e responde endpoints de saude
- `blazor` fica operacional e consegue consumir a `api`
- `readiness` da API reflete acesso real ao MySQL
- o compose tem hardening leve aplicado (`healthcheck`, `depends_on` por condicao, `restart` policy)
- o `README.md` descreve com clareza os comandos e checks de validacao

## Validacao Automatizada

Deve haver evidencias automatizadas em dois niveis.

### Compose e Estrutura

- `docker compose --env-file .env -f docker/docker-compose.yml config` sem erros
- validacao de que os servicos criticos possuem configuracoes esperadas:
  - `mysql` com `healthcheck`
  - `api` com `healthcheck`
  - `blazor` com `healthcheck`
  - `migrations` com comportamento one-shot

### Aplicacao Containerizada

- stack sobe sem falha fatal apos build
- `GET /health/live` retorna sucesso via `api` containerizada
- `GET /health/ready` retorna sucesso com MySQL pronto
- evidencia de que `blazor` alcanca a `api` pela rede interna do compose

## Validacao Manual

Checks manuais esperados:

- abrir `http://localhost:8080` e confirmar carregamento do Blazor
- abrir Swagger da API em `http://localhost:8081/swagger` (ou rota equivalente)
- consultar `GET /health/live`
- consultar `GET /health/ready`
- observar logs dos containers para confirmar ordem de inicializacao e ausencia de loop de falha

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- compose valido (`docker compose ... config`)
- stack completa iniciando localmente
- health checks operacionais
- conectividade entre servicos confirmada
- README atualizado e aderente ao fluxo real
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- a stack depende de ordem manual fora do compose
- `api` inicia sem garantia de migrations concluidas
- readiness retornar sucesso sem validar dependencia real
- `blazor` subir mas nao conseguir integrar com a API
- documentacao estiver desatualizada em relacao aos comandos reais
- os ajustes aumentarem complexidade operacional sem beneficio claro de confiabilidade
