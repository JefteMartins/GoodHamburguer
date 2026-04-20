# Publico-Alvo

## Publico Principal

O publico principal deste projeto e o avaliador tecnico que vai baixar o repositorio, subir o ambiente localmente e analisar a qualidade da implementacao.

## O que Esse Publico Espera

Esse publico precisa encontrar:

- execucao local simples e previsivel
- arquitetura clara ao navegar pelo codigo
- separacao coerente de responsabilidades
- testes que inspirem confianca
- demonstracao funcional rapida por Swagger e interface visual

## Forma Principal de Consumo

Na demonstracao inicial, a API sera consumida por:

- Swagger / OpenAPI, para exploracao rapida dos endpoints
- frontend em Blazor, para mostrar o fluxo funcional de ponta a ponta

## Publico Secundario

O lider tecnico e um publico secundario importante. Ao revisar a solucao, ele deve conseguir identificar:

- boas decisoes de arquitetura
- uso disciplinado de patterns
- preparo para evolucao futura
- preocupacao com qualidade operacional, como logging, health checks e padronizacao de erros

## Prioridades de Experiencia

A experiencia do projeto deve ser otimizada nesta ordem:

1. clareza arquitetural ao ler o codigo
2. facilidade extrema para rodar localmente
3. fluidez da demonstracao funcional

## Implicacoes para a Solucao

Essas prioridades significam que:

- o codigo deve ser facil de navegar e explicar
- o setup local deve exigir o minimo possivel de intervencao manual
- a documentacao deve ser objetiva e orientada a demonstracao
- os fluxos principais devem funcionar sem depender de configuracoes escondidas
