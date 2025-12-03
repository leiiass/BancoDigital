# Banco Digital - API GraphQL (.NET 8)

API de simulação de banco digital, construída com **.NET 8**, **GraphQL (HotChocolate)**, **Entity Framework Core** e **MySQL**.

A API expõe operações para:

- Criar conta
- Consultar saldo
- Realizar depósitos
- Realizar saques (com validação de saldo insuficiente)

---

## ✅ Pré-requisitos

- **.NET SDK 8.0+**  
- **MySQL** instalado e rodando localmente
- Um usuário de banco com permissão para criar banco e tabelas

---

## ⚙️ Configuração do banco de dados

1. No MySQL, crie um banco (ou apenas deixe que a aplicação crie ao subir, se o usuário tiver permissão):

   ```sql
   CREATE DATABASE bancodigital;
   
2. No projeto BancoDigital.Api, localize o arquivo appsettings.json e ajuste a connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=PORTA;Database=bancodigital;User=SEU_USUARIO;Password=SUA_SENHA;"
  }
}
``` 
- PORTA: porta do MySQL (geralmente 3306)
- SEU_USUARIO: usuário do MySQL
- SUA_SENHA: senha do usuário

> Ao subir a aplicação, o EF Core aplica as migrations automaticamente e cria as tabelas necessárias no banco (se ainda não existirem).
  
## ▶️ Como rodar o projeto

Você pode rodar via Visual Studio.

Visual Studio:
- Abra a solução no Visual Studio.
- Defina o projeto BancoDigital.Api como Startup Project.
- Compile a solução.
- Rode a aplicação (F5 ou Ctrl+F5).

## 🧪 Testando a API (GraphQL / Nitro)
Com a aplicação rodando, acesse no navegador:
```text
https://localhost:PORTA/graphql
```
> Você verá a IDE do GraphQL (Nitro / Banana Cake Pop).

> É por ela que você vai enviar as queries e mutations.

## ✅ Testes automatizados

O projeto inclui testes unitários com xUnit para o serviço de conta (ContaService), cobrindo cenários como:
- Criação de conta
- Depósito válido e inválido
- Saque com saldo suficiente
- Saque com saldo insuficiente
- Conta inexistente

## 🔧 Estrutura do projeto

- BancoDigital.Api – camada de apresentação (GraphQL)
- BancoDigital.Servicos – regras de negócio (ContaService)
- BancoDigital.Dominio – entidades de domínio (ex.: Conta) e interfaces
- BancoDigital.Infraestrutura – EF Core / MySQL / repositórios
- BancoDigital.Testes – testes unitários com xUnit
