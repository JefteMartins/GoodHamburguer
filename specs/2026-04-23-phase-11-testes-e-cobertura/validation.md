# Validation

## Criterios de Sucesso

A `Fase 11 - Testes e Cobertura` sera considerada bem-sucedida quando:

- as suites automatizadas forem ampliadas de forma perceptivel e orientada a risco
- existirem novos cenarios cobrindo happy path, edge cases e falhas relevantes
- a cobertura for medida de forma repetivel
- a CI produzir evidencia clara de cobertura
- a cobertura minima global de `70%` for atingida
- o aumento de cobertura vier acompanhado de testes uteis, e nao de inflacao artificial

## Validacao Automatizada

Deve haver evidencias automatizadas em quatro niveis.

### Build da Solucao

- `dotnet build GoodHamburguer.slnx -v minimal` sem erros

### Suites de Teste

- `tests/GoodHamburguer.UnitTests` passando
- `tests/GoodHamburguer.IntegrationTests` passando
- `tests/GoodHamburguer.Blazor.Tests` passando

### Cobertura

- os testes devem ser executados com coleta de cobertura habilitada
- deve existir evidencia consolidada de cobertura para a solucao
- a cobertura minima global deve ser validada contra a meta de `70%`

### Pipeline

- a pipeline principal deve executar build, testes e cobertura
- a pipeline deve falhar quando a validacao objetiva definida para a fase nao for atendida

## Validacao Manual

Checks manuais esperados:

- revisar os relatorios ou sumarios de cobertura produzidos
- confirmar que as suites adicionadas cobrem lacunas reais e nao apenas caminhos triviais
- revisar que a CI ficou compreensivel para avaliacao do projeto
- verificar que a ampliacao dos testes nao tornou o fluxo local excessivamente opaco

## Evidencias Esperadas Antes do Merge

Antes do merge, deve existir evidencia de:

- build da solution sem erros
- testes unitarios passando
- testes de integracao passando
- testes Blazor passando
- cobertura consolidada acima do minimo global
- CI atualizada para reproduzir essas evidencias
- alinhamento com `specs/mission.md`
- alinhamento com `specs/tech-stack.md`
- alinhamento com `specs/roadmap.md`

## Nao Concluir Esta Fase Se

Nao considerar esta fase pronta se:

- a cobertura for apenas estimada manualmente
- a cobertura nao estiver integrada ao fluxo automatizado
- a meta minima nao for atingida
- houver aumento de percentual sem protecao real de fluxos criticos
- uma das suites principais permanecer claramente subaproveitada
- a CI continuar sem evidenciar cobertura de forma objetiva

