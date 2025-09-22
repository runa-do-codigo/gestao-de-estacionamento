# Gestão de Estacionamento

## Introdução

O projeto Gestão de Estacionamento tem como objetivo gerenciar hóspedes, veículos, tickets, vagas e faturamentos de forma prática, organizada e eficiente.

Desenvolvido como projeto acadêmico, a aplicação utiliza C# com .NET 8.0, com interface web em ASP.NET MVC, seguindo uma arquitetura em camadas, garantindo separação de responsabilidades, manutenção facilitada e escalabilidade.

A solução permite que empresas de estacionamento controlem entradas e saídas de veículos, associem tickets a vagas, gerenciem faturamentos e otimizem o atendimento aos clientes.

---

## Tecnologias

<p align="left"> 
  <img src="https://skillicons.dev/icons?i=cs" height="50"/> 
  <img src="https://skillicons.dev/icons?i=dotnet" height="50"/> 
  <img src="https://skillicons.dev/icons?i=visualstudio" height="50"/> 
  <img src="https://skillicons.dev/icons?i=html" height="50"/> 
  <img src="https://skillicons.dev/icons?i=css" height="50"/> 
  <img src="https://skillicons.dev/icons?i=js" height="50"/> 
  <img src="https://skillicons.dev/icons?i=bootstrap" height="50"/> 
  <img src="https://skillicons.dev/icons?i=git" height="50"/> 
  <img src="https://skillicons.dev/icons?i=github" height="50"/> 
  <img src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/postgresql/postgresql-original.svg" height="45"/> 
  <img src="https://skillicons.dev/icons?i=docker" height="50"/> 
  <img src="https://cdn.jsdelivr.net/gh/simple-icons/simple-icons/icons/newrelic.svg" height="40"/> 
</p>

---

## Funcionalidades

### Para Empresas (Admin)
- **Hóspedes:** cadastrar, editar, excluir e visualizar informações (nome, CPF).
- **Veículos:** gerenciar veículos vinculados aos hóspedes (placa, modelo, cor, observações).
- **Tickets:** registrar entradas e saídas de veículos, associando tickets a veículos, vagas e faturamentos.
- **Vagas:** criar, atualizar e monitorar vagas disponíveis no estacionamento.
- **Faturamento:** registrar e gerar relatórios financeiros por período.

### Para Clientes
- Consultar status de vagas (quando implementado para painel de clientes ou app).

---

## Como utilizar

1. Clone o repositório ou baixe o código fonte.
2. Abra o terminal ou o prompt de comando e navegue até a pasta raiz
3. Utilize o comando abaixo para restaurar as dependências do projeto.

```
dotnet restore
```

4. Em seguida, compile a solução utilizando o comando:
   
```
dotnet build --configuration Release
```

5. Para executar o projeto compilando em tempo real
   
```
dotnet run --project GestaoDeEstacionamento.ConsoleApp
```

6. Para executar o arquivo compilado, navegue até a pasta `./GestaoDeEstacionamento.WebApi/bin/Release/net8.0/` e execute o arquivo:
   
```
GestaoDeEstacionamento.ConsoleApp.exe
```

## Requisitos

- .NET SDK (recomendado .NET 8.0 ou superior) para compilação e execução do projeto.

- Visual Studio 2022 ou superior (opcional, para desenvolvimento).
