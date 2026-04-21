# Validation

## Como Saber se a Implementacao Foi Bem-Sucedida

A implementacao desta fase pode ser considerada bem-sucedida quando:

- o catalogo e os pedidos estiverem persistidos em MySQL
- existir um `DbContext` funcional na camada `Infrastructure`
- os mapeamentos refletirem corretamente o estado atual do dominio
- a migration inicial puder ser aplicada com sucesso
- o seed inicial popular catalogo e pedidos de exemplo
- o servico de migration estiver integrado ao `docker-compose`
- houver testes de integracao basicos cobrindo a persistencia

## Verificacoes Automatizadas

Devem passar:

- build da solution
- testes unitarios existentes sem regressao
- testes de integracao da persistencia do catalogo
- testes de integracao da persistencia de pedidos
- validacao da migration inicial em ambiente de teste

## Verificacoes Manuais

Deve ser feita revisao manual para confirmar:

- clareza das entidades e mapeamentos de persistencia
- separacao adequada entre dominio e infraestrutura
- seed com dados coerentes para demonstracao
- leitura do menu funcionando sobre dados persistidos
- configuracao do servico de migration no Docker consistente com a estrategia do projeto

## O que Nao e Obrigatorio Nesta Fase

Ainda nao e obrigatorio:

- expor CRUD completo de pedidos pela API
- aplicar todas as validacoes de negocio do pedido
- calcular subtotal, desconto e total
- concluir health checks, observabilidade e logging
- integrar frontend com a persistencia real dos pedidos

## Criterios de Merge

Esta fase pode ser mergeada quando atender aos seguintes criterios:

- a implementacao estiver aderente aos documentos em `specs/`
- catalogo e pedidos estiverem persistidos em MySQL
- a migration inicial e o seed funcionarem corretamente
- os testes definidos para a fase estiverem aprovados
- a modelagem persistida estiver pronta para evoluir para `Fase 5 - Regras de Validacao`

## Sinais de Qualidade Esperados

Alguns sinais adicionais ajudam a confirmar que a fase ficou realmente pronta:

- o `DbContext` ficou restrito a `Infrastructure`
- os repositorios nao vazam detalhes de EF Core para as outras camadas
- a troca da fonte estatica do menu por persistencia real nao altera o contrato externo da API
- o seed melhora a demonstracao sem mascarar problemas estruturais
