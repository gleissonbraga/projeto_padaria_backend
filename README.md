# Projeto Backend - Padaria

Este projeto é o backend da aplicação **Padaria**, desenvolvido em .NET. Este guia irá te ajudar a fazer o pull do código, configurar o ambiente e rodar a aplicação usando Docker Compose.

---
## 1. Pré-requisitos

Para rodar este projeto, você precisará ter o Docker e o .NET 7.0 SDK instalados em sua máquina.

- **Docker:**
  Você pode baixar o Docker Desktop diretamente do site oficial. Ele já inclui o Docker Compose, que será usado para orquestrar os contêineres.
  [Link para download do Docker](https://www.docker.com/products/docker-desktop/)

- **.NET 7.0 SDK:**
  O SDK é necessário para compilar e rodar a aplicação .NET, caso você precise fazer alguma alteração localmente.
  [Link para download do .NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)

---
## 2. Clonando o Repositório

Primeiro, clone o repositório do projeto para sua máquina local. Abra o terminal e use o comando `git clone`:

git clone <URL_DO_SEU_REPOSITORIO>

## 3. Configurando as Variáveis de Ambiente
O projeto utiliza o Docker Compose para orquestrar os serviços. Você precisará criar dois arquivos de ambiente (.env) para configurar o banco de dados e outras variáveis.

a) container-postgres.env
Crie um arquivo chamado container-postgres.env na raiz do projeto e adicione o seguinte conteúdo:

Bash

POSTGRES_USER= # Nome de usuário do PostgreSQL
POSTGRES_PASSWORD= # Senha do PostgreSQL
POSTGRES_DB= # Nome do banco de dados

PGADMIN_DEFAULT_EMAIL= # E-mail de login para o pgAdmin
PGADMIN_DEFAULT_PASSWORD= # Senha de login para o pgAdmin

JWT_SECRET=ACHAVEMAISDOIDADODESENVOLVEMTODOO32BYTES # Chave secreta para a autenticação JWT
b) postgres.env
Em seguida, crie um arquivo chamado postgres.env na mesma pasta. Este arquivo conterá as variáveis de conexão que o backend usará para se conectar ao banco de dados.

Atenção: Os dados abaixo são apenas um modelo. Seus colegas devem preencher com as informações corretas que foram fornecidas, mas mantendo a estrutura.

Bash

DB_HOST= # Host do banco de dados (o nome do serviço no Docker)<br>
DB_PORT= # Porta para conexão com o banco de dados<br>
DB_NAME= # Nome do banco de dados<br>
DB_USER= # Nome de usuário do banco de dados<br>
DB_PASS= # Senha do usuário do banco de dados<br>

JWT_SECRET=ACHAVEMAISDOIDADODESENVOLVEMTODOO32BYTES # Chave secreta para a autenticação JWT
## 4. Iniciando o Projeto
Com os arquivos de ambiente criados, você pode iniciar todos os serviços (o backend e o banco de dados) usando o Docker Compose. Execute o seguinte comando na raiz do projeto:

Bash

docker-compose up -d --build
docker-compose up: Inicia os contêineres definidos no arquivo docker-compose.yml.

-d: Roda os contêineres em detached mode (em segundo plano).

--build: Força a recriação das imagens dos contêineres, garantindo que as versões mais recentes sejam usadas.

Depois de rodar o comando, o projeto estará ativo e acessível na porta configurada no docker-compose.yml.

## 5. Parando o Projeto
Para parar os contêineres e remover as redes, execute:

Bash

docker-compose down
