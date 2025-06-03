# Feedback - Avaliação Geral

## Front End

### Navegação
  * Pontos positivos:
    - Projeto MVC com views para autenticação, produtos e categorias.
    - Navegação básica operante com rotas definidas.

### Design
  - O design é simples e funcional, adequado para um painel administrativo.

### Funcionalidade
  * Pontos positivos:
    - CRUD de produtos e categorias implementados nas camadas API e MVC.
    - Autenticação com ASP.NET Identity funcional.
    - SQLite configurado corretamente.

  * Pontos negativos:
    - Seed de dados implementado apenas no projeto MVC, deixando a API sem dados iniciais.
    - Não há criação da entidade `Vendedor` no momento da criação do usuário ou em outro momento, ferindo o requisito do escopo.

## Back End

### Arquitetura
  * Pontos positivos:
    - Separação de projetos (API, Domain, Infra, MVC) implementada.

  * Pontos negativos:
    - O projeto introduz complexidade desnecessária ao separar Domain e Infra, considerando a simplicidade do escopo, uma camada `Core` compartilhada entre API e MVC resolveria a questão.
    - Configurações como `JwtSettings` estão modeladas como entidade de domínio, o que é indevido.

### Funcionalidade
  * Pontos positivos:
    - Operações básicas de autenticação e CRUD funcionam corretamente.
  
  * Pontos negativos:
    - Falta de integração entre registro de usuário e criação de `Vendedor`.

### Modelagem
  * Pontos positivos:
    - Entidades mapeadas com os campos necessários.

  * Pontos negativos:
    - Uso de data annotations para validações nas entidades do domínio compromete a separação de responsabilidades, como foi adotado o conceito de domínio, esperava-se uma validação mais robusta.

## Projeto

### Organização
  * Pontos positivos:
    - Projeto organizado em múltiplos diretórios, com uso do `.sln` na raiz.
    - Documentação presente com `README.md` e `FEEDBACK.md`.

### Documentação
  * Pontos positivos:
    - README.md presente com estruturação básica.
    - Swagger implementado na API.

### Instalação
  * Pontos positivos:
    - SQLite configurado corretamente.

  * Pontos negativos:
    - Seed de dados não está disponível na API, o que impede testes diretos dessa camada.

---

# 📊 Matriz de Avaliação de Projetos

| **Critério**                   | **Peso** | **Nota** | **Resultado Ponderado**                  |
|-------------------------------|----------|----------|------------------------------------------|
| **Funcionalidade**            | 30%      | 6        | 1,8                                      |
| **Qualidade do Código**       | 20%      | 7        | 1,4                                      |
| **Eficiência e Desempenho**   | 20%      | 8        | 1,6                                      |
| **Inovação e Diferenciais**   | 10%      | 7        | 0,7                                      |
| **Documentação e Organização**| 10%      | 10       | 1,0                                      |
| **Resolução de Feedbacks**    | 10%      | 7        | 0,7                                      |
| **Total**                     | 100%     | -        | **7,2**                                  |

## 🎯 **Nota Final: 7,2 / 10**
