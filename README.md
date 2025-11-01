# IASB

## About the Project
`IASB` é um projeto versátil e focado em auxiliar as operações de sonoplastia em igrejas. Ele foi desenvolvido para simplificar o gerenciamento e a reprodução de conteúdos multimídia essenciais para os cultos e atividades congregacionais. As principais funcionalidades do sistema incluem:

*   **Gerenciamento de Arquivos**: Uma ferramenta eficiente para organizar, armazenar e acessar arquivos e documentos relacionados às atividades da igreja.
*   **Apresentação de Vídeos Semanais**: Suporte dedicado para os vídeos "Provai e Vede" e "Informativo", permitindo que sejam facilmente acessados, baixados e reproduzidos.
*   **Reprodutor de YouTube sem Anúncios**: Um player integrado de YouTube que proporciona a reprodução de vídeos sem interrupções comerciais, ideal para uso durante os serviços.

O projeto busca oferecer uma solução robusta e intuitiva para automatizar fluxos de trabalho, integrar sistemas e gerenciar recursos digitais de forma eficaz, com foco em modularidade, escalabilidade e facilidade de manutenção.

## Technologies Used
*   **C#**: Principal linguagem de programação utilizada no desenvolvimento.
*   **.NET**: Framework que oferece as bibliotecas e o ambiente de execução.
*   **Visual Studio 2026**: Ambiente de desenvolvimento integrado (IDE) utilizado para construção e depuração.
*   **ASP.NET Core**: Para a construção de aplicações web e APIs.
*   **MudBlazor**: Componentes UI para construir a interface de usuário.

## Use Cases
`IASB` pode ser efetivamente aplicado em vários cenários, incluindo, mas não se limitando a:
*   **Suporte à Sonoplastia**: Ferramentas dedicadas para sonoplastas gerenciarem e apresentarem conteúdo multimídia durante os serviços.
*   **Reprodução de Conteúdo Educacional/Inspiracional**: Facilita a exibição de vídeos "Provai e Vede" e "Informativo" para a congregação.
*   **Gerenciamento de Mídia da Igreja**: Organização centralizada de arquivos de áudio, vídeo e documentos.
*   **Streaming e Apresentações**: Utilização do player de YouTube integrado para exibir conteúdo relevante sem distrações de anúncios.
*   **Automação de Tarefas**: Otimização do processo de obtenção e disponibilização de conteúdos semanais.

## Getting Started

Para ter uma cópia local do projeto em execução, siga estas etapas simples.

### Prerequisites

Certifique-se de ter o seguinte instalado em sua máquina de desenvolvimento:
*   **.NET SDK**: Versão 8.0 ou posterior.
*   **Visual Studio 2026**: Com as cargas de trabalho necessárias (por exemplo, __desenvolvimento para desktop com .NET__, __desenvolvimento ASP.NET e web__, etc.).
*   [**Docker**](https://www.docker.com/get-started): Para conteinerização (opcional, se estiver usando Docker).

### Installation and Running

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/MauryDev/IASB.git
    ```
2.  **Navegue até o diretório do projeto:**
    ```bash
    cd IASB
    ```
3.  **Abra a solução no Visual Studio 2026:**
    Localize e abra o arquivo `.sln` (por exemplo, `IASB.sln`) no Visual Studio.
4.  **Restaure os pacotes NuGet:**
    O Visual Studio deve restaurar automaticamente os pacotes NuGet necessários. Caso contrário, clique com o botão direito na solução no __Gerenciador de Soluções__ e selecione __Restaurar Pacotes NuGet__.
5.  **Compile o projeto:**
    Vá para __Compilar > Compilar Solução__ ou pressione __Ctrl+Shift+B__.
6.  **Execute a aplicação:**
    Defina o projeto desejado como projeto de inicialização (se aplicável) e pressione __F5__ ou clique no botão __Iniciar__ no Visual Studio para executar a aplicação.
    Acesse a aplicação usando a URL fornecida na janela de saída (por exemplo, `http://localhost:5000`).

## Creator
Este projeto foi criado por [MauryDev](https://github.com/MauryDev).