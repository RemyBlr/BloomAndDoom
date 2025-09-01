
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

## Gestion des Ressources (Assets)

Étant donné que nous ne créons pas nos propres ressources, nous utilisons des packs d'assets provenant principalement du Unity Asset Store. Pour maintenir un projet propre et organisé :

- **Stockage** : Les packs d'assets importés sont conservés dans un dossier dédié, par exemple `Assets/ThirdParty`, pour les isoler du reste du projet.
- **Utilisation via Prefabs** : Nous n'utilisons jamais directement un modèle 3D ou un asset d'un pack dans une scène. **Chaque asset doit d'abord être converti en un prefab** que nous configurons. Cela nous permet de modifier les assets sans craindre de perdre notre travail lors d'une mise à jour du pack d'origine.
- **Conventions de Nommage** : Les prefabs et autres ressources créées par nos soins suivent une convention de nommage claire (par exemple, `PRE_Player`, `MAT_Ground`, `TEX_Rock_Diffuse`).

## Gestion des Scènes

Les fichiers de scène (`.unity`) sont des fichiers binaires qui ne se prêtent pas bien à la fusion (`merge`) sur Git, ce qui peut causer des conflits difficiles à résoudre. Pour éviter ces problèmes, nous adoptons les règles suivantes :

- **Travail exclusif sur une scène** : Pour prévenir les conflits, une seule personne à la fois modifie une scène donnée. Une coordination s'effectue avant d'intervenir sur une scène partagée pour assurer l'intégrité des branches.
- **Privilégier les Prefabs** : La majorité du travail s'effectue sur des **prefabs**. Les scènes servent principalement à assembler ces prefabs, car la modification d'un prefab est plus sûre et plus facile à fusionner que celle d'une scène.

## Déploiement

Chaque nouvelle version du jeu est publiée sur notre page [itch.io](https://REMPLACER_PAR_LIEN_ITCH.IO) pour garantir un accès simple et public.

## Méthodologie Agile

Nous nous inspirons de la méthodologie AGILE en organisant un débriefing quotidien en début de journée. Ce point rapide permet à chacun de :
-   Partager les difficultés rencontrées la veille et solliciter de l'aide.
-   Valider collectivement les objectifs et la planification pour la journée même.
Tous les sprints ont été définis sur le Discord, voici ce qui à été décidé pour ces 3 semaines.

### Sprint planning 1
![Sprint planning 1](./img/sprint_planning_1.png)

### Sprint review 1
![Sprint review 1](./img/sprint_review_1.png)

### Sprint retrsopective 1
![Sprint retrsopective 1](./img/sprint_retro_1.png)

### Sprint planning 2
![Sprint planning 2](./img/sprint_planning_2.png)

### Sprint review 2
![Sprint review 2](./img/sprint_review_2.png)

### Sprint retrsopective 2
![Sprint retrsopective 2](./img/sprint_retro_2.png)

### Sprint planning 3
![Sprint planning 3](./img/sprint_planning_3.png)