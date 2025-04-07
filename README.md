
# **BeautyStore - Aplicação de loja de beleza com MVC e API RESTful**

[BeautyStore](https://github.com/TatiMachado/BeautyStore)

## **1. Apresentação**

Bem-vindo ao repositório do projeto  **BeautyStore**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo  **Introdução ao Desenvolvimento ASP.NET Core**. O objetivo principal desenvolver uma aplicação de de uma loja de beleza que permite aos usuários criar, editar, visualizar e excluir categorias e produtos, tanto através de uma interface web utilizando MVC quanto através de uma API RESTful. 


### **Autor(es)**

[](https://github.com/desenvolvedor-io/template-repositorio-mba#autores)

-   **Tatiane Siqueira Machado** 

## **2. Proposta do Projeto**

O projeto consiste em:

-   **Aplicação MVC:**  Interface web para permitir crud de produtos e categorias.
-   **API RESTful:**  Exposição dos recursos  para login, bem como CRUD das entidades citadas acima.
-   **Autenticação e Autorização:**  Implementação de controle de acesso.
-   **Acesso a Dados:**  Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**

-   **Linguagem de Programação:**  C#
-   **Frameworks:**
    -   ASP.NET Core MVC
    -   ASP.NET Core Web API
    -   Entity Framework Core
-   **Banco de Dados:**  SQL Server
-   **Autenticação e Autorização:**
    -   ASP.NET Core Identity
    -   JWT (JSON Web Token) para autenticação na API
-   **Front-end:**
    -   Razor Pages/Views
    -   HTML/CSS para estilização básica
-   **Documentação da API:**  Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

-   BeautyStore.MVC/ - Projeto MVC
-   BeautyStore.API/ - API 
-   BeautyStore.Domain/ - Entidades e serviços
-  BeautyStore.Data/ - Modelos de Dados e Configuração do EF Core
-   README.md - Arquivo de Documentação do Projeto
-   FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
-   .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

-   **CRUD para Categorias e Produtos:**  Permite criar, editar, visualizar e excluir categorias e produtos.
-   **Autenticação e Autorização:**  Diferenciação entre usuários comuns e administradores.
-   **API RESTful:**  Exposição de endpoints para operações CRUD via API.
-   **Documentação da API:**  Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

-   .NET SDK 9.0 ou superior
-   SQL Server
-   Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
-   Git

### **Passos para Execução**

1.  **Clone o Repositório:**
    
    -   `git clone https://github.com/TatiMachado/BeautyStore.git`
    -   `cd BeautyStore`
 
2.  **Configuração do Banco de Dados:**
    
    -   Os arquivos  `appsettings.json`,  já estão configurados.
    -  Execução no modo **Development**:  os dados serão gravados no sqlite no diretório BeautyStore.Infra.Data\DbSqlLite , onde nesse modo de execução as tabelas serão populadas automaticamente.
    - Execução no modo **Production**, configure a string de conexão do SQL Server e rode as migrations necessárias para criação das tabelas referente ao identity e para as demais tabelas.
    
3.  **Executar a Aplicação MVC:**
    
    -   No visualStudio, selecione o projeto BeautyStore.MVC, modo Development e clique no botão executar.  
    -   Acesse a aplicação em:  [https://localhost:7227](https://localhost:7227)
4.  **Executar a API:**
    
    -   No visualStudio, selecione o projeto BeautyStore.API, modo Development e cliquei no botão executar.
    -   Acesse a documentação da API em:  [https://localhost:7197/swagger](https://localhost:7197/swagger)

## **7. Instruções de Configuração**

-   **JWT para API:**  As chaves de configuração do JWT estão no  `appsettings.json`.
-   **Migrações do Banco de Dados:**  As migrações são gerenciadas pelo Entity Framework Core.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

[https://localhost:7197/swagger](https://localhost:7197/swagger)

## **9. Avaliação**

-   Este projeto é parte de um curso acadêmico e não aceita contribuições externas.
-   Para feedbacks ou dúvidas utilize o recurso de Issues
-   O arquivo  `FEEDBACK.md`  é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
