# Requirements

## Escopo

Esta feature cobre a `Fase 9 - Integracao via Docker`, com foco em tornar o ambiente containerizado uma experiencia previsivel para avaliacao tecnica local, incluindo ajustes estruturais leves de hardening operacional.

A fase deve validar de ponta a ponta a integracao entre:

- `mysql`
- `migrations`
- `api`
- `blazor`

## O que Esta Incluido

- validacao completa da subida por `docker compose`
- ajuste estrutural do `docker-compose.yml` quando necessario
- encadeamento de inicializacao por saude real entre servicos
- `healthcheck` para servicos de runtime (`api` e `blazor`), alem do MySQL
- politicas de `restart` adequadas ao papel de cada servico
- confirmacao de readiness real contra MySQL no ambiente containerizado
- verificacao de conectividade `Blazor -> API` e `API -> MySQL`
- revisao pontual de Dockerfiles quando houver gap operacional
- documentacao operacional objetiva no `README.md`

## O que Fica Fora Desta Fase

- mudancas de contrato funcional da API de dominio (`menu` e `orders`)
- mudancas de regra de negocio de pedidos/descontos
- orquestracao de producao (Kubernetes, Swarm, ECS, etc.)
- observabilidade avancada (dashboards, collector dedicado, APM externo)
- seguranca avancada de container (rootless hardening completo, scanning CI, SBOM)
- split de multiplos arquivos compose por perfil complexo

## Decisoes Confirmadas

### Direcao de Hardening

Foi aprovada a opcao de hardening leve focada em avaliacao local:

- manter arquivo unico principal de compose
- melhorar robustez de inicializacao via `healthcheck` e `depends_on` por condicao
- definir `restart` policy explicita por servico
- evitar aumento desnecessario de complexidade operacional

### Ordem de Inicializacao

A ordem funcional esperada e:

1. `mysql` fica saudavel
2. `migrations` executa e conclui com sucesso
3. `api` inicia e passa nos probes de saude
4. `blazor` inicia e consegue consumir `api`

### Sem Simular Readiness

`readiness` nao pode ser sucesso fixo sem dependencia real. Em ambiente Docker, o status deve refletir estado real de acesso ao MySQL.

### Papel do Servico de Migracao

`migrations` continua como servico one-shot:

- executa apenas o necessario para migration/seed
- nao reinicia automaticamente
- bloqueia subida da `api` em caso de falha

### Politicas de Restart

A fase deve explicitar politicas de restart coerentes:

- `migrations`: `restart: "no"`
- servicos de runtime (`api`, `blazor`, `mysql`): politica resiliente a falhas transientes locais (ex.: `unless-stopped`)

## Contexto e Restricoes

O projeto ja possui:

- compose com servicos principais
- Dockerfiles para API e Blazor
- migration/seed em servico dedicado
- endpoints de health na API

A fase ainda precisa consolidar:

- robustez da cadeia de inicializacao
- verificacao objetiva de saude dos servicos de runtime
- documentacao alinhada aos comandos reais

## Padroes a Seguir

- preferir simplicidade operacional e comandos curtos
- manter alinhamento com `specs/mission.md`, `specs/tech-stack.md` e `specs/roadmap.md`
- fazer apenas ajustes estruturais com beneficio direto e verificavel
- evitar overengineering no compose
- garantir que qualquer avaliador consiga subir e validar o ambiente com poucos passos
