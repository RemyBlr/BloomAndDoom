
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

- **Tests Manuels (Système)** : Chaque fonctionnalité est testée manuellement dans l'éditeur Unity, puis une seconde fois dans une version exportée (`build`) du jeu pour s'assurer de son bon fonctionnement en conditions réelles.

## Flux de travail GitHub (GitHub Flow)

Notre processus de développement suit le modèle classique du GitHub Flow :

1.  **Création d'une Issue** : Chaque tâche ou fonctionnalité fait l'objet d'une issue.
2.  **Création de Branche** : Une branche est créée pour chaque issue.
3.  **Développement** : Implémentation du code nécessaire pour la fonctionnalité.
4.  **Pull Request** : Une fois le développement terminé, une Pull Request est ouverte pour proposer les changements.
5.  **Revue de Code** : Le code est relu et validé par au moins un autre membre de l'équipe.
6.  **Fusion** : Après validation, la branche est fusionnée (`merge`) dans la branche principale.

## CI/CD
En ce qui concerne la CI/CD, nous avons mis en place à l'aide de [Game-ci](https://game.ci) un workflow basique. Lors de chaque PR (étape 4 du paragraphe précédent), le workflow se lance et essaie de build le projet pour Windows et pour MacOS. Si les builds passent, un artifact est crée. L'artifact étant l'éxectuable de notre jeu.
Nous avons lié Github et Discord, de se fait, nous étions toujour tenu au courant de l'état des builds. Voici comment cela se présentait :

![github on discord](./img/github_discrod.png)

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

Chaque nouvelle version du jeu est ajoutée manuellement sur la [Landing page](https://remyblr.github.io/BloomAndDoom). Nous avons testé plusieurs façons pour que les liens sur la landing page soient mis à jour automatiquement, mais malheureusement aucune ne fonctionnait.

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

## Points négatifs
Nous n'avons pas complètement atteint notre objectif durant ces 3 sprints, le but était d'avoir un jeu complètement fonctionnel, avec des règles de base simples et 5 niveaux au total. Nous avions très bien commencé en travaillant chacun de notre côté lors du premier sprint. Mais lors du deuxième sprint, lors de la mise en commun, nous nous sommes aperçus que nous avions manqué de discussion entre nous. Chacun avait mis en place un système pour la vie ou pour les dégats de son côté, et lorsqu'il fallait mettre en commun, nous avons perdu beaucoup de temps à tout faire fonctionner correctement. Avec plus de communication à l'avance et lors du développement de ces fonctionnalités, nous aurions pu avancer beaucoup plus rapidement, avec un code cohérent et propre, pour finir notre projet à temps.

Un autre point négatif, peut-être pas le meilleur endroit où en parler mais il n'y en a pas d'autre, est que nous sommes très limité par LFS qui est programme tier de Github. LFS sert à mettre les gros fichiers, comme les images, qui ne vont pas sur Github à cause de leur taille, sur un serveur connexe. Il n'y a aucune indication lors de sa mise en place, mais après une certaine limite, il faut payer pour ajouter d'autres fichiers, sans ajouter des fonds sur Github, le main est bloqué.
