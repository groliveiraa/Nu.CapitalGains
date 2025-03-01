# Capital Gains Calculator

Este projeto é uma solução para o desafio "Capital Gains" do Nubank. O programa calcula o imposto sobre ganhos de capital para operações de compra e venda de ações, conforme especificado no documento do desafio.

## Decisões Técnicas e Arquiteturais

- **Linguagem e Plataforma**: Desenvolvido em C# usando o .NET SDK (versão 8.0). Escolhi C# devido à sua forte tipagem, suporte nativo para manipulação de JSON, e facilidade para criar testes unitários.
- **Arquitetura**:
  - O projeto segue uma arquitetura em camadas para manter a separação de responsabilidades:
    - **CapitalGains.Domain**: Contém modelos (`Operations`, `TaxResult`), enums (`OperationType`), e interfaces (`ITaxService`).
    - **CapitalGains.Infrastructure**: Contém a lógica de entrada/saída (`JsonInputParser` para desserialização de JSON).
    - **CapitalGains.Application**: Contém a lógica de negócios:
      - `TaxCalculatorService`: Calcula o imposto para uma única operação.
      - `TaxService`: Processa uma lista de operações, gerenciando o estado dentro de uma simulação.
      - `ProcessOperationsUseCase`: Coordena a execução.
    - **CapitalGains.CLI**: Contém o ponto de entrada (`Program.cs`), que lida com entrada/saída via `stdin`/`stdout`.
    - **CapitalGains.Tests**: Contém testes unitários organizados em classes separadas.
  - Essa estrutura facilita a manutenção e extensão do programa (ex.: adicionar novos tipos de operação ou regras de cálculo).
- **Estado**: O estado (`weightedAverage`, `totalShares`, `accumulatedLoss`) é gerenciado em memória dentro do método `TaxService.CalculateTaxes`, sendo reiniciado para cada linha de entrada (simulação), conforme especificado.
- **Arredondamento**: O preço médio ponderado (`weightedAverage`) e o imposto (`tax`) são arredondados para duas casas decimais usando `Math.Round(value, 2)`.
- **Docker**: Incluí um `Dockerfile` e `docker-compose.yml` para facilitar a execução em ambientes Unix/Mac, conforme permitido pelo documento ("Builds conteinerizadas são bem-vindas").

## Justificativa para Bibliotecas

- **System.Text.Json**: Usado para desserialização/serialização de JSON. Escolhi essa biblioteca por ser parte do .NET SDK, leve, e atender às necessidades do desafio (parsing de JSON e saída em formato JSON).
- **xUnit**: Usado para testes unitários. É uma biblioteca open source amplamente usada em projetos .NET, com suporte para asserções e execução de testes no Visual Studio e via `dotnet test`.

## Como Compilar e Executar o Projeto

### Pré-requisitos
- .NET SDK (versão 8.0 ou superior) instalado.
- Docker.

### Passos para Compilar e Executar (Sem Docker)

1. Navegue até a pasta do projeto:
    ```bash
    cd caminho/para/CapitalGains.CLI
    ```
2. Compile o projeto:
    ```bash
    dotnet build
    ```
3. Execute o programa:
    ```bash
    dotnet run
    ```
4. Insira as operações linha por linha no terminal, pressionando Enter após cada linha. Por exemplo:

[{"operation":"buy", "unit-cost":10.00, "quantity": 10000},{"operation":"sell", "unit-cost":20.00, "quantity": 5000}]

**Pressione Enter, e o programa retornará os impostos para aquela linha:**

[{"tax":0.00},{"tax":10000.00}]

### Passos para Compilar e Executar (Com Docker)

1. Certifique-se de que o Docker e o Docker Compose estão instalados.
2. Navegue até a pasta do projeto (onde estão **Dockerfile** e **docker-compose.yml**):
    ```bash
    cd caminho/para/CapitalGains.CLI
    ```
3. Construa a imagem Docker:
    ```bash
    docker-compose build
    ```
4. Execute o contêiner:
    ```bash
    docker-compose run capitalgains.cli
    ```
### Executando os Testes

### Pré-requisitos

- .NET SDK (versão 8.0 ou superior) instalado.
- O projeto de testes **(CapitalGains.Tests)** contém os testes unitários localizados na pasta UnitTests.

### Passos para Executar os Testes

1. Navegue até a pasta do projeto de testes:
    ```bash
    cd caminho/para/CapitalGains.Tests
    ```
2. Execute os testes usando o comando dotnet test:
    ```bash
    dotnet test
    ```
3. Os resultados dos testes serão exibidos no terminal, indicando quais testes passaram ou falharam.

### Estrutura dos Testes

*UnitTests:* Contém testes unitários organizados em classes separadas:

- **JsonInputParserTests.cs:** Testes para o parser de entrada JSON.
- **TaxCalculatorServiceTests.cs:** Testes para o cálculo de impostos em operações individuais.
- **TaxServiceTests.cs:** Testes para o processamento de uma lista de operações.
- **ProcessOperationsUseCaseTests.cs:** Testes para o caso de uso principal.
