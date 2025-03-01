# Capital Gains Calculator

Este projeto � uma solu��o para o desafio "Capital Gains" do Nubank. O programa calcula o imposto sobre ganhos de capital para opera��es de compra e venda de a��es, conforme especificado no documento do desafio.

## Decis�es T�cnicas e Arquiteturais

- **Linguagem e Plataforma**: Desenvolvido em C# usando o .NET SDK (vers�o 8.0). Escolhi C# devido � sua forte tipagem, suporte nativo para manipula��o de JSON, e facilidade para criar testes unit�rios.
- **Arquitetura**:
  - O projeto segue uma arquitetura em camadas para manter a separa��o de responsabilidades:
    - **CapitalGains.Domain**: Cont�m modelos (`Operations`, `TaxResult`), enums (`OperationType`), e interfaces (`ITaxService`).
    - **CapitalGains.Infrastructure**: Cont�m a l�gica de entrada/sa�da (`JsonInputParser` para desserializa��o de JSON).
    - **CapitalGains.Application**: Cont�m a l�gica de neg�cios:
      - `TaxCalculatorService`: Calcula o imposto para uma �nica opera��o.
      - `TaxService`: Processa uma lista de opera��es, gerenciando o estado dentro de uma simula��o.
      - `ProcessOperationsUseCase`: Coordena a execu��o.
    - **CapitalGains.CLI**: Cont�m o ponto de entrada (`Program.cs`), que lida com entrada/sa�da via `stdin`/`stdout`.
    - **CapitalGains.Tests**: Cont�m testes unit�rios organizados em classes separadas.
  - Essa estrutura facilita a manuten��o e extens�o do programa (ex.: adicionar novos tipos de opera��o ou regras de c�lculo).
- **Estado**: O estado (`weightedAverage`, `totalShares`, `accumulatedLoss`) � gerenciado em mem�ria dentro do m�todo `TaxService.CalculateTaxes`, sendo reiniciado para cada linha de entrada (simula��o), conforme especificado.
- **Arredondamento**: O pre�o m�dio ponderado (`weightedAverage`) e o imposto (`tax`) s�o arredondados para duas casas decimais usando `Math.Round(value, 2)`.
- **Docker**: Inclu� um `Dockerfile` e `docker-compose.yml` para facilitar a execu��o em ambientes Unix/Mac, conforme permitido pelo documento ("Builds conteinerizadas s�o bem-vindas").

## Justificativa para Bibliotecas

- **System.Text.Json**: Usado para desserializa��o/serializa��o de JSON. Escolhi essa biblioteca por ser parte do .NET SDK, leve, e atender �s necessidades do desafio (parsing de JSON e sa�da em formato JSON).
- **xUnit**: Usado para testes unit�rios. � uma biblioteca open source amplamente usada em projetos .NET, com suporte para asser��es e execu��o de testes no Visual Studio e via `dotnet test`.

## Como Compilar e Executar o Projeto

### Pr�-requisitos
- .NET SDK (vers�o 8.0 ou superior) instalado.
- Docker.

### Passos para Compilar e Executar (Sem Docker)

1. Navegue at� a pasta do projeto:
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
4. Insira as opera��es linha por linha no terminal, pressionando Enter ap�s cada linha. Por exemplo:

[{"operation":"buy", "unit-cost":10.00, "quantity": 10000},{"operation":"sell", "unit-cost":20.00, "quantity": 5000}]

**Pressione Enter, e o programa retornar� os impostos para aquela linha:**

[{"tax":0.00},{"tax":10000.00}]

### Passos para Compilar e Executar (Com Docker)

1. Certifique-se de que o Docker e o Docker Compose est�o instalados.
2. Navegue at� a pasta do projeto (onde est�o **Dockerfile** e **docker-compose.yml**):
    ```bash
    cd caminho/para/CapitalGains.CLI
    ```
3. Construa a imagem Docker:
    ```bash
    docker-compose build
    ```
4. Execute o cont�iner:
    ```bash
    docker-compose run capitalgains.cli
    ```
### Executando os Testes

### Pr�-requisitos

- .NET SDK (vers�o 8.0 ou superior) instalado.
- O projeto de testes **(CapitalGains.Tests)** cont�m os testes unit�rios localizados na pasta UnitTests.

### Passos para Executar os Testes

1. Navegue at� a pasta do projeto de testes:
    ```bash
    cd caminho/para/CapitalGains.Tests
    ```
2. Execute os testes usando o comando dotnet test:
    ```bash
    dotnet test
    ```
3. Os resultados dos testes ser�o exibidos no terminal, indicando quais testes passaram ou falharam.

### Estrutura dos Testes

*UnitTests:* Cont�m testes unit�rios organizados em classes separadas:

**JsonInputParserTests.cs:** Testes para o parser de entrada JSON.
**TaxCalculatorServiceTests.cs:** Testes para o c�lculo de impostos em opera��es individuais.
**TaxServiceTests.cs:** Testes para o processamento de uma lista de opera��es.
**ProcessOperationsUseCaseTests.cs:** Testes para o caso de uso principal.