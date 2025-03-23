# MediLabo

## Description
MediLabo est un projet développé dans le cadre du parcours .NET de l'OpenClassrooms.
Ce projet vise à créer une application en architecture microservices à destination des médecins.
L'objectif est de les aider à détecter les facteurs de risques de déclenchement d'un diabète chez un patient.

## Fonctionnalités
Pour cela, l'application devra permettre au médecin :
- de pouvoir gérer les données générales concernant les patients
- de pouvoir ajouter / modifier / supprimer des notes pour chaque patient
- de visualiser en direct un indicateur de risque de diabète calculé à partir des données et des notes du patient

## Prérequis
- .NET SDK 9.0
- Visual Studio ou tout autre IDE compatible avec .NET
- SQL Server pour la base de données des patients
- MongoDB pour la base de données des notes
- Docker pour exécuter l'application en mode "Container"

## Architecture
L'application est découpée en microservices :
- FrontendMVC : interface de l'application
- Gateway : passerelle gérant la communication entre le Frontend et le Backend
- IdentityAPI : gestion des utilisateurs et de la connexion à l'application
- PatientsAPI : gestion des données générales des patients avec une base de données SQL (SQL Server)
- NotesAPI : gestion des notes des patients avec une base de donnée NoSQL (MongoDB)
- DiabeteAPI : calcul du risque de diabète pour un patient

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
** Définition
Le Green Code (ou développement éco-responsable) vise à réduire l'impact environnemental du code informatique en optimisant les performances, la consommation énergétique et les ressources utilisées.
L'objectif est d'écrire un code plus efficace, qui consomme moins d'énergie et de bande passante, tout en restant performant et maintenable.

** Propositions de mise en place
- Implémenter un cache mémoire pour réduire les appels aux API
- S'assurer que les DTOs ne renvoient que les données nécessaires pour limiter les flux de données
- Frontend : ne charger les infos que lorsqu'elles sont demandées, par exemple les notes ne pourraient être affichées que lorsqu’on clique sur un bouton / idem pour le rapport de risque
- Utilisation des conteneurs avec auto-scaling et mise en veille
- Optimiser les traitements potentiellement gourmands en CPU

