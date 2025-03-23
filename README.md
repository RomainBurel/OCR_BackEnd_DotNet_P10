# MediLabo

## Description
MediLabo est un projet d�velopp� dans le cadre du parcours .NET de l'OpenClassrooms.
Ce projet vise � cr�er une application en architecture microservices � destination des m�decins.
L'objectif est de les aider � d�tecter les facteurs de risques de d�clenchement d'un diab�te chez un patient.

## Fonctionnalit�s
Pour cela, l'application devra permettre au m�decin :
- de pouvoir g�rer les donn�es g�n�rales concernant les patients
- de pouvoir ajouter / modifier / supprimer des notes pour chaque patient
- de visualiser en direct un indicateur de risque de diab�te calcul� � partir des donn�es et des notes du patient

## Pr�requis
- .NET SDK 9.0
- Visual Studio ou tout autre IDE compatible avec .NET
- SQL Server pour la base de donn�es des patients
- MongoDB pour la base de donn�es des notes
- Docker pour ex�cuter l'application en mode "Container"

## Architecture
L'application est d�coup�e en microservices :
- FrontendMVC : interface de l'application
- Gateway : passerelle g�rant la communication entre le Frontend et le Backend
- IdentityAPI : gestion des utilisateurs et de la connexion � l'application
- PatientsAPI : gestion des donn�es g�n�rales des patients avec une base de donn�es SQL (SQL Server)
- NotesAPI : gestion des notes des patients avec une base de donn�e NoSQL (MongoDB)
- DiabeteAPI : calcul du risque de diab�te pour un patient

## Utilisation
Lancez l'application via votre IDE ou en utilisant les lignes de commande :
   ```bash
   dotnet run --project ./IdentityAPI/IdentityAPI.csproj
   ```
   ```bash
   dotnet run --project ./PatientsAPI/PatientsAPI.csproj
   ```
   ```bash
   dotnet run --project ./NotesAPI/NotesAPI.csproj
   ```
   ```bash
   dotnet run --project ./DiabeteAPI/DiabeteAPI.csproj
   ```
   ```bash
   dotnet run --project ./Gateway/Gateway.csproj
   ```
   ```bash
   dotnet run --project ./FrontendMVC/FrontendMVC.csproj
   ```
OU
   ```bash
   docker-compose up -d
   ```

## Green Code
** D�finition
Le Green Code (ou d�veloppement �co-responsable) vise � r�duire l'impact environnemental du code informatique en optimisant les performances, la consommation �nerg�tique et les ressources utilis�es.
L'objectif est d'�crire un code plus efficace, qui consomme moins d'�nergie et de bande passante, tout en restant performant et maintenable.

** Propositions de mise en place
- Impl�menter un cache m�moire pour r�duire les appels aux API
- S'assurer que les DTOs ne renvoient que les donn�es n�cessaires pour limiter les flux de donn�es
- Frontend : ne charger les infos que lorsqu'elles sont demand�es, par exemple les notes ne pourraient �tre affich�es que lorsqu�on clique sur un bouton / idem pour le rapport de risque
- Utilisation des conteneurs avec auto-scaling et mise en veille
- Optimiser les traitements potentiellement gourmands en CPU

