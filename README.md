# 📌 **Projeto UVV – Fintech**  
Sistema Bancário desenvolvido como projeto acadêmico para a disciplina de **POO II** da Universidade Vila Velha (UVV). O objetivo é implementar um sistema seguindo rigorosamente os princípios de **Clean Code**, **SOLID** e o padrão arquitetural **MVC**, simulando operações reais de um ambiente bancário.

---

## 🧩 **Sobre o Projeto**
Este projeto consiste na criação de um sistema bancário completo, com funcionalidades para:

- Cadastro e gerenciamento de **clientes**  
- Criação e manutenção de **contas bancárias** (corrente e poupança)  
- Execução de **transações** como depósito, saque e transferência  
- Persistência de dados por meio de repositórios e integração com banco  
- Interface gráfica criada com **XAML/WPF**  
- Controllers responsáveis por conectar Views, ViewModels, Interfaces e Repositórios  

O sistema foi projetado aplicando:

- **Clean Code** → código legível, funções pequenas e responsabilidades claras  
- **SOLID** → foco em desacoplamento e extensibilidade  
- **MVC** → separação clara entre interface (View), lógica de controle (Controller) e acesso a dados (Model/Repository)  

---

## 🗂 **Arquitetura do Projeto**

A estrutura segue esse padrão principal:

```bash
/
├── Controllers/
│   ├── ClienteController.cs
│   ├── ContaController.cs
│   └── TransacaoController.cs
│
├── Repository/
│   ├── ClienteRepository.cs
│   ├── ContaCorrenteRepository.cs
│   ├── ContaPoupancaRepository.cs
│   ├── TransacaoRepository.cs
│   └── Interfaces/
│       ├── IClienteRepository.cs
│       ├── IContaRepository.cs
│       └── ITransacaoRepository.cs
│
├── ViewModels/
│   ├── ClienteViewModel.cs
│   ├── ContaViewModel.cs
│   └── TransacaoViewModel.cs
│
├── Views/
│   └── (Arquivos XAML das telas do sistema)
│
├── DiagramaClasses.png
├── DiagramaObjetos.png
├── Wireframe.png
└── README.md
```



---

## 📌 **Funcionalidades Principais**

### 👤 **Clientes**
- Cadastro de novos clientes  
- Edição e listagem  
- Validações básicas  

### 🏦 **Contas (Corrente e Poupança)**
- Criação de contas com tipos diferentes  
- Listagem e consulta  
- Regras individuais aplicadas em cada tipo  

### 💸 **Transações**
- Depósito  
- Saque  
- Transferência entre contas  
- Registro das transações no repositório  

---

## 🧠 **Padrões e Princípios Aplicados**

- **Single Responsibility Principle (SRP)**  
  Controllers, repositórios e viewmodels possuem responsabilidade única.

- **Open/Closed Principle (OCP)**  
  Contas e repositórios podem ser estendidos sem modificar código existente.

- **Dependency Inversion (DIP)**  
  Controllers dependem de **interfaces**, não de implementações concretas.

- **Separação de camadas (MVC)**  
  Views → Controllers → Interfaces → Repositórios → Banco  

---

## 🔗 **Links Importantes**

### 📒 **Trello (Gerenciamento do Projeto)**  
👉 *adicione aqui o link do Trello*  

### 📘 **Diagrama de Classes**  
Arquivo na raiz do projeto:  
👉 *adicione aqui o link do arquivo ou imagem*  

### 📗 **Diagrama de Objetos**  
Arquivo na raiz do projeto:  
👉 *adicione aqui o link do arquivo ou imagem*  

### 🖼 **Wireframe das Telas**  
Arquivo na raiz do projeto:  
👉 *adicione aqui o link do arquivo ou imagem*  

---

## ▶️ **Como Executar o Projeto**

1. Abra o projeto no **Visual Studio**
2. Restaure dependências (se necessário)
3. Execute a solução

---

## 👥 **Equipe**

- **Teo** – Interface e implementação visual  
- **Pablo** – Controllers, integração MVC e fluxo entre camadas  
- **Gabriel** – Repositórios, acesso a dados e banco
