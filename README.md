# ContaFinanceira
API Rest em .Net Core 5.0 para abertura de conta financeira para pessoa física e ou jurídica utilizando banco de dados SQLServer.

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
1. Clone o repositório do git ou faça download do projeto
2. Abra a solução no Visual Studio 2019 ou posterior
3. Abra o arquivo ContasFinanceiras.API > appsettings.json e altere o SqlServerConnection com suas informações
4. Clique em Ferramentas > Gerenciador de Pacotes do Nuget > Console do Gerenciador de Pacotes
5. Selecione o projeto ContasFinanceiras.Persistance para Inicialização
6. Rode o comando "update-database" e verifique se o banco de dados foi criado com sucesso
7. Certifique-se que o projeto de inicialização é o ContasFinanceiras.API (destacado em negrito). Se estiver, pule para o passo 9
8. Clique com o botão direito do mouse sobre o projeto ContasFinanceiras.API e selecione a opção Definir como Projeto de Inicialização
9. No botão de iniciar, selecione o projeto ContasFinanceiras.API
10. Pronto! O navegador deve abrir com o [Swagger para teste](#swagger)
11. Siga o [passo a passo para testar a aplicação](#como-testar-a-aplicação)

## Publicação local
## Docker

# Como testar a aplicação

## Postman
Baixe a collection aqui ou acesso o [link](https://www.getpostman.com/collections/5ea39cbebdd05afc37d2).

## Swagger
