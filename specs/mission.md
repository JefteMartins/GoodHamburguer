# Missao do Produto

## Visao

Construir um sistema simples, confiavel e bem estruturado para registrar pedidos da lanchonete Good Hamburger, com foco em clareza das regras de negocio, escalabilidade da solucao, boa arquitetura e uma experiencia previsivel para quem consome a API.

## Problema que Estamos Resolvendo

A Good Hamburger precisa registrar pedidos de forma consistente, evitando ambiguidades no calculo de valores e descontos promocionais. O desafio exige que o sistema represente corretamente o cardapio, valide combinacoes invalidas e exponha isso por meio de uma API REST.

## Objetivo Principal

Entregar uma solucao completa em .NET capaz de:

- expor o cardapio disponivel
- criar, listar, consultar, atualizar e remover pedidos
- calcular subtotal, desconto e total final com base nas regras promocionais
- impedir pedidos invalidos, especialmente itens duplicados por categoria
- retornar erros claros e consistentes para facilitar integracao e manutencao
- atender os requisitos obrigatorios e opcionais do desafio de forma coesa e bem executada
- considerar o frontend desde o inicio da arquitetura, mesmo com prioridade de implementacao posterior ao back-end

## Principios da Solucao

- Simplicidade bem aplicada: evitar complexidade desnecessaria, mas sem sacrificar extensibilidade.
- Clareza acima de esperteza: regras de negocio, fluxos e contratos devem ser faceis de entender ao ler o codigo.
- Escalabilidade arquitetural: a arquitetura deve suportar crescimento funcional e evolucao do codigo sem reescrita desordenada.
- Regras explicitas: logica de desconto e validacao deve ficar facil de localizar e testar.
- API previsivel: contratos claros, respostas consistentes e codigos HTTP corretos.
- Evolucao segura: estrutura preparada para crescer sem comprometer a leitura do codigo.
- Confianca no codigo entregue: regras centrais devem poder ser validadas sem depender da camada HTTP e sustentadas por testes.

## Escopo Inicial

O escopo inicial cobre:

- cardapio fixo definido no desafio tecnico
- pedidos com no maximo um sanduiche, uma batata e um refrigerante
- descontos por combinacao de itens
- persistencia relacional em MySQL desde a primeira versao
- ambiente local reproduzivel com Docker para facilitar execucao e avaliacao tecnica
- implementacao de todos os requisitos opcionais do desafio para fortalecer a demonstracao tecnica
- frontend em Blazor consumindo a API
- testes automatizados com prioridade para unitarios e cobertura variada de cenarios, incluindo happy path, edge cases e outros casos relevantes

## Prioridades de Entrega

A ordem de implementacao deve respeitar esta prioridade:

1. back-end completo e funcional
2. frontend integrado consumindo a API
3. refinamentos adicionais de demonstracao e qualidade

O frontend faz parte do escopo desde o inicio e deve influenciar as decisoes de arquitetura, mas sua implementacao vem depois da consolidacao do back-end.

## Fora do Escopo Inicial

Para a primeira entrega, nao precisamos priorizar:

- autenticacao e autorizacao
- pagamento
- estoque
- painel administrativo
- multiunidade ou multiloja
- personalizacao de ingredientes

## Resultado Esperado

Ao final da primeira versao, qualquer consumidor da solucao deve conseguir consultar o cardapio, montar um pedido valido e receber valores calculados corretamente, com mensagens claras em cenarios de erro.

O projeto tambem deve poder ser executado com poucos passos por quem estiver avaliando, idealmente subindo API, frontend e banco via Docker.

O sucesso da entrega sera medido principalmente por:

- boa impressao tecnica ao ler a arquitetura e os testes
- arquitetura clara e facil de navegar
- setup simples para execucao local
- documentacao objetiva para avaliacao
- testes confiaveis e relevantes
- demonstracao funcional completa com API e frontend
- execucao local simples, com foco em baixar o repositorio e subir o ambiente com Docker
