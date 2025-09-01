# Documentation - Ajouter une nouvelle classe

## Objectif
Cette documentation explique les différentes étapes pour ajouter une nouvelle classe jouable à notre jeu. Elle permet
à n'importe quelle personne qui reprend le projet, même sans connaissance du code, de créer une classe avec ses sorts, armes
, stats et modèle.

## Avantages
Nous avons opté pour cette architecture, car elle offre les avantages suivants :
- Modularité -> Il n'y a pas besoin d'ajouter du code pour ajouter une nouvelle classe (à part les actions spécifiques de la classe).
- Lisbilité -> Toutes les données liées à une classe sont centralisées dans un ScriptableObject propre à la classe.
- Extensibilité -> Il est facile d'ajouter une nouvelle classe.
- Accessibilité -> Il est facile de le faire pour une personne qui n'est que très peu expérimentée avec Unity ou le projet en lui même en suivant les instructions qui vont suivre.

## Étapes
### 1. Créer un **ScriptableObject**
1. Se rendre dans le dossier qui contient toutes les classes. Il est important de créer la classe dans ce dossier, car la sélection du personnage génère dynamiquement les classes affichées. Le dossier se trouve dans **Assets -> Resources -> Classes**.
2. Dans ce dossier, faire un clic droit et **Create -> Character -> CharacterClass**.
3. Cliquer sur le fichier **CharacterClass.asset** nouvellement créer.
4. Dans l'inspector (fenêtre s'ouvrant sur la partie droite de l'écran par défaut), compléter les différents champs:
    - className -> Nom de la classe affiché lors de la sélection.
    - Base stats -> Ce sont les statistiques de base du personnage (celles qui augmente en montant de niveau).
    - Advanced stats -> Ce sont des statistiques avancées (n'augmente *pas* lors d'un nouveau niveau).
    - Resources -> Les resrouces avec lesquelles la classe commence une partie.
    - Per level up bonus -> À chaque montée de niveau, la classe recevra un bonus de stat indiqué dans cette section.
    - Preview -> Chaque classe a besoin de 3 assets, 1 qui est l'image de la tête de la classe (côté gauche de la sélection de classe), 1 autre qui est le prefab qui sera affiché au centre de l'écran la classe sélectionnée, et finalement un autre préfab de la classe qui lui contient tous les scripts (c'est ce prefab qui est chargé lors du démarage de la partie).
    - Spells -> Les personnages sont pour l'instant designé (au niveau de l'UI) pour avoir 3 sorts (2 sorts actifs et 1 passif). Il n'y a pas de limite pour les sorts, mais il vous faut en créer 3 et remplir le champ icon (image du sort) et description (epxlication du sort qui est affiché lors de la sélection de classe).
    - Weapons -> Chaque personnage commence avec une arme par défaut, et le joueur peut en acheter une, voir plusieurs, lors de la sélection de classe. Ces armes sont ajoutées dynamiquement à la sélection de class, il n'y a donc thériquement pas de limite. Il faut remplire les différents champs, le nom, une image qui montre l'arme, une checkbox pour l'arme par défaut et finalement le prix d'achat.

### 2. Créer les prefab du personnage
1. Se rendre dans le dossier contenant tous les prefabs.
2. Ajouter un modèle 3D du modèle
3. Dupliquer le modèle avant toute modification, c'est ce modèle qui servira comme preview de la classe
4. Ajouter tous les composants nécessaires (Animator, Character controller, Player input, Character stats, Player controller, Animation state control et action spécifiqu à la classe).
5. Ajuster les scales des 2 prefabs.
6. Enregistrer les 2 modèles en tant que prefab (il suffit de glisser le modèle de la hierachie dans le dossier souhaité)

### 3. Verification
1. Il suffit de se rendre sur la scène **CharacterSelection** et de la lancer.
2. Sur le panneau gauche, une nouvelle icone apparaît.
3. Sur le clique de cette icone, le personnage et son arme devrait apparaître dans le panneau du milieu. Ses statistiques ainsi que ses sort devraient apparaître dans le panneau de droite.
4. Sur l'appui du bouton **Jouer**, on apparaît avec la nouvelle classe dans le premier niveau.