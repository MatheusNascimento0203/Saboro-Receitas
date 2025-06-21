CREATE TABLE "CategoriaFavorita" (
    "Id" SERIAL PRIMARY KEY,
    "NomeCategoria" VARCHAR(50) NOT NULL,
)

CREATE TABLE "NivelCulinario" (
    "Id" SERIAL PRIMARY KEY,
    "NomeNivel" VARCHAR(50) NOT NULL,
)

CREATE TABLE "UsuarioStatus" (
    "Id" INT PRIMARY KEY,
    "NomeStatus" VARCHAR(50) NOT NULL,
)

CREATE TABLE "DificuldadeReceita" (
    "Id" INT PRIMARY KEY,
    "Dificuldade" VARCHAR(50) NOT NULL
);

CREATE TABLE "Receita" (
    "Id" SERIAL PRIMARY KEY,
    "IdUsuario" INT NOT NULL,
    "IdDificuldadeReceita" INT NOT NULL,
    "IdCategoriaFavorita" INT NOT NULL,
    "TituloReceita" VARCHAR(200) NOT NULL,
    "DescricaoReceita" VARCHAR(2000) NOT NULL,
    "TempoPreparo" INT NOT NULL,
    "QtdPorcoes" INT NOT NULL,
    "DataCadastro" DATE NOT NULL DEFAULT CURRENT_DATE,
    "UsuarioUltimaAlteracao" INT,
    "DataUltimaAlteracao" DATE,

    FOREIGN KEY ("IdUsuario") REFERENCES "Usuario"("Id"),
    FOREIGN KEY ("IdDificuldadeReceita") REFERENCES "DificuldadeReceita"("Id"),
    FOREIGN KEY ("IdCategoriaFavorita") REFERENCES "CategoriaFavorita"("Id")
);

CREATE TABLE "ModoPreparo" (
    "Id" SERIAL PRIMARY KEY,
    "IdReceita" INT NOT NULL,
    "Ordem" INT NOT NULL,
    "Descricao" VARCHAR(200) NOT NULL,

    FOREIGN KEY ("IdReceita") REFERENCES "Receita"("Id")
);

CREATE TABLE "IngredienteReceita" (
    "Id" SERIAL PRIMARY KEY,
    "IdReceita" INT NOT NULL,
    "DescricaoIngrediente" INT NOT NULL,
    FOREIGN KEY ("IdReceita") REFERENCES "Receita"("Id")
);



CREATE TABLE "Usuario" (
    "Id" SERIAL PRIMARY KEY,
    "IdCategoriaFavorita" INT NULL,
    "IdNivelCulinario" INT NULL,
    "IdUsuarioStatus" INT NOT NULL,
    "NomeCompleto" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(254) NOT NULL UNIQUE,
    "Senha" VARCHAR(60) NOT NULL,
    "Biografia" VARCHAR(500),
    "UsuarioCadastro" INT NOT NULL,
    "DataCadastro" DATE NOT NULL DEFAULT CURRENT_DATE,
    "UsuarioUltimaAlteracao" INT,
    "DataUltimaAlteracao" DATE,
    "DataDesativacao" DATE,
    "TentativasInvalidas" INT DEFAULT 0,

    FOREIGN KEY ("IdCategoriaFavorita") REFERENCES "CategoriaFavorita"("Id"),
    FOREIGN KEY ("IdNivelCulinario") REFERENCES "NivelCulinario"("Id"),
    FOREIGN KEY ("IdUsuarioStatus") REFERENCES "UsuarioStatus"("Id")
)





