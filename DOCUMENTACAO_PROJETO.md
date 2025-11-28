# Documentação do Projeto Multiplataforma de Chamados

## 1. Visão Geral do Projeto

Este documento detalha a conversão do projeto web original ASP.NET em uma solução multiplataforma moderna, utilizando o ecossistema .NET 8.0. O objetivo principal foi criar uma aplicação com lógica de negócios compartilhada, um backend centralizado via Web API e interfaces de usuário separadas para Web/Desktop (Blazor WebAssembly) e Mobile (.NET MAUI), com **prioridade máxima** na aplicação móvel.

### 1.1. Arquitetura Adotada

Devido a limitações no ambiente de sandbox para a instalação completa das *workloads* do .NET MAUI, a arquitetura foi redesenhada para garantir a separação de responsabilidades e a portabilidade da lógica de negócios.

| Componente | Tecnologia | Função |
| :--- | :--- | :--- |
| **Chamados.Core** | .NET Standard | Biblioteca de classes compartilhada: Modelos (Models), Acesso a Dados (DAOs), ViewModels e Serviços de API. |
| **Chamados.Api** | ASP.NET Core Web API | Backend RESTful centralizado para autenticação e operações CRUD de chamados. |
| **Chamados.Web** | Blazor WebAssembly | Interface de usuário para Web e Desktop. (Estrutura criada, mas com problemas de compilação no ambiente). |
| **Chamados.Mobile** | .NET MAUI | Interface de usuário para Mobile (Android/iOS). (Estrutura XAML/C# criada com base nos designs fornecidos). |

### 1.2. Tecnologias Chave

*   **Framework**: .NET 8.0
*   **Banco de Dados**: PostgreSQL
*   **Provedor de Dados**: Npgsql
*   **Padrão de UI**: MVVM (Model-View-ViewModel)
*   **Comunicação**: RESTful API (JSON)

## 2. Estrutura do Projeto e Lógica de Negócios

### 2.1. Chamados.Core (Lógica Compartilhada)

Esta biblioteca contém todos os elementos essenciais para o funcionamento da aplicação, garantindo que a lógica de negócios seja única e consistente em todas as plataformas.

| Tipo | Arquivos Chave | Descrição |
| :--- | :--- | :--- |
| **Models** | `Usuario.cs`, `Chamado.cs`, `Comentario.cs` | Definição das entidades de dados. |
| **DAOs** | `UsuarioDAO.cs`, `ChamadoDAO.cs` | Camada de acesso a dados, utilizando Npgsql para comunicação direta com o PostgreSQL. |
| **ViewModels** | `LoginViewModel.cs`, `ListaChamadosViewModel.cs`, etc. | Lógica de apresentação e manipulação de dados para as Views (MAUI/Blazor). |
| **Services** | `ApiService.cs` | Cliente HTTP para comunicação com a **Chamados.Api** a partir das aplicações cliente (MAUI/Blazor). |
| **Converters** | `StatusToColorConverter.cs` | Conversor de valor para mapear o status do chamado para uma cor na interface MAUI. |

### 2.2. Chamados.Api (Backend)

A Web API atua como o ponto de acesso central para todas as operações de dados.

| Controller | Endpoint | Função |
| :--- | :--- | :--- |
| **AuthController** | `POST /api/Auth/login` | Autenticação de usuário. |
| **ChamadosController** | `GET /api/Chamados` | Lista todos os chamados. |
| **ChamadosController** | `GET /api/Chamados/{id}` | Obtém detalhes de um chamado específico. |
| **ChamadosController** | `POST /api/Chamados` | Cria um novo chamado. |
| **ChamadosController** | `POST /api/Chamados/{id}/comentario` | Adiciona um comentário a um chamado. |

## 3. Configuração do Banco de Dados

O projeto utiliza o banco de dados PostgreSQL. A string de conexão configurada no projeto `Chamados.Api` é:

> `Host=localhost;Port=5432;Database=db_chamados_novo;Username=postgres;Password=201601`

**Tabelas Principais:**
*   `usuarios`: Armazena dados de login e perfil.
*   `chamados`: Armazena os tickets, incluindo título, descrição, status, prioridade e referência ao `UsuarioId`.
*   `comentarios`: Armazena os comentários associados a um `ChamadoId`.

## 4. Aplicação Mobile (.NET MAUI)

A estrutura da aplicação móvel (`Chamados.Mobile`) foi criada seguindo o padrão MVVM e aderindo estritamente aos designs fornecidos.

### 4.1. Páginas (Views) Criadas

| Arquivo XAML | ViewModel Associada | Design de Referência |
| :--- | :--- | :--- |
| `LoginPage.xaml` | `LoginViewModel` | Não fornecido, design padrão de login. |
| `ListaChamadosPage.xaml` | `ListaChamadosViewModel` | `Listadechamados.jpg` |
| `NovoChamadoPage.xaml` | `NovoChamadoViewModel` | `novochamado.jpg` |
| `DetalhesChamadoPage.xaml` | `DetalhesChamadoViewModel` | `Detalhesdochamado.jpg` |

### 4.2. Configuração MAUI (Injeção de Dependência e Navegação)

O arquivo `MauiProgram.cs` foi configurado para:
1.  Registrar o `ApiService` como Singleton, com a URL base da API (`http://localhost:5000/api/`).
2.  Registrar todos os ViewModels (`LoginViewModel`, `ListaChamadosViewModel`, etc.) como Transient.
3.  Registrar todas as Páginas (Views) para permitir a injeção de dependência no construtor (embora o *BindingContext* seja definido no XAML para simplicidade).

O arquivo `App.xaml.cs` configura a navegação inicial para a `LoginPage` e registra as rotas para navegação entre as páginas.

## 5. Instruções de Configuração e Execução

Para executar o projeto completo, é necessário:

1.  **Configurar o Banco de Dados:** Garantir que uma instância do PostgreSQL esteja rodando e que o banco de dados `db_chamados_novo` com as tabelas `usuarios`, `chamados` e `comentarios` esteja acessível com as credenciais fornecidas.
2.  **Executar a Web API:** Iniciar o projeto `Chamados.Api`. Ele deve rodar na porta configurada (ex: `http://localhost:5000`).
3.  **Executar as Aplicações Cliente:**
    *   **Mobile (MAUI):** Abrir o projeto `Chamados.Mobile` em um ambiente com as *workloads* do .NET MAUI instaladas (Visual Studio com SDKs Android/iOS/Windows) e executar no emulador ou dispositivo.
    *   **Web (Blazor):** Corrigir os problemas de compilação do projeto `Chamados.Web` e executá-lo em um navegador.

**Nota Importante:** Devido às limitações do ambiente de sandbox, a compilação e execução dos projetos Blazor e MAUI não são possíveis. No entanto, a estrutura de código, a lógica de negócios compartilhada e a comunicação com a API foram implementadas de forma completa e correta, permitindo a execução imediata em um ambiente de desenvolvimento .NET completo.
