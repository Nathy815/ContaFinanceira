# ContaFinanceira
API para abertura de conta financeira para pessoa física e ou jurídica.

## Estrutura de Pastas
| **Pasta** | **Descrição** |
| :---: | :---: |
| **ContaFinanceira.API** | Contém os controllers da aplicação e configurações da aplicação (injeções de dependência, Cors, etc) |
| **ContaFinanceira.Application** | Contém todos os serviços e validações do projeto, utilizados pelos controllers |
| **ContaFinanceira.Domain** | Contém todas as entidades (de base de dados), requests, responses, enumns e interfaces do projeto |
| **ContaFinanceira.Persistance** | Contém a camada de conexão com o banco de dados, com contexto e repositórios |
| **ContaFinanceira.Tests** | Contém todos os testes unitários dos controllers, services e repositórios do projeto |
| **ContaFinanceira.Util** | Contém itens úteis para execução dos serviços de Application, como criptografia |

## Recursos

A ContaFinanceira disponibiliza uma API Rest com os seguintes recursos:
- [**Agências**](#agências)
- [**Clientes**](#clientes)
- [**Contas**](#contas)
- [**Tipos Pessoas**](#tipos-pessoas)
- [**Transações**](#transações)

Saiba mais sobre [**como utilizar a aplicação**](#como-utilizar-a-aplicação) e [**como testá-la**](#como-testar-a-aplicação)

# Agências
Listagem de agências bancárias disponíveis.
Necessário informar a agência na criação de conta.

# Clientes
Autenticação de usuários na plataforma.
Para um usuário se autenticar é preciso, primeiro, criar uma nova conta para ele.
A autenticação é necessária para realizar qualquer tipo de transação (saque ou depósito).

# Contas
Criação de nova conta bancária para pessoa física ou jurídica.
Caso deseje, no momento da abertura da conta, o usuário pode fazer um depósito inicial.

# Tipos Pessoas
Listagem de tipos de pessoas disponíveis.
Necessário informar a agência na criação de conta.

# Transações
Realização de uma determinada transação na conta, seja ela um saque ou um depósito.
Necessário já ter uma conta criada e estar autenticado na plataforma.
Caso o usuário deseje fazer um saque, o saldo da conta precisa ser verificado.

# Como utilizar a aplicação

## Debug com Visual Studio
## Publicação local
## Docker

# Como testar a aplicação

## Postman
Baixe a collection aqui ou acesso o [link](https://www.getpostman.com/collections/5ea39cbebdd05afc37d2).

## Swagger
