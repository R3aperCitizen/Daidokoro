CREATE DATABASE IF NOT EXISTS daidokoro;
USE daidokoro;

CREATE TABLE IF NOT EXISTS utente(
    IdUtente INT NOT NULL AUTO_INCREMENT,
    Username VARCHAR(20) NOT NULL,
    Foto MEDIUMBLOB NOT NULL,
    Pwd VARCHAR(30) NOT NULL,
    Email VARCHAR(30) NOT NULL,
    Esperienza INT DEFAULT 0,
    Livello INT DEFAULT 1,
    PRIMARY KEY (IdUtente)
)AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS categoria_nutrizionale(
    IdCategoria INT NOT NULL AUTO_INCREMENT,
    Nome VARCHAR(30) NOT NULL,
    PRIMARY KEY (IdCategoria)
)AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS obiettivo(
    IdObiettivo INT NOT NULL AUTO_INCREMENT,
    Nome VARCHAR(30) NOT NULL,
    Descrizione VARCHAR(255) NOT NULL,
    Esperienza INT NOT NULL,
    IdCategoria INT NOT NULL,
    PRIMARY KEY (IdObiettivo),
    FOREIGN KEY (IdCategoria) REFERENCES categoria_nutrizionale(IdCategoria)
)AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS obiettivo_ottenuto(
    IdObiettivo INT NOT NULL,
    IdUtente INT NOT NULL,
    DataOttenimento DATE NOT NULL,
    FOREIGN KEY (IdObiettivo) REFERENCES obiettivo(IdObiettivo),
    FOREIGN KEY (IdUtente) REFERENCES utente(IdUtente),
    PRIMARY KEY (IdObiettivo, IdUtente)
);

CREATE TABLE IF NOT EXISTS valore_nutrizionale(
    IdValoreNutrizionale INT NOT NULL AUTO_INCREMENT,
    Calorie FLOAT NOT NULL,
    Grassi FLOAT NOT NULL,
    Grassi_Saturi FLOAT NOT NULL,
    Carboidrati FLOAT NOT NULL,
    Zucchero FLOAT NOT NULL,
    Fibre FLOAT NOT NULL,
    Proteine FLOAT NOT NULL,
    Sale FLOAT NOT NULL,
    IdIngrediente INT NOT NULL,
    PRIMARY KEY (IdValoreNutrizionale)
)AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS ingrediente(
    IdIngrediente INT NOT NULL AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL,
    Descrizione VARCHAR(255) NOT NULL,
    IdValoreNutrizionale INT NOT NULL,
    IdCategoria INT NOT NULL,
    PRIMARY KEY (IdIngrediente),
    FOREIGN KEY (IdCategoria) REFERENCES categoria_nutrizionale(IdCategoria)
)AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS ricetta(
    IdRicetta INT NOT NULL AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL,
    Descrizione VARCHAR(255) NOT NULL,
    Passaggi JSON NOT NULL,
    Foto MEDIUMBLOB NOT NULL,
    Difficolta INT NOT NULL,
    Tempo INT NOT NULL,
    DataCreazione DATE NOT NULL,
    IdUtente INT NOT NULL,
    PRIMARY KEY (IdRicetta),
    FOREIGN KEY (IdUtente) REFERENCES utente(IdUtente)
)AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS likes (
	IdUtente INT NOT NULL,
	IdRicetta INT NOT NULL,
	Data DATE NOT NULL,
	FOREIGN KEY (IdUtente) REFERENCES utente(IdUtente),
	FOREIGN KEY (IdRicetta) REFERENCES ricetta(IdRicetta),
	PRIMARY KEY (IdUtente, IdRicetta)
);

CREATE TABLE IF NOT EXISTS ingrediente_ricetta(
    IdIngrediente INT NOT NULL,
    IdRicetta INT NOT NULL,
    PesoInGrammi INT NOT NULL,
    FOREIGN KEY (IdIngrediente) REFERENCES ingrediente(IdIngrediente),
    FOREIGN KEY (IdRicetta) REFERENCES ricetta(IdRicetta),
    PRIMARY KEY (IdIngrediente, IdRicetta)
);

CREATE TABLE IF NOT EXISTS collezione(
    IdCollezione INT NOT NULL AUTO_INCREMENT,
    Nome VARCHAR(50) NOT NULL,
    Descrizione VARCHAR(255) NOT NULL,
    Dieta TINYINT(1) NOT NULL,
    DataCreazione DATE NOT NULL,
    IdUtente INT NOT NULL,
    IdCategoria INT NOT NULL,
    PRIMARY KEY (IdCollezione),
    FOREIGN KEY (IdUtente) REFERENCES utente(IdUtente),
    FOREIGN KEY (IdCategoria) REFERENCES categoria_nutrizionale(IdCategoria)
)AUTO_INCREMENT=1;

CREATE TABLE IF NOT EXISTS ricetta_collezione(
    IdRicetta INT NOT NULL,
    IdCollezione INT NOT NULL,
    FOREIGN KEY (IdRicetta) REFERENCES ricetta(IdRicetta),
    FOREIGN KEY (IdCollezione) REFERENCES collezione(IdCollezione),
    PRIMARY KEY (IdRicetta, IdCollezione)
);

CREATE TABLE IF NOT EXISTS valutazione_ricetta(
    IdRicetta INT NOT NULL,
    IdUtente INT NOT NULL,
    Voto TINYINT(1) NOT NULL,
    DataValutazione DATETIME NOT NULL,
    Commento VARCHAR(255) DEFAULT '',
    PRIMARY KEY (IdUtente, IdRicetta),
    FOREIGN KEY (IdUtente) REFERENCES utente(IdUtente),
    FOREIGN KEY (IdRicetta) REFERENCES ricetta(IdRicetta)
);

CREATE TABLE IF NOT EXISTS valutazione_collezione(
    IdCollezione INT NOT NULL,
    IdUtente INT NOT NULL,
    Voto TINYINT(1) NOT NULL,
    DataValutazione DATETIME NOT NULL,
    Commento VARCHAR(255) DEFAULT '',
    PRIMARY KEY (IdUtente, IdCollezione),
    FOREIGN KEY (IdUtente) REFERENCES utente(IdUtente),
    FOREIGN KEY (IdCollezione) REFERENCES collezione(IdCollezione)
);

ALTER TABLE ingrediente
ADD FOREIGN KEY (IdValoreNutrizionale) REFERENCES valore_nutrizionale(IdValoreNutrizionale);

ALTER TABLE valore_nutrizionale
ADD FOREIGN KEY (IdIngrediente) REFERENCES ingrediente(IdIngrediente);

INSERT INTO categoria_nutrizionale (Nome) VALUES ("Generico");
INSERT INTO obiettivo (Nome, Descrizione, Esperienza, IdCategoria) VALUES ("Benvenuto", "Ti sei registrato a Daidokoro!", 100, 1);