
# Processus de Travail

## Communication

Toutes les communications de l'équipe sont centralisées sur un serveur Discord dédié.

## Outils de Développement

- **Moteur de jeu** : Unity 6.2
- **Langage** : C#
- **IDE** : Selon les préférences.

## Répartition du Travail

La répartition des tâches évolue avec l'avancement du projet. Pour chaque nouvelle fonctionnalité ou tâche concrète, une issue est créée sur GitHub et assignée à un membre de l'équipe.

Chaque Pull Request (PR) génère une notification sur notre canal de travail Discord pour en informer l'équipe.

## Conventions de Codage

Pour garantir la lisibilité et la cohérence du code, nous suivons les [conventions de codage C# officielles de Microsoft](https://docs.microsoft.com/fr-fr/dotnet/csharp/fundamentals/coding-style/coding-conventions), qui sont la norme dans l'écosystème .NET et Unity.

## Stratégie de Test

Notre approche des tests se décompose comme suit :

- **Tests Unitaires et d'Intégration** : Nous utilisons le framework **Unity Test Framework** pour valider la logique du code (fonctions, classes) et les interactions entre les différents composants.
- **Tests Manuels (Système)** : Chaque fonctionnalité est testée manuellement dans l'éditeur Unity, puis une seconde fois dans une version exportée (`build`) du jeu pour s'assurer de son bon fonctionnement en conditions réelles.

## Flux de travail GitHub (GitHub Flow)

Notre processus de développement suit le modèle classique du GitHub Flow :

1.  **Création d'une Issue** : Chaque tâche ou fonctionnalité fait l'objet d'une issue.
2.  **Création de Branche** : Une branche est créée pour chaque issue.
3.  **Développement** : Implémentation du code nécessaire pour la fonctionnalité.
4.  **Pull Request** : Une fois le développement terminé, une Pull Request est ouverte pour proposer les changements.
5.  **Revue de Code** : Le code est relu et validé par au moins un autre membre de l'équipe.
6.  **Fusion** : Après validation, la branche est fusionnée (`merge`) dans la branche principale.

## Déploiement

Chaque nouvelle version du jeu est publiée sur notre page [itch.io](https://REMPLACER_PAR_LIEN_ITCH.IO) pour garantir un accès simple et public.

## Méthodologie Agile

Nous nous inspirons de la méthodologie AGILE en organisant un débriefing quotidien en fin de journée. Ce point rapide permet à chacun de :
-   Partager les difficultés rencontrées et solliciter de l'aide.
-   Valider collectivement les objectifs et la planification pour le lendemain.
